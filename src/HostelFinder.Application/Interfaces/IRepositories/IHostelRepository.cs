using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IHostelRepository : IBaseGenericRepository<Hostel>
{
    Task<bool> CheckDuplicateHostelAsync(string hostelName, string province, string district, string commune, string detailAddress);
    Task<IEnumerable<Hostel>> GetHostelsByLandlordIdAsync(Guid landlordId);
    Task<Hostel> GetHostelWithReviewsByPostIdAsync(Guid postId);
    Task<Hostel> GetHostelByIdAsync(Guid postId);

}
