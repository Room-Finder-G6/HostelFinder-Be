using AutoMapper;
using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Application.DTOs.Amenity.Response;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services;

public class AmenityService : IAmenityService
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    
    public AmenityService(IAmenityRepository amenityRepository, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
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

    public async Task<List<AmenityResponse>> GetAllAmenitiesAsync()
    {
        var amenities = await _amenityRepository.GetAmenitiesAsync();
        var amenityResponses = _mapper.Map<List<AmenityResponse>>(amenities);
        return amenityResponses;
    }
}