using Market.Enums;

namespace Market.Controllers;

public class SearchProductDTO
{
    public string? ProductName { get; set; }
    public SortType? SortType { get; set; }
    public ProductCategory? Category { get; set; }
    public bool Ascending { get; set; } = true;
    public int Skip { get; set; }= 0;
    public int Take { get; set; }= 50;
}