namespace DATN_70.Models.ViewModels;

public sealed class HomeIndexViewModel
{
    public List<HomeBannerViewModel> Banners { get; set; } = [];
}

public sealed class HomeBannerViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string LinkUrl { get; set; } = string.Empty;
}

public sealed class BannerManagementViewModel
{
    public List<BannerListItemViewModel> Items { get; set; } = [];
}

public sealed class BannerListItemViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string LinkUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
