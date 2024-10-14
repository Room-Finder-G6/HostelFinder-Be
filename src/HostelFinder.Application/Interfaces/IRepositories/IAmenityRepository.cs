using HostelFinder.Application.Common;
using HostelFinder.Domain.Entities;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace HostelFinder.Application.Interfaces.IRepositories;

public interface IAmenityRepository : IBaseGenericRepository<Amenity>
{
    Task<Amenity> AddAmenityAsync(Amenity amenity);
    Task<List<Amenity>> GetAmenitiesAsync();
}