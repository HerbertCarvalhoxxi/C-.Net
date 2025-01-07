namespace APICatalogo.Pagination
{
    public class CategoryParams
    {
        const int MaxPageSize = 50;
        private int _pageSize;
        public int PageNumber { get; set; }
        public int PageSize {
            get {
                return _pageSize; 
                }
            set {
                 _pageSize = (value) > MaxPageSize ? _pageSize : value;
                }
        }
    }
}
