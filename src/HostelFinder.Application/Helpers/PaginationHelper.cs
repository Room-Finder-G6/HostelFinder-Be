using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Helpers
{
    public class PaginationHelper
    {
        public static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData, int pageNumber, int pageSize, int totalRecords)
        {
            var response = new PagedResponse<List<T>>(pagedData, pageNumber, pageSize);

            // Calculate total pages
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            response.TotalPages = totalPages;
            response.TotalRecords = totalRecords;

            // Add pagination metadata
            response.FirstPage = new Uri($"?PageNumber=1&PageSize={pageSize}", UriKind.Relative);
            response.LastPage = new Uri($"?PageNumber={totalPages}&PageSize={pageSize}", UriKind.Relative);

            response.NextPage = pageNumber < totalPages
                ? new Uri($"?PageNumber={pageNumber + 1}&PageSize={pageSize}", UriKind.Relative)
                : null;

            response.PreviousPage = pageNumber > 1
                ? new Uri($"?PageNumber={pageNumber - 1}&PageSize={pageSize}", UriKind.Relative)
                : null;

            return response;
        }
    }
}
