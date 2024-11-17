using HostelFinder.Application.DTOs.Vehicle.Request;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IVehicleService
    {
        Task<Response<AddVehicleDto>> AddVehicleAsync(AddVehicleDto request);
    }
}
