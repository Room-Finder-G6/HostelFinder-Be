using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services;

public class AmenityService : IAmenityService
{
    private readonly IAmenityRepository _amenityRepository;
    
    public AmenityService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }
    
    public async Task<Amenity> AddAmenityAsync(AddAmenityDto addAmenityDto)
    {
        var amenity = new Amenity
        {
            AmenityName = addAmenityDto.AmenityName,
            IsSelected = false
        };

        return await _amenityRepository.AddAmenityAsync(amenity);
    }

    public async Task<Response<bool>> DeleteAmenityAsync(Guid amenityId)
    {
        var amenity = await _amenityRepository.GetByIdAsync(amenityId);
        if (amenity == null)
        {
            return new Response<bool>("Amenity not found");
        }
        await _amenityRepository.DeletePermanentAsync(amenityId);
        return new Response<bool>("Amenity deleted successfully");
    }
}