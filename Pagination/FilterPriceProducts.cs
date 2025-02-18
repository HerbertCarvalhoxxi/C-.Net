namespace APICatalogo.Pagination;

public class FilterPriceProducts : QueryStringParameters
{
    public decimal? Price {  get; set; }
    public string? CriterionPrice { get; set; }
}
