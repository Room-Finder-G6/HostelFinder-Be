using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.DTOs.RentalContract.Response;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface ITenantService
    {
        Task<TenantResponse> AddTenentServiceAsync(AddTenantDto request);

        //Lấy ra thông tin thông tin của người thuê phòng của từng phòng
        Task<List<InformationTenacyReponseDto>> GetInformationTenacyAsync(Guid roomId);
        Task<Response<string>> AddRoommateAsync(AddRoommateDto request);
    }
}
