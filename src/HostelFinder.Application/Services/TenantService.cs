using AutoMapper;
using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.DTOs.RentalContract.Response;
using HostelFinder.Application.DTOs.Room.Responses;
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
        private readonly IRoomTenancyRepository _roomTenancyRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRentalContractRepository _rentalContractRepository;

        public TenantService(ITenantRepository tenantRepository,
            IMapper mapper,
            IS3Service s3Service,
            IRoomTenancyRepository roomTenancyRepository,
            IRentalContractRepository rentalContractRepository,
            IRoomRepository roomRepository)
        {
            _tenantRepository = tenantRepository;
            _mapper = mapper;
            _s3Service = s3Service;
            _roomTenancyRepository = roomTenancyRepository;
            _rentalContractRepository = rentalContractRepository;
            _roomRepository = roomRepository;
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
               throw new Exception(ex.Message);
            }
        }
        //lấy ra thông tin người thuê phòng tại phòng nào đó
        public async Task<List<InformationTenacyReponseDto>> GetInformationTenacyAsync(Guid roomId)
        {
            try
            {
                // lấy ra bản hợp đồng mới nhất
                var roomTenacy = await _roomTenancyRepository.GetRoomTenacyByIdAsync(roomId);

                //map to response 
                var informationTenacyRoomListDto = _mapper.Map<List<InformationTenacyReponseDto>>(roomTenacy);
                return informationTenacyRoomListDto;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Response<string>> AddRoommateAsync(AddRoommateDto request)
        {
            // Kiểm tra phòng tồn tại
            var room = await _roomRepository.GetByIdAsync(request.RoomId);
            if (room == null)
            {
                return new Response<string> { Message = "Phòng không tồn tại", Succeeded = false };
            }

            // Kiểm tra số lượng tenant hiện tại trong phòng
            var currentTenantsCount = await _roomTenancyRepository.CountCurrentTenantsAsync(request.RoomId);
            if (currentTenantsCount >= room.MaxRenters)
            {
                return new Response<string> { Message = "Phòng đã đầy, không thể thêm người thuê", Succeeded = false };
            }

            // Tạo tenant mới từ DTO
            var tenant = _mapper.Map<Tenant>(request);
            tenant.CreatedOn = DateTime.Now;

            // Upload ảnh (giả sử bạn sử dụng AWS S3)
            if (request.AvatarImage != null)
            {
                tenant.AvatarUrl = await _s3Service.UploadFileAsync(request.AvatarImage);
            }

            tenant.FrontImageUrl = await _s3Service.UploadFileAsync(request.FrontImageImage);
            tenant.BackImageUrl = await _s3Service.UploadFileAsync(request.BackImageImage);

            // Thêm tenant vào database
            var tenantCreated = await _tenantRepository.AddAsync(tenant);

            // Tạo bản ghi RoomTenancy để liên kết tenant và room
            var roomTenancy = new RoomTenancy
            {
                TenantId = tenantCreated.Id,
                RoomId = request.RoomId,
                MoveInDate = DateTime.Now
            };

            // Thêm bản ghi RoomTenancy
            await _roomTenancyRepository.AddAsync(roomTenancy);

            return new Response<string> { Message = "Thêm người thuê vào phòng thành công", Succeeded = true };
        }
    }
}
