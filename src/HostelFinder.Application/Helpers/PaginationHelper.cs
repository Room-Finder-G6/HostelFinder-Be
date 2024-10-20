using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Helpers
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, int pageNumber, int pageSize, int totalRecords)
        {
            var response = new PagedResponse<List<T>>(pagedData, pageNumber, pageSize);
            var totalPages = ((double)totalRecords) /(double)pageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }
    }
}
