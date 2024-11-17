using AutoMapper;
using HostelFinder.Application.DTOs.Vehicle.Request;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IS3Service _s3Service;
        private readonly IMapper _mapper;
        private readonly ITenantRepository _tenantRepository;

        public VehicleService(IVehicleRepository vehicleRepository,
            IS3Service s3Service,
            IMapper mapper,
            ITenantRepository tenantRepository)
        {
            _vehicleRepository = vehicleRepository;
            _s3Service = s3Service;
            _mapper = mapper;
            _tenantRepository = tenantRepository;
        }
        public async Task<Response<AddVehicleDto>> AddVehicleAsync(AddVehicleDto request)
        {
            try
            {
                var tenant = await _tenantRepository.GetByIdAsync(request.TenantId);
                if (tenant == null)
                {
                    return new Response<AddVehicleDto> { Succeeded = false, Message = "Không tìm thấy người thuê trọ" };
                }

                //upload image vehical to aws
                var uploadVehicalImageToAWS = await _s3Service.UploadFileAsync(request.Image);

                var hehicle = _mapper.Map<Vehicle>(request);
                hehicle.ImageUrl = uploadVehicalImageToAWS;

                var createVehicalOfTerant = await _vehicleRepository.AddAsync(hehicle);
                var vehicalDto = _mapper.Map<AddVehicleDto>(createVehicalOfTerant);

                return new Response<AddVehicleDto> { Data = vehicalDto, Succeeded = true, Message = $"Thêm xe của người thuê  {tenant.FullName}" };
            }
            catch (Exception ex)
            {
                return new Response<AddVehicleDto> { Errors = new List<string>() { ex.Message } };
            }

        }
    }
}
