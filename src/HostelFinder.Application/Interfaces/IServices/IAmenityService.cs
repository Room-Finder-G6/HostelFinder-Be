using HostelFinder.Application.DTOs.Amenity.Request;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IAmenityService
{
    public Task<Amenity> AddAmenityAsync(AddAmenityDto addAmenityDto);
}