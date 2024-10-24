using HostelFinder.Application.Common;
using HostelFinder.Domain.Common.Constants;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IHostelRepository : IBaseGenericRepository<Hostel>
{
    Task<bool> CheckDuplicateHostelAsync(string hostelName, string province, string district, string commune, string detailAddress);
    Task<IEnumerable<Hostel>> GetHostelsByUserIdAsync(Guid landlordId);
    Task<Hostel> GetHostelWithReviewsByPostIdAsync(Guid postId);
    Task<Hostel?> GetHostelByIdAsync(Guid postId);
    Task<(IEnumerable<Hostel> Data, int TotalRecords)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
    Task<Hostel> GetHostelByIdAndUserIdAsync(Guid hostelId,Guid userId);
}
