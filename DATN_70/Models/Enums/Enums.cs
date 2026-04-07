namespace DATN_70.Models.Enums
{
    public class Enums
    {
        // 1. Trạng thái Hóa Đơn
        public enum TrangThaiHoaDon
        {
            ChoDuyet = 0,
            DangGiao = 1,
            HoanThanh = 2,
            DaHuy = 3
        }
        // 2. Loại giao dịch
        public enum LoaiGiaoDich
        {
            Online = 0,
            PosTaiQuay = 1
        }
        // 3. Giới tính chung cho Khách Hàng & Nhân Viên
        public enum GioiTinh
        {
            Nam = 0,
            Nu = 1,
            Khac = 2
        }
        // 4. Trạng thái hoạt động
        public enum TrangThaiHoatDong
        {
            NgungHoatDong = 0,
            HoatDong = 1
        }
        //5. Kiểu thanh toán
        public enum KieuThanhToan
        {
            Online = 0,
            Offline = 1
        }
        //6. Trạng thái thanh toán
        public enum TrangThaiThanhToan
        {
            ThatBai = 0,
            ThanhCong = 1
        }
    }
}
