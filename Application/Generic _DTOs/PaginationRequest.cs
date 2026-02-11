namespace Application.Generic_DTOs
{
    public class PaginationRequest
    {
        public int PageSize { get; set; }=10;
        public int PageIndex { get; set; }=0;
    }
}