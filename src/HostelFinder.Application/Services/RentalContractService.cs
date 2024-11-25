using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using HostelFinder.Application.DTOs.RentalContract.Request;
using HostelFinder.Application.DTOs.Room.Responses;
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
        private readonly IMeterReadingService _meterReadingService;
        public RentalContractService(
            IRentalContractRepository rentalContractRepository,
            IRoomRepository roomRepository,
            IRoomTenancyRepository roomTenancyRepository,
            ITenantService tenantService,
            IMapper mapper,
            IMeterReadingService meterReadingService)
        {
            _rentalContractRepository = rentalContractRepository;
            _roomRepository = roomRepository;
            _roomTenancyRepository = roomTenancyRepository;
            _tenantService = tenantService;
            _mapper = mapper;
            _meterReadingService = meterReadingService;
        }
        public async Task<Response<string>> CreateRentalContractAsync(AddRentalContractDto request)
        {
            try
            {
                // kiểm tra xem hợp đồng đã hết hạn chưa ?
                var checkExpiredContract = await _rentalContractRepository.CheckExpiredContractAsync(request.RoomId, request.StartDate, request.EndDate);
                // Nếu khác null là thì không nằm trong thời gian hợp đồng
                if (checkExpiredContract != null)
                {
                    return new Response<string> { Succeeded = false, Message = $"Hiện tại đã có hợp đồng tồn tại trong khoảng thời gian {checkExpiredContract.StartDate} - {checkExpiredContract.EndDate}." +
                        $" Hoặc bạn có thể cập nhập lại thời hạn hợp đồng " };
                }

                // Kiểm tra số người thuê trọ hiện tại trong phòng
                var room = await _roomRepository.GetRoomByIdAsync(request.RoomId);
                var currentTenantsCount = await _roomTenancyRepository.CountCurrentTenantsAsync(room.Id);
                if (currentTenantsCount >= room.MaxRenters)
                {
                    //throw new Exception("Phòng đã đạt số lượng người thuê tối đa");
                    return new Response<string> { Succeeded = false, Message = "Phòng hiện tại đã đạt tối đa số lượng người thuê" };
                }

              

                // tạo người thuê phòng
                var tenantCreated = await _tenantService.AddTenentServiceAsync(request.AddTenantDto);

                // tạo hợp đồng
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


                // kiểm tra số lượng trong phòng
                if(currentTenantsCount + 1 >= 1)
                {
                    room.IsAvailable = false;
                   await _roomRepository.UpdateAsync(room);
                }

                // ghi số dịch vụ tại thời điểm hợp đồng
                //await _meterReadingService.AddMeterReadingAsync(room.Id, request.ServiceId, request.Reading, DateTime.Now.Month, DateTime.Now.Year);
                var listMeterReaderingSevice = request.AddMeterReadingServiceDto;
                foreach(var readingService in listMeterReaderingSevice)
                {
                    await _meterReadingService.AddMeterReadingAsync(room.Id, readingService.ServiceId, readingService.Reading, DateTime.Now.Month, DateTime.Now.Year);
                }

                return new Response<string> { Succeeded = true, Message = $"Tạo hợp đồng cho thuê thành công cho phòng {room.RoomName} với người thuê : {tenantCreated.FullName}" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Succeeded = false, Errors = new List<string> { ex.Message}, Message = ex.Message };
            }
        }

        public async Task<RoomContractHistoryResponseDto> GetRoomContractHistoryLasest(Guid roomId)
        {
            try
            {
                // lấy ra thông tin của hợp đồng theo room
                var getRoomContract = await _rentalContractRepository.GetRoomRentalContrctByRoom(roomId);
                if(getRoomContract == null)
                {
                    return null;
                } 
                var roomrentalContractResponseDto = _mapper.Map<RoomContractHistoryResponseDto>(getRoomContract);
                return roomrentalContractResponseDto;
                    
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RoomContractHistoryResponseDto> GetRoomContractHistoryLasest(Guid roomId)
        {
            try
            {
                // lấy ra thông tin của hợp đồng theo room
                var getRoomContract = await _rentalContractRepository.GetRoomRentalContrctByRoom(roomId);
                if(getRoomContract == null)
                {
                    return null;
                } 
                var roomrentalContractResponseDto = _mapper.Map<RoomContractHistoryResponseDto>(getRoomContract);
                return roomrentalContractResponseDto;
                    
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
