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

        public async Task<Response<List<ServiceCostResponseDto>>> GetAllAsync()
        {
            var serviceCosts = await _serviceCostRepository.ListAllAsync();
            var result = _mapper.Map<List<ServiceCostResponseDto>>(serviceCosts);
            return new Response<List<ServiceCostResponseDto>>(result);
        }

        public async Task<Response<ServiceCostResponseDto>> GetByIdAsync(Guid id)
        {
            var serviceCost = await _serviceCostRepository.GetByIdAsync(id);
            if (serviceCost == null)
                return new Response<ServiceCostResponseDto>("Service cost not found.");

            var result = _mapper.Map<ServiceCostResponseDto>(serviceCost);
            return new Response<ServiceCostResponseDto>(result);
        }

        public async Task<Response<ServiceCostResponseDto>> CreateAsync(AddServiceCostDto serviceCostDto)
        {
            var serviceCost = _mapper.Map<ServiceCost>(serviceCostDto);
            serviceCost = await _serviceCostRepository.AddAsync(serviceCost);

            var result = _mapper.Map<ServiceCostResponseDto>(serviceCost);
            return new Response<ServiceCostResponseDto>(result, "Service cost created successfully.");
        }

        public async Task<Response<ServiceCostResponseDto>> UpdateAsync(Guid id, UpdateServiceCostDto serviceCostDto)
        {
            var serviceCost = await _serviceCostRepository.GetByIdAsync(id);
            if (serviceCost == null)
                return new Response<ServiceCostResponseDto>("Service cost not found.");

            _mapper.Map(serviceCostDto, serviceCost);
            serviceCost = await _serviceCostRepository.UpdateAsync(serviceCost);

            var result = _mapper.Map<ServiceCostResponseDto>(serviceCost);
            return new Response<ServiceCostResponseDto>(result, "Service cost updated successfully.");
        }

        public async Task<Response<bool>> DeleteAsync(Guid id)
        {
            var serviceCost = await _serviceCostRepository.GetByIdAsync(id);
            if (serviceCost == null)
                return new Response<bool>("Service cost not found.");

            await _serviceCostRepository.DeletePermanentAsync(id);
            return new Response<bool>(true, "Service cost deleted successfully.");
        }

    }
}
