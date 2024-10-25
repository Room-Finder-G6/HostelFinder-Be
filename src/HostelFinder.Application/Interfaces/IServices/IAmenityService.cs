using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.Amenity.Response;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IAmenityService
{
    public Task<AmenityResponse> AddAmenityAsync(AddAmenityDto addAmenityDto);
    public Task<Response<bool>> DeleteAmenityAsync(Guid amenityId);
    public Task<List<AmenityResponse>> GetAllAmenitiesAsync();
}