using DATN_70.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DATN_70.Data;

public sealed class StorefrontDataSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StorefrontDataSeeder> _logger;

    public StorefrontDataSeeder(
        AppDbContext dbContext,
        ILogger<StorefrontDataSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {       
        await SeedDanhMucAsync(cancellationToken);
        await SeedThuongHieuAsync(cancellationToken);
        await SeedKichCoAsync(cancellationToken);
        await SeedMauAsync(cancellationToken);       
        await SeedSanPhamAsync(cancellationToken);
        await SeedChiTietSanPhamAsync(cancellationToken);

        _logger.LogInformation("Storefront seed data is ready via EF Core.");
    }

    private async Task SeedDanhMucAsync(CancellationToken cancellationToken)
    {
        var items = new[]
        {
                new DanhMuc { DanhMucID = "DM001", Ten = "Áo Khoác Nam" },
                new DanhMuc { DanhMucID = "DM002", Ten = "Áo Len & Hoodie" },
                new DanhMuc { DanhMucID = "DM003", Ten = "Phụ Kiện Mùa Đông" }
            };

        foreach (var item in items)
        {
            if (!await _dbContext.DanhMucs.AnyAsync(x => x.DanhMucID == item.DanhMucID, cancellationToken))
                _dbContext.DanhMucs.Add(item);
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    private async Task SeedThuongHieuAsync(CancellationToken cancellationToken)
    {
        var items = new[]
        {
                new ThuongHieu { ThuongHieuID = "TH001", Ten = "Arctic Wear", LogoURL = "", MoTa = "Chuyên đồ hàn đới" },
                new ThuongHieu { ThuongHieuID = "TH002", Ten = "Urban Style", LogoURL = "", MoTa = "Phong cách đường phố" }
            };

        foreach (var item in items)
        {
            if (!await _dbContext.ThuongHieus.AnyAsync(x => x.ThuongHieuID == item.ThuongHieuID, cancellationToken))
                _dbContext.ThuongHieus.Add(item);
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedKichCoAsync(CancellationToken cancellationToken)
    {
        var items = new[]
        {
            new KichCo { KichCoID = "SZS", Ten = "Size S", MoTa = string.Empty },
            new KichCo { KichCoID = "SZM", Ten = "Size M", MoTa = string.Empty },
            new KichCo { KichCoID = "SZL", Ten = "Size L", MoTa = string.Empty },
            new KichCo { KichCoID = "SZX", Ten = "Size XL", MoTa = string.Empty }
        };

        foreach (var item in items)
        {
            if (!await _dbContext.KichCos.AnyAsync(x => x.KichCoID == item.KichCoID, cancellationToken))
            {
                _dbContext.KichCos.Add(item);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedMauAsync(CancellationToken cancellationToken)
    {
        var items = new[]
        {
            new Mau { MauID = "BLK", Ten = "Đen Onyx" },
            new Mau { MauID = "CRM", Ten = "Kem Cashmere" },
            new Mau { MauID = "GRN", Ten = "Xanh Rêu" },
            new Mau { MauID = "BRN", Ten = "Nâu Cocoa" },
            new Mau { MauID = "GRY", Ten = "Ghi Khói" }
        };

        foreach (var item in items)
        {
            var existing = await _dbContext.Maus.FirstOrDefaultAsync(x => x.MauID == item.MauID, cancellationToken);
            if (existing is null)
            {
                _dbContext.Maus.Add(item);
            }
            else if (existing.Ten != item.Ten)
            {
                existing.Ten = item.Ten;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedSanPhamAsync(CancellationToken cancellationToken)
    {
        var items = new[]
        {
            new SanPham { SanPhamID = "SP0001", Ten = "Ao Phao Arctic Shield", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Form dai giu nhiet tot, be mat can gio nhe va phu hop cho nhung ngay lanh sau. Thiet ke toi gian de phoi cung jeans, len va boots." },
            new SanPham { SanPhamID = "SP0002", Ten = "Ao Da Sherpa Espresso", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM002", ThuongHieuID = "TH002", MoTa = "Chat lieu gia da mem, lot long am ap va tao diem nhan thanh lich cho phong cach thanh pho mua dong." },
            new SanPham { SanPhamID = "SP0003", Ten = "Sweater Alpine Soft Knit", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM003", ThuongHieuID = "TH002", MoTa = "Mau sweater len mem nhe, tay raplan de mac layering ca ngay ma van gon dang." },
            new SanPham { SanPhamID = "SP0004", Ten = "Ao Mangto Wool Blend", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM003", ThuongHieuID = "TH001", MoTa = "Thiet ke mangto dang suong, tong mau tram sang, hop cho outfit cong so va di choi cuoi tuan." },
            new SanPham { SanPhamID = "SP0005", Ten = "Hoodie Fleece Cloudline", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM002", ThuongHieuID = "TH002", MoTa = "Ni fleece day dan, giu am nhanh va cho cam giac thoai mai trong nhung ngay se lanh." },
            new SanPham { SanPhamID = "SP0006", Ten = "Gile Phao Urban Heat", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Gile phao gon nhe, de phoi layer voi hoodie hoac ao len, hop cho di chuyen hang ngay." },
            new SanPham { SanPhamID = "SP0001", Ten = "Áo Phao Arctic Shield", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Form dài giữ nhiệt tốt, bề mặt cản gió nhẹ và phù hợp cho những ngày lạnh sâu. Thiết kế tối giản để phối cùng jeans, len và boots." },
            new SanPham { SanPhamID = "SP0002", Ten = "Áo Da Sherpa Espresso", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Chất liệu giả da mềm, lót lông ấm áp và tạo điểm nhấn thanh lịch cho phong cách thành phố mùa đông." },
            new SanPham { SanPhamID = "SP0003", Ten = "Sweater Alpine Soft Knit", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Mẫu sweater len mềm nhẹ, tay raglan để mặc layering cả ngày mà vẫn gọn dáng." },
            new SanPham { SanPhamID = "SP0004", Ten = "Áo Măng Tô Wool Blend", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Thiết kế măng tô dáng suông, tông màu trầm sang, hợp cho outfit công sở và đi chơi cuối tuần." },
            new SanPham { SanPhamID = "SP0005", Ten = "Hoodie Fleece Cloudline", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Nỉ fleece dày dặn, giữ ấm nhanh và cho cảm giác thoải mái trong những ngày se lạnh." },
            new SanPham { SanPhamID = "SP0006", Ten = "Gile Phao Urban Heat", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Gile phao gọn nhẹ, dễ phối layer với hoodie hoặc áo len, hợp cho di chuyển hằng ngày." },
            new SanPham { SanPhamID = "SP0007", Ten = "Áo Khoác Ngắn Metro Zip", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Áo khoác ngắn dáng gọn, hợp với set đồ đi học và đi làm trong ngày trở gió." },
            new SanPham { SanPhamID = "SP0008", Ten = "Áo Len Merino Ease", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Áo len mỏng nhẹ, giữ nhiệt vừa đủ và phù hợp để mặc layer trong mọi ngày." },
            new SanPham { SanPhamID = "SP0009", Ten = "Áo Giữ Nhiệt Core Warm", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Lớp áo giữ nhiệt co giãn, ôm vừa cơ thể và mặc lót bên trong rất linh hoạt." },
            new SanPham { SanPhamID = "SP0010", Ten = "Parka Snow Ranger", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Parka mũ lông nhân tạo, kháng gió tốt và phù hợp cho chuyến đi xa ngày lạnh." },
            new SanPham { SanPhamID = "SP0011", Ten = "Cardigan Layer Softline", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Cardigan dáng mở, chất len mềm và dễ phối cùng thun, sơ mi hoặc áo giữ nhiệt." },
            new SanPham { SanPhamID = "SP0012", Ten = "Bomber Frost Street", ChatLieu = "Vải Phao Chống Nước", DanhMucID = "DM001", ThuongHieuID = "TH001", MoTa = "Bomber dáng ngắn, phom trẻ trung, phù hợp phong cách phố và di chuyển hằng ngày." }
        };

        foreach (var item in items)
        {
            var existing = await _dbContext.SanPhams.FirstOrDefaultAsync(x => x.SanPhamID == item.SanPhamID, cancellationToken);
            if (existing is null)
            {
                _dbContext.SanPhams.Add(item);
            }
            else
            {
                existing.Ten = item.Ten;
                existing.MoTa = item.MoTa;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedChiTietSanPhamAsync(CancellationToken cancellationToken)
    {
        var items = new[]
        {
            NewChiTietSanPham("CT0001", "SP0001", "SZM", "BLK", 1290000m, 18),
            NewChiTietSanPham("CT0002", "SP0001", "SZL", "BLK", 1290000m, 12),
            NewChiTietSanPham("CT0003", "SP0001", "SZX", "CRM", 1350000m, 7),
            NewChiTietSanPham("CT0004", "SP0002", "SZM", "BRN", 1490000m, 10),
            NewChiTietSanPham("CT0005", "SP0002", "SZL", "BRN", 1490000m, 6),
            NewChiTietSanPham("CT0006", "SP0002", "SZL", "BLK", 1520000m, 5),
            NewChiTietSanPham("CT0007", "SP0003", "SZS", "CRM", 790000m, 16),
            NewChiTietSanPham("CT0008", "SP0003", "SZM", "GRY", 790000m, 14),
            NewChiTietSanPham("CT0009", "SP0003", "SZL", "GRN", 820000m, 11),
            NewChiTietSanPham("CT0010", "SP0004", "SZM", "GRY", 1890000m, 9),
            NewChiTietSanPham("CT0011", "SP0004", "SZL", "CRM", 1890000m, 4),
            NewChiTietSanPham("CT0012", "SP0004", "SZX", "BLK", 1950000m, 6),
            NewChiTietSanPham("CT0013", "SP0005", "SZM", "GRN", 690000m, 22),
            NewChiTietSanPham("CT0014", "SP0005", "SZL", "GRY", 690000m, 13),
            NewChiTietSanPham("CT0015", "SP0005", "SZX", "BLK", 720000m, 8),
            NewChiTietSanPham("CT0016", "SP0006", "SZS", "CRM", 890000m, 12),
            NewChiTietSanPham("CT0017", "SP0006", "SZM", "BLK", 890000m, 15),
            NewChiTietSanPham("CT0018", "SP0006", "SZL", "GRN", 920000m, 9),
            NewChiTietSanPham("CT0019", "SP0007", "SZS", "GRY", 990000m, 14),
            NewChiTietSanPham("CT0020", "SP0007", "SZM", "BLK", 1020000m, 11),
            NewChiTietSanPham("CT0021", "SP0007", "SZL", "CRM", 1020000m, 8),
            NewChiTietSanPham("CT0022", "SP0008", "SZS", "CRM", 720000m, 17),
            NewChiTietSanPham("CT0023", "SP0008", "SZM", "GRN", 720000m, 16),
            NewChiTietSanPham("CT0024", "SP0008", "SZL", "BRN", 760000m, 10),
            NewChiTietSanPham("CT0025", "SP0009", "SZS", "BLK", 390000m, 25),
            NewChiTietSanPham("CT0026", "SP0009", "SZM", "GRY", 390000m, 20),
            NewChiTietSanPham("CT0027", "SP0009", "SZL", "CRM", 420000m, 18),
            NewChiTietSanPham("CT0028", "SP0010", "SZM", "GRN", 1590000m, 13),
            NewChiTietSanPham("CT0029", "SP0010", "SZL", "BLK", 1650000m, 9),
            NewChiTietSanPham("CT0030", "SP0010", "SZX", "CRM", 1690000m, 6),
            NewChiTietSanPham("CT0031", "SP0011", "SZS", "CRM", 680000m, 12),
            NewChiTietSanPham("CT0032", "SP0011", "SZM", "BRN", 720000m, 9),
            NewChiTietSanPham("CT0033", "SP0011", "SZL", "GRY", 720000m, 11),
            NewChiTietSanPham("CT0034", "SP0012", "SZM", "BLK", 1090000m, 10),
            NewChiTietSanPham("CT0035", "SP0012", "SZL", "BRN", 1090000m, 8),
            NewChiTietSanPham("CT0036", "SP0012", "SZX", "GRY", 1150000m, 5)
        };

        foreach (var item in items)
        {
            if (!await _dbContext.ChiTietSanPhams.AnyAsync(x => x.ChiTietSanPhamID == item.ChiTietSanPhamID, cancellationToken))
            {
                _dbContext.ChiTietSanPhams.Add(item);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static ChiTietSanPham NewChiTietSanPham(
        string id,
        string sanPhamId,
        string kichCoId,
        string mauId,
        decimal giaNiemYet,
        int soLuongTon)
    {
        return new ChiTietSanPham
        {
            ChiTietSanPhamID = id,
            SanPhamID = sanPhamId,
            KichCoID = kichCoId,
            MauID = mauId,
            SKU = string.Empty,
            GiaNiemYet = giaNiemYet,
            SoLuongTonKho = soLuongTon
        };
    }
}
