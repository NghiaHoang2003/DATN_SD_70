using DATN_70.Data;
using DATN_70.Models.Orders;
using DATN_70.Models.Products;
using Microsoft.Data.SqlClient;

namespace DATN_70.Services;

public sealed class StoreRepository : IStoreRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public StoreRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<ProductListItemResponse>> GetProductsAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                sp.SanPhamID,
                sp.Ten,
                sp.MoTa,
                MIN(ctsp.GiaNiemYet) AS GiaThapNhat,
                SUM(ctsp.SoLuongTon) AS TongSoLuongTon
            FROM SanPham sp
            LEFT JOIN ChiTietSanPham ctsp ON ctsp.SanPhamID = sp.SanPhamID
            GROUP BY sp.SanPhamID, sp.Ten, sp.MoTa
            ORDER BY sp.Ten;
            """;

        var products = new List<ProductListItemResponse>();

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            products.Add(new ProductListItemResponse
            {
                SanPhamID = reader.GetString(0),
                Ten = reader.GetString(1),
                MoTa = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                GiaThapNhat = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                TongSoLuongTon = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
            });
        }

        return products;
    }

    public async Task<ProductDetailResponse?> GetProductDetailAsync(string productId, CancellationToken cancellationToken)
    {
        const string productSql = """
            SELECT SanPhamID, Ten, MoTa
            FROM SanPham
            WHERE SanPhamID = @SanPhamID;
            """;

        const string variantSql = """
            SELECT
                ctsp.ChiTietSanPhamID,
                ctsp.KichCoID,
                kc.Ten,
                ctsp.MauID,
                m.Ten,
                ctsp.GiaNiemYet,
                ctsp.SoLuongTon
            FROM ChiTietSanPham ctsp
            INNER JOIN KichCo kc ON kc.KichCoID = ctsp.KichCoID
            INNER JOIN Mau m ON m.MauID = ctsp.MauID
            WHERE ctsp.SanPhamID = @SanPhamID
            ORDER BY kc.Ten, m.Ten;
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var product = await LoadProductAsync(connection, productSql, productId, cancellationToken);
        if (product is null)
        {
            return null;
        }

        await using var variantCommand = new SqlCommand(variantSql, connection);
        variantCommand.Parameters.AddWithValue("@SanPhamID", productId);

        await using var reader = await variantCommand.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            product.BienThe.Add(new ProductVariantResponse
            {
                ChiTietSanPhamID = reader.GetString(0),
                KichCoID = reader.GetString(1),
                TenKichCo = reader.GetString(2),
                MauID = reader.GetString(3),
                TenMau = reader.GetString(4),
                GiaNiemYet = reader.GetDecimal(5),
                SoLuongTon = reader.GetInt32(6)
            });
        }

        return product;
    }

    public async Task<ServiceResult<OrderCreatedResponse>> PlaceOrderAsync(
        PlaceOrderRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedItems = request.Items
            .GroupBy(item => item.ChiTietSanPhamID)
            .Select(group => new OrderItemRequest
            {
                ChiTietSanPhamID = group.Key,
                SoLuong = group.Sum(item => item.SoLuong)
            })
            .ToList();

        if (normalizedItems.Count == 0)
        {
            return ServiceResult<OrderCreatedResponse>.Fail("Don hang phai co it nhat 1 san pham.");
        }

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var orderId = GenerateId("HD", 20);
            var orderCreatedAt = DateTime.Now;
            decimal totalAmount = 0;
            var orderDetails = new List<(string DetailId, string ProductDetailId, int Quantity, decimal UnitPrice, decimal LineTotal)>();

            foreach (var item in normalizedItems)
            {
                var stockInfo = await GetStockInfoAsync(connection, transaction, item.ChiTietSanPhamID, cancellationToken);
                if (stockInfo is null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return ServiceResult<OrderCreatedResponse>.Fail($"Khong tim thay bien the {item.ChiTietSanPhamID}.");
                }

                var stock = stockInfo.Value;

                if (stock.SoLuongTon < item.SoLuong)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return ServiceResult<OrderCreatedResponse>.Fail(
                        $"San pham {item.ChiTietSanPhamID} chi con {stock.SoLuongTon} trong kho.");
                }

                var detailId = GenerateId("HDCT", 20);
                var lineTotal = stock.GiaNiemYet * item.SoLuong;
                totalAmount += lineTotal;

                orderDetails.Add((detailId, item.ChiTietSanPhamID, item.SoLuong, stock.GiaNiemYet, lineTotal));
            }

            await InsertOrderAsync(connection, transaction, orderId, request, orderCreatedAt, totalAmount, cancellationToken);

            foreach (var detail in orderDetails)
            {
                await InsertOrderDetailAsync(connection, transaction, orderId, detail, cancellationToken);
                await UpdateStockAsync(connection, transaction, detail.ProductDetailId, detail.Quantity, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return ServiceResult<OrderCreatedResponse>.Ok(new OrderCreatedResponse
            {
                HoaDonID = orderId,
                TenKhachHang = request.TenKhachHang,
                SoDienThoai = request.SoDienThoai,
                DiaChiGiaoHang = request.DiaChiGiaoHang,
                NgayTao = orderCreatedAt,
                TongTien = totalAmount,
                TrangThai = 0,
                ChiTiet = orderDetails.Select(detail => new OrderCreatedItemResponse
                {
                    HoaDonChiTietID = detail.DetailId,
                    ChiTietSanPhamID = detail.ProductDetailId,
                    SoLuong = detail.Quantity,
                    DonGia = detail.UnitPrice,
                    ThanhTien = detail.LineTotal
                }).ToList()
            });
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static string GenerateId(string prefix, int totalLength)
    {
        var suffixLength = totalLength - prefix.Length;
        var suffix = Guid.NewGuid().ToString("N")[..suffixLength];
        return prefix + suffix;
    }

    private static async Task<ProductDetailResponse?> LoadProductAsync(
        SqlConnection connection,
        string sql,
        string productId,
        CancellationToken cancellationToken)
    {
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@SanPhamID", productId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new ProductDetailResponse
        {
            SanPhamID = reader.GetString(0),
            Ten = reader.GetString(1),
            MoTa = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
        };
    }

    private static async Task<(decimal GiaNiemYet, int SoLuongTon)?> GetStockInfoAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string productDetailId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT GiaNiemYet, SoLuongTon
            FROM ChiTietSanPham WITH (UPDLOCK, ROWLOCK)
            WHERE ChiTietSanPhamID = @ChiTietSanPhamID;
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@ChiTietSanPhamID", productDetailId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return (reader.GetDecimal(0), reader.GetInt32(1));
    }

    private static async Task InsertOrderAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string orderId,
        PlaceOrderRequest request,
        DateTime createdAt,
        decimal totalAmount,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO HoaDon (
                HoaDonID,
                TenKhachHang,
                SoDienThoai,
                DiaChiGiaoHang,
                NgayTao,
                TongTien,
                TrangThai
            )
            VALUES (
                @HoaDonID,
                @TenKhachHang,
                @SoDienThoai,
                @DiaChiGiaoHang,
                @NgayTao,
                @TongTien,
                @TrangThai
            );
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@HoaDonID", orderId);
        command.Parameters.AddWithValue("@TenKhachHang", request.TenKhachHang);
        command.Parameters.AddWithValue("@SoDienThoai", request.SoDienThoai);
        command.Parameters.AddWithValue("@DiaChiGiaoHang", request.DiaChiGiaoHang);
        command.Parameters.AddWithValue("@NgayTao", createdAt);
        command.Parameters.AddWithValue("@TongTien", totalAmount);
        command.Parameters.AddWithValue("@TrangThai", 0);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task InsertOrderDetailAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string orderId,
        (string DetailId, string ProductDetailId, int Quantity, decimal UnitPrice, decimal LineTotal) detail,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO HoaDonChiTiet (
                HoaDonChiTietID,
                HoaDonID,
                ChiTietSanPhamID,
                SoLuong,
                DonGia,
                ThanhTien
            )
            VALUES (
                @HoaDonChiTietID,
                @HoaDonID,
                @ChiTietSanPhamID,
                @SoLuong,
                @DonGia,
                @ThanhTien
            );
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@HoaDonChiTietID", detail.DetailId);
        command.Parameters.AddWithValue("@HoaDonID", orderId);
        command.Parameters.AddWithValue("@ChiTietSanPhamID", detail.ProductDetailId);
        command.Parameters.AddWithValue("@SoLuong", detail.Quantity);
        command.Parameters.AddWithValue("@DonGia", detail.UnitPrice);
        command.Parameters.AddWithValue("@ThanhTien", detail.LineTotal);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static async Task UpdateStockAsync(
        SqlConnection connection,
        SqlTransaction transaction,
        string productDetailId,
        int quantity,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE ChiTietSanPham
            SET SoLuongTon = SoLuongTon - @SoLuong
            WHERE ChiTietSanPhamID = @ChiTietSanPhamID;
            """;

        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@SoLuong", quantity);
        command.Parameters.AddWithValue("@ChiTietSanPhamID", productDetailId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
