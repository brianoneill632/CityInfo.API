namespace CityInfo.API.Services
{
    public class PaginationMatadata(int totalCount, int pageSize, int currentPage)
    {
        public int TotalCount { get; set; } = totalCount;
        public int PageSize { get; set; } = pageSize;
        public int CurrentPage { get; set; } = currentPage;
        public int TotalPages { get; set; } = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
