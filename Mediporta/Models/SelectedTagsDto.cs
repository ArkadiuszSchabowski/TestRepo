namespace Mediporta.Models
{
    public class SelectedTagsDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; }
        public string Order { get; set; }
    }
}
