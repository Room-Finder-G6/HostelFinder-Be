using AutoMapper;
using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.DTOs.RentalContract.Response;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IMapper _mapper;
        private readonly IS3Service _s3Service;

        public TenantService(ITenantRepository tenantRepository,
            IMapper mapper,
            IS3Service s3Service)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _s3Service = s3Service;
        }

        public async Task<TenantResponse> AddTenentServiceAsync(AddTenantDto request)
        {
            try
            {
                var checkIdentityCardNumber = await _tenantRepository.GetByIdentityCardNumber(request.IdentityCardNumber);
                if (checkIdentityCardNumber != null)
                {
                    throw new Exception($"Đã tồn tại cccd của {checkIdentityCardNumber.FullName}");
                }
                var tenent = _mapper.Map<Tenant>(request);
                //upload image to cloud AWS
                if (request.AvatarImage != null)
                {
                    tenent.AvatarUrl = await _s3Service.UploadFileAsync(request.AvatarImage);
                }
                //upload image CCCD
                tenent.BackImageUrl = await _s3Service.UploadFileAsync(request.BackImageImage);
                tenent.FrontImageUrl = await _s3Service.UploadFileAsync(request.FrontImageImage);

                tenent.CreatedOn = DateTime.Now;


                var tenentCreated = await _tenantRepository.AddAsync(tenent);

                //map to Dto 
                var tenentCreatedDto = _mapper.Map<TenantResponse>(tenentCreated);
                return tenentCreatedDto;

            }
            catch (Exception ex)
            {
               throw new Exception("Lỗi khi tạo khách thuê phòng", ex);
            }
        }
    }
}
