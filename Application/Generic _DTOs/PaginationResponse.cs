namespace Application.Generic_DTOs
{
    public class PaginationResponse <T> where T : class
    {
        public List<T> Item { get; set; }
        public int Count { get; set; }
    }
}