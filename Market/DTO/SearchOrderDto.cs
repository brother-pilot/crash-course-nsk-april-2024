namespace Market.DTO;

public class SearchOrderDto
{
    //актуальные
    public bool IncludeDone{ get; set; }
    //отмененные
    public bool IncludeCancelled{ get; set; }
    
    //public SearchOrderType SearchOrderType { get; set; } = SearchOrderType.OnlyCreated;
}