using AutoMapper;
using HostelFinder.Application.DTOs.ServiceCost.Request;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class ServiceCostService : IServiceCostService
    {
        private readonly IServiceCostRepository _serviceCostRepository;
        private readonly IMapper _mapper;

        public ServiceCostService(IServiceCostRepository serviceCostRepository, IMapper mapper)
        {
            _serviceCostRepository = serviceCostRepository;
            _mapper = mapper;
        }

        public async Task<Response<ServiceCostResponseDto>> AddServiceCostAsync(AddServiceCostDto dto)
        {

            var serviceCost = _mapper.Map<ServiceCost>(dto);
            serviceCost.CreatedOn = DateTime.Now;
            serviceCost.CreatedBy = "System";
            try
            {
                await _serviceCostRepository.AddAsync(serviceCost);
                var serviceCostResponse = _mapper.Map<ServiceCostResponseDto>(serviceCost);
                return new Response<ServiceCostResponseDto>(serviceCostResponse, "Chi phí dịch vụ tạo thành công!");
            }
            catch (Exception ex)
            {
                return new Response<ServiceCostResponseDto>(message: ex.Message);
            }
        }

        public async Task<Response<ServiceCostResponseDto>> UpdateServiceCostAsync(Guid id, UpdateServiceCostDto dto)
        {
            var existingServiceCost = await _serviceCostRepository.GetByIdAsync(id);
            if (existingServiceCost == null)
            {
                return new Response<ServiceCostResponseDto>("Không tìm thấy chi phí dịch vụ cần cập nhật!");
            }

            try
            {
                _mapper.Map(dto, existingServiceCost);
                existingServiceCost.LastModifiedOn = DateTime.Now;
                existingServiceCost.LastModifiedBy = "System";

                await _serviceCostRepository.UpdateAsync(existingServiceCost);
                var updatedServiceCostDto = _mapper.Map<ServiceCostResponseDto>(existingServiceCost);
                return new Response<ServiceCostResponseDto>(updatedServiceCostDto, "Chi phí dịch vụ cập nhật thành công!");
            }
            catch (Exception ex)
            {
                return new Response<ServiceCostResponseDto>(message: ex.Message);
            }
        }

        public async Task<Response<bool>> DeleteServiceCostAsync(Guid id)
        {
            var serviceCost = await _serviceCostRepository.GetByIdAsync(id);
            if (serviceCost == null)
            {
                return new Response<bool>(false, "Không tìm thấy chi phí dịch vụ cần xóa!");
            }

            try
            {
                await _serviceCostRepository.DeletePermanentAsync(serviceCost.Id);
                return new Response<bool>(true, "Chi phí dịch vụ xóa thành công!");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, message: ex.Message);
            }
        }

    }
}
