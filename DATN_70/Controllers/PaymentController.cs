using DATN_70.Data;
using DATN_70.Models.Entities;
using DATN_70.Models.Enums;
using DATN_70.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayOS;
using PayOS.Models;
using PayOS.Models.V2.PaymentRequests;
using PayOS.Models.Webhooks;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DATN_70.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public class PaymentRequestModel
        {
            public int Amount { get; set; }
        }
        private readonly PayOSClient _payOSClient;
        private readonly AppDbContext _dbContext;

        public PaymentController(PayOSClient payOSClient, AppDbContext dbContext)
        {
            _payOSClient = payOSClient;
            _dbContext = dbContext;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] PayOS.Models.Webhooks.Webhook webhookBody)
        {
            try
            {
                // 1. Xác thực dữ liệu từ PayOS
                dynamic verifiedData = await _payOSClient.Webhooks.VerifyAsync(webhookBody);
                Console.WriteLine("=============================================");
                Console.WriteLine($"[TING TING] TIỀN ĐÃ VỀ TÀI KHOẢN!");
                Console.WriteLine($"Mã đơn hàng: {verifiedData.OrderCode}");
                Console.WriteLine($"Số tiền nhận: {verifiedData.Amount} VND");
                Console.WriteLine("=============================================");

                string currentOrderCodeStr = verifiedData.OrderCode.ToString();

                // 2. Tìm bản ghi thanh toán tương ứng
                var chiTietThanhToan = await _dbContext.Set<ChiTietThanhToan>()
                    .FirstOrDefaultAsync(c => c.MaThamChieu == currentOrderCodeStr);

                if (chiTietThanhToan == null)
                {
                    Console.WriteLine($"[WEBHOOK] Không tìm thấy giao dịch với mã: {currentOrderCodeStr}");
                    return Ok(new { success = true, message = "Không tìm thấy giao dịch tương ứng." });
                }

                // 3. Chống replay: nếu đã thành công rồi thì thôi
                if (chiTietThanhToan.TrangThai == Enums.TrangThaiThanhToan.ThanhCong)
                {
                    Console.WriteLine($"[WEBHOOK] Giao dịch {currentOrderCodeStr} đã được xử lý trước đó. Bỏ qua.");
                    return Ok(new { success = true, message = "Giao dịch đã được xử lý." });
                }

                // 4. Cập nhật trạng thái thanh toán -> Thành công
                chiTietThanhToan.TrangThai = Enums.TrangThaiThanhToan.ThanhCong;

                // 5. Tìm hóa đơn liên quan
                var hoaDon = await _dbContext.HoaDons
                    .Include(h => h.HoaDonChiTiets)
                    .FirstOrDefaultAsync(h => h.HoaDonID == chiTietThanhToan.HoaDonID);

                if (hoaDon != null)
                {
                    // Chỉ tự động xử lý nếu là đơn Online (LoaiGiaoDich = 0)
                    if (hoaDon.LoaiGiaoDich == Enums.LoaiGiaoDich.Online)
                    {
                        bool canConfirm = hoaDon.TrangThai == Enums.TrangThaiHoaDon.ChoDuyet ||
                                          hoaDon.TrangThai == Enums.TrangThaiHoaDon.DangChoThanhToanQR;

                        if (canConfirm)
                        {
                            // Trừ kho nếu là QR (trạng thái 7)
                            if (hoaDon.TrangThai == Enums.TrangThaiHoaDon.DangChoThanhToanQR)
                            {
                                var chiTietHoaDon = await _dbContext.HoaDonChiTiets
                                    .Where(hdct => hdct.HoaDonID == hoaDon.HoaDonID)
                                    .ToListAsync();

                                foreach (var item in chiTietHoaDon)
                                {
                                    var ctsp = await _dbContext.ChiTietSanPhams
                                        .FirstOrDefaultAsync(ct => ct.ChiTietSanPhamID == item.ChiTietSanPhamID);
                                    if (ctsp != null)
                                    {
                                        ctsp.SoLuongTonKho -= item.SoLuong;
                                    }
                                }
                            }

                            // Trừ khuyến mãi (nếu có)
                            if (!string.IsNullOrEmpty(hoaDon.KhuyenMaiID))
                            {
                                var khuyenMai = await _dbContext.KhuyenMais
                                    .FirstOrDefaultAsync(km => km.KhuyenMaiID == hoaDon.KhuyenMaiID);

                                if (khuyenMai != null && (khuyenMai.SoLuongToiDa == 0 || khuyenMai.SoLuongDaDung < khuyenMai.SoLuongToiDa))
                                {
                                    khuyenMai.SoLuongDaDung += 1;
                                    Console.WriteLine($"[WEBHOOK] Đã tăng số lần sử dụng cho khuyến mãi {khuyenMai.KhuyenMaiID} (Mã: {khuyenMai.MaCode ?? "Tự động"})");
                                }
                            }

                            // Chuyển trạng thái
                            hoaDon.TrangThai = Enums.TrangThaiHoaDon.DaXacNhan;
                            Console.WriteLine($"[WEBHOOK] Đơn hàng online {hoaDon.HoaDonID} đã được xác nhận tự động.");
                        }
                    }
                    else // Đơn POS: không tự động xác nhận, chỉ ghi nhận thanh toán
                    {
                        Console.WriteLine($"[WEBHOOK] Đơn hàng POS {hoaDon.HoaDonID} - chỉ ghi nhận thanh toán, chờ nhân viên xác nhận.");
                    }
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new { success = true, message = "Cập nhật thanh toán thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WEBHOOK BÁO LỖI]: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("create-online-payment")]
        public async Task<IActionResult> CreateOnlinePaymentUrl(
     [FromBody] CreateOnlinePaymentRequest request,
     CancellationToken cancellationToken)
        {
            // 1. Yêu cầu load Session nếu chưa có sẵn
            await HttpContext.Session.LoadAsync();
            var currentUserId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để thực hiện chức năng này." });
            }

            // 2. TÌM HÓA ĐƠN VÀ KIỂM TRA QUYỀN SỞ HỮU
            var hoaDon = await _dbContext.HoaDons
                .Include(h => h.ChiTietThanhToans)
                .FirstOrDefaultAsync(h => h.HoaDonID == request.HoaDonID, cancellationToken);

            if (hoaDon == null)
            {
                return NotFound(new { message = "Không tìm thấy hóa đơn." });
            }

            // Kiểm tra trạng thái: Chỉ tạo link khi hóa đơn ở trạng thái Chờ duyệt hoặc Chờ thanh toán QR
            if (hoaDon.TrangThai != Enums.TrangThaiHoaDon.ChoDuyet && hoaDon.TrangThai != Enums.TrangThaiHoaDon.DangChoThanhToanQR)
            {
                return BadRequest(new { message = $"Đơn hàng không ở trạng thái chờ thanh toán. Trạng thái hiện tại: {hoaDon.TrangThai}" });
            }

            // Xóa các yêu cầu thanh toán cũ chưa thành công (nếu có)
            if (hoaDon.ChiTietThanhToans != null)
            {
                var pendingPayments = hoaDon.ChiTietThanhToans
                    .Where(ct => ct.TrangThai == Enums.TrangThaiThanhToan.ThatBai)
                    .ToList();
                foreach (var pp in pendingPayments)
                {
                    _dbContext.Set<ChiTietThanhToan>().Remove(pp);
                }
            }

            // 3. SINH MÃ ORDERCODE DUY NHẤT
            long orderCode = GenerateUniquePayOSOrderCode();

            // 4. TẠO BẢN GHI CHI TIẾT THANH TOÁN
            var chiTietThanhToan = new ChiTietThanhToan
            {
                ChiTietThanhToanID = Guid.NewGuid().ToString(),
                HoaDonID = hoaDon.HoaDonID,
                Ten = "Thanh toán QR Online",
                SoTien = (decimal)hoaDon.ThanhTien,
                MaThamChieu = orderCode.ToString(),
                ThoiGianThanhToan = DateTime.Now,
                TrangThai = Enums.TrangThaiThanhToan.ThatBai,
                PhuongThucThanhToanID = "CASH"
            };
            _dbContext.Set<ChiTietThanhToan>().Add(chiTietThanhToan);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // 5. GỌI PAYOS ĐỂ TẠO LINK THANH TOÁN
            try
            {
                var paymentRequest = new CreatePaymentLinkRequest
                {
                    OrderCode = orderCode,
                    Amount = (int)hoaDon.ThanhTien,
                    Description = $"DH{orderCode}"[..Math.Min(25, orderCode.ToString().Length + 2)],
                    CancelUrl = "https://localhost:7220/Home/Cancel",
                    ReturnUrl = "https://localhost:7220/Home/Success"
                };

                var paymentLink = await _payOSClient.PaymentRequests.CreateAsync(paymentRequest);

                return Ok(new
                {
                    success = true,
                    checkoutUrl = paymentLink.CheckoutUrl,
                    orderCode = orderCode
                });
            }
            catch (Exception ex)
            {
                _dbContext.Set<ChiTietThanhToan>().Remove(chiTietThanhToan);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        private long GenerateUniquePayOSOrderCode()
        {
            var timestampPart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var randomPart = new Random().Next(100, 999).ToString();
            return long.Parse(timestampPart + randomPart);
        }

        [HttpDelete("cancel-qr/{hoaDonId}")]
        public async Task<IActionResult> CancelQRPayment(string hoaDonId)
        {
            var chiTiet = await _dbContext.Set<ChiTietThanhToan>()
                .FirstOrDefaultAsync(c => c.HoaDonID == hoaDonId && c.TrangThai == Enums.TrangThaiThanhToan.ThatBai);
            if (chiTiet != null)
            {
                _dbContext.Set<ChiTietThanhToan>().Remove(chiTiet);
            }

            var hoaDon = await _dbContext.HoaDons.FirstOrDefaultAsync(h => h.HoaDonID == hoaDonId);
            if (hoaDon != null && hoaDon.TrangThai == Enums.TrangThaiHoaDon.DangChoThanhToanQR)
            {
                hoaDon.TrangThai = Enums.TrangThaiHoaDon.DaHuy;
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpPost("create-qr-intent")]
        public async Task<IActionResult> CreateQRPaymentIntent([FromBody] PlaceOrderRequest request)
        {
            HttpContext.Session.SetString("LastPlaceOrderRequest", JsonSerializer.Serialize(request));

            long orderCode = GenerateUniquePayOSOrderCode();

            int tempAmount = 1000;
            var paymentRequest = new CreatePaymentLinkRequest
            {
                OrderCode = orderCode,
                Amount = tempAmount,
                Description = $"QR{orderCode}".Substring(0, 25),
                CancelUrl = "https://localhost:7220/Home/Cancel",
                ReturnUrl = "https://localhost:7220/Home/Success"
            };
            var paymentLink = await _payOSClient.PaymentRequests.CreateAsync(paymentRequest);

            return Ok(new { success = true, checkoutUrl = paymentLink.CheckoutUrl, orderCode });
        }
    }
}