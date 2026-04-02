using Microsoft.Data.SqlClient;

namespace DATN_70.Data;

public sealed class StorefrontDataSeeder
{
    private readonly SqlConnectionFactory _connectionFactory;
    private readonly ILogger<StorefrontDataSeeder> _logger;

    public StorefrontDataSeeder(
        SqlConnectionFactory connectionFactory,
        ILogger<StorefrontDataSeeder> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            IF OBJECT_ID(N'dbo.KichCo', N'U') IS NULL
            BEGIN
                CREATE TABLE KichCo (
                    KichCoID VARCHAR(10) PRIMARY KEY,
                    Ten NVARCHAR(50)
                );
            END;

            IF OBJECT_ID(N'dbo.Mau', N'U') IS NULL
            BEGIN
                CREATE TABLE Mau (
                    MauID VARCHAR(10) PRIMARY KEY,
                    Ten NVARCHAR(50)
                );
            END;

            IF OBJECT_ID(N'dbo.SanPham', N'U') IS NULL
            BEGIN
                CREATE TABLE SanPham (
                    SanPhamID VARCHAR(20) PRIMARY KEY,
                    Ten NVARCHAR(200),
                    MoTa NVARCHAR(MAX)
                );
            END;

            IF OBJECT_ID(N'dbo.ChiTietSanPham', N'U') IS NULL
            BEGIN
                CREATE TABLE ChiTietSanPham (
                    ChiTietSanPhamID VARCHAR(20) PRIMARY KEY,
                    SanPhamID VARCHAR(20) FOREIGN KEY REFERENCES SanPham(SanPhamID),
                    KichCoID VARCHAR(10) FOREIGN KEY REFERENCES KichCo(KichCoID),
                    MauID VARCHAR(10) FOREIGN KEY REFERENCES Mau(MauID),
                    GiaNiemYet DECIMAL(18,0),
                    SoLuongTon INT
                );
            END;

            IF OBJECT_ID(N'dbo.HoaDon', N'U') IS NULL
            BEGIN
                CREATE TABLE HoaDon (
                    HoaDonID VARCHAR(20) PRIMARY KEY,
                    TenKhachHang NVARCHAR(100),
                    SoDienThoai VARCHAR(15),
                    DiaChiGiaoHang NVARCHAR(255),
                    NgayTao DATETIME DEFAULT GETDATE(),
                    TongTien DECIMAL(18,0),
                    TrangThai INT DEFAULT 0
                );
            END;

            IF OBJECT_ID(N'dbo.HoaDonChiTiet', N'U') IS NULL
            BEGIN
                CREATE TABLE HoaDonChiTiet (
                    HoaDonChiTietID VARCHAR(20) PRIMARY KEY,
                    HoaDonID VARCHAR(20) FOREIGN KEY REFERENCES HoaDon(HoaDonID),
                    ChiTietSanPhamID VARCHAR(20) FOREIGN KEY REFERENCES ChiTietSanPham(ChiTietSanPhamID),
                    SoLuong INT,
                    DonGia DECIMAL(18,0),
                    ThanhTien DECIMAL(18,0)
                );
            END;

            IF NOT EXISTS (SELECT 1 FROM KichCo WHERE KichCoID = 'SZS')
                INSERT INTO KichCo (KichCoID, Ten) VALUES ('SZS', N'Size S');
            IF NOT EXISTS (SELECT 1 FROM KichCo WHERE KichCoID = 'SZM')
                INSERT INTO KichCo (KichCoID, Ten) VALUES ('SZM', N'Size M');
            IF NOT EXISTS (SELECT 1 FROM KichCo WHERE KichCoID = 'SZL')
                INSERT INTO KichCo (KichCoID, Ten) VALUES ('SZL', N'Size L');
            IF NOT EXISTS (SELECT 1 FROM KichCo WHERE KichCoID = 'SZX')
                INSERT INTO KichCo (KichCoID, Ten) VALUES ('SZX', N'Size XL');

            IF NOT EXISTS (SELECT 1 FROM Mau WHERE MauID = 'BLK')
                INSERT INTO Mau (MauID, Ten) VALUES ('BLK', N'Den Onyx');
            IF NOT EXISTS (SELECT 1 FROM Mau WHERE MauID = 'CRM')
                INSERT INTO Mau (MauID, Ten) VALUES ('CRM', N'Kem Cashmere');
            IF NOT EXISTS (SELECT 1 FROM Mau WHERE MauID = 'GRN')
                INSERT INTO Mau (MauID, Ten) VALUES ('GRN', N'Xanh Reu');
            IF NOT EXISTS (SELECT 1 FROM Mau WHERE MauID = 'BRN')
                INSERT INTO Mau (MauID, Ten) VALUES ('BRN', N'Nau Cocoa');
            IF NOT EXISTS (SELECT 1 FROM Mau WHERE MauID = 'GRY')
                INSERT INTO Mau (MauID, Ten) VALUES ('GRY', N'Ghi Khoi');

            IF NOT EXISTS (SELECT 1 FROM SanPham WHERE SanPhamID = 'SP0001')
                INSERT INTO SanPham (SanPhamID, Ten, MoTa) VALUES
                ('SP0001', N'Ao Phao Arctic Shield', N'Form dai giu nhiet tot, be mat can gio nhe va phu hop cho nhung ngay lanh sau. Thiet ke toi gian de phoi cung jeans, len va boots.');

            IF NOT EXISTS (SELECT 1 FROM SanPham WHERE SanPhamID = 'SP0002')
                INSERT INTO SanPham (SanPhamID, Ten, MoTa) VALUES
                ('SP0002', N'Ao Da Sherpa Espresso', N'Chat lieu gia da mem, lot long am ap va tao diem nhan thanh lich cho phong cach thanh pho mua dong.');

            IF NOT EXISTS (SELECT 1 FROM SanPham WHERE SanPhamID = 'SP0003')
                INSERT INTO SanPham (SanPhamID, Ten, MoTa) VALUES
                ('SP0003', N'Sweater Alpine Soft Knit', N'Mau sweater len mem nhe, tay raplan de mac layering ca ngay ma van gon dang.');

            IF NOT EXISTS (SELECT 1 FROM SanPham WHERE SanPhamID = 'SP0004')
                INSERT INTO SanPham (SanPhamID, Ten, MoTa) VALUES
                ('SP0004', N'Ao Mangto Wool Blend', N'Thiet ke mangto dang suong, tong mau tram sang, hop cho outfit cong so va di choi cuoi tuan.');

            IF NOT EXISTS (SELECT 1 FROM SanPham WHERE SanPhamID = 'SP0005')
                INSERT INTO SanPham (SanPhamID, Ten, MoTa) VALUES
                ('SP0005', N'Hoodie Fleece Cloudline', N'Ni fleece day dan, giu am nhanh va cho cam giac thoai mai trong nhung ngay se lanh.');

            IF NOT EXISTS (SELECT 1 FROM SanPham WHERE SanPhamID = 'SP0006')
                INSERT INTO SanPham (SanPhamID, Ten, MoTa) VALUES
                ('SP0006', N'Gile Phao Urban Heat', N'Gile phao gon nhe, de phoi layer voi hoodie hoac ao len, hop cho di chuyen hang ngay.');

            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0001')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0001', 'SP0001', 'SZM', 'BLK', 1290000, 18);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0002')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0002', 'SP0001', 'SZL', 'BLK', 1290000, 12);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0003')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0003', 'SP0001', 'SZX', 'CRM', 1350000, 7);

            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0004')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0004', 'SP0002', 'SZM', 'BRN', 1490000, 10);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0005')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0005', 'SP0002', 'SZL', 'BRN', 1490000, 6);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0006')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0006', 'SP0002', 'SZL', 'BLK', 1520000, 5);

            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0007')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0007', 'SP0003', 'SZS', 'CRM', 790000, 16);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0008')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0008', 'SP0003', 'SZM', 'GRY', 790000, 14);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0009')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0009', 'SP0003', 'SZL', 'GRN', 820000, 11);

            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0010')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0010', 'SP0004', 'SZM', 'GRY', 1890000, 9);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0011')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0011', 'SP0004', 'SZL', 'CRM', 1890000, 4);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0012')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0012', 'SP0004', 'SZX', 'BLK', 1950000, 6);

            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0013')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0013', 'SP0005', 'SZM', 'GRN', 690000, 22);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0014')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0014', 'SP0005', 'SZL', 'GRY', 690000, 13);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0015')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0015', 'SP0005', 'SZX', 'BLK', 720000, 8);

            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0016')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0016', 'SP0006', 'SZS', 'CRM', 890000, 12);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0017')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0017', 'SP0006', 'SZM', 'BLK', 890000, 15);
            IF NOT EXISTS (SELECT 1 FROM ChiTietSanPham WHERE ChiTietSanPhamID = 'CT0018')
                INSERT INTO ChiTietSanPham (ChiTietSanPhamID, SanPhamID, KichCoID, MauID, GiaNiemYet, SoLuongTon) VALUES
                ('CT0018', 'SP0006', 'SZL', 'GRN', 920000, 9);
            """;

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation("Storefront mock data is ready.");
    }
}
