using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.Amenity.Response;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IAmenityService
{
    Task<Response<AmenityResponse>> AddAmenityAsync(AddAmenityDto addAmenityDto);
    Task<Response<bool>> DeleteAmenityAsync(Guid amenityId);
    Task<Response<List<AmenityResponse>>> GetAllAmenitiesAsync();
}