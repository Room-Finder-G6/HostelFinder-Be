using AutoMapper;
using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class RentalContractService : IRentalContractService
    {
        private readonly IRentalContractRepository _rentalContractRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomTenancyRepository _roomTenancyRepository;
        private readonly ITenantService _tenantService;
        private readonly IMapper _mapper;
        public RentalContractService(
            IRentalContractRepository rentalContractRepository,
            IRoomRepository roomRepository,
            IRoomTenancyRepository roomTenancyRepository,
            ITenantService tenantService,
            IMapper mapper)
        {
            _rentalContractRepository = rentalContractRepository;
            _roomRepository = roomRepository;
            _roomTenancyRepository = roomTenancyRepository;
            _tenantService = tenantService;
            _mapper = mapper;
        }
        public async Task<Response<string>> CreateRentalContractAsync(AddRentalContractDto request)
        {
            try
            {
                var tenantCreated = await _tenantService.AddTenentServiceAsync(request.AddTenantDto);

                var room = await _roomRepository.GetRoomByIdAsync(request.RoomId);

                // Kiểm tra số người thuê trọ hiện tại trong phòng

                var currentTenantsCount = await _roomTenancyRepository.CountCurrentTenantsAsync(room.Id);
                if (currentTenantsCount >= room.MaxRenters)
                {
                    throw new Exception("Phòng đã đạt số lượng người thuê tối đa");
                }

                var rentalContract = new RentalContract
                {
                    TenantId = tenantCreated.Id,
                    RoomId = room.Id,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    MonthlyRent = request.MonthlyRent,
                    DepositAmount = request.DepositAmount,
                    PaymentCycleDays = request.PaymentCycleDays,
                    ContractTerms = request.ContractTerms,
                    CreatedOn = DateTime.Now,
                };

                await _rentalContractRepository.AddAsync(rentalContract);
                // tạo mới roomTenacy cho người thuê chính

                var roomTenacy = new RoomTenancy
                {
                    TenantId = tenantCreated.Id,
                    RoomId = room.Id,
                    MoveInDate = rentalContract.StartDate,
                    MoveOutDate = rentalContract.EndDate,
                };

                await _roomTenancyRepository.AddAsync(roomTenacy);

                if(currentTenantsCount + 1 >= room.MaxRenters)
                {
                    room.IsAvailable = false;
                   await _roomRepository.UpdateAsync(room);
                }
                return new Response<string> { Succeeded = true, Message = $"Tạo hợp đồng cho thuê thành công cho phòng {room.RoomName} với người thuê : {tenantCreated.FullName}" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Succeeded = false, Errors = new List<string> { ex.Message}, Message = "Lỗi xảy ra khi tạo hợp đồng" };
            }
        }
    }
}
