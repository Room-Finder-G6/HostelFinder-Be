using DocumentFormat.OpenXml.VariantTypes;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServiceRepository _serviceRepository;
       
        public MeterReadingService(IMeterReadingRepository meterReadingRepository, 
            IRoomRepository roomRepository, 
            IServiceRepository serviceRepository)
        {
            _meterReadingRepository = meterReadingRepository;
            _roomRepository = roomRepository;
            _serviceRepository = serviceRepository;
        }
        public async Task<Response<string>> AddMeterReadingAsync(Guid roomId, Guid serviceId, int reading, int billingMonth, int billingYear)
        {
            try
            {
                var existingRoom = await _roomRepository.GetRoomByIdAsync(roomId);
                if (existingRoom == null)
                {
                    return new Response<string> { Message = "Không tìm thấy phòng để ghi số liệu", Succeeded = false }; 
                }

                if (existingRoom.IsAvailable)
                {
                    return new Response<string> { Message = "Phòng trọ không có người thuê phòng không thể ghi số liệu" , Succeeded = true};
                }

                var existingService = await _serviceRepository.GetByIdAsync(serviceId);
                if (existingService == null)
                {
                    return new Response<string> { Succeeded = false, Message = "Không có dịch vụ trong phòng trọ" };
                }
                // Validate input
                if (reading < 0)
                {
                    return new Response<string> { Message = "Số liệu phải lớn hơn 0", Succeeded = false };
                }

                if (billingMonth < 1 || billingMonth > 12)
                {
                    return new Response<string> { Message = "Tháng lập hóa đơn phải nằm trong khoảng từ 1 đến 12.", Succeeded = false };
                }

                if (billingYear < 2000 || billingYear > DateTime.Now.Year)
                {
                    return new Response<string> { Message = $"Năm lập hóa đơn không hợp lệ. Phải lớn hơn 2000 và nhỏ hơn {DateTime.Now.Year}", Succeeded = false };
                }
                var existingReading = await _meterReadingRepository.GetCurrentMeterReadingAsync(roomId, serviceId, billingMonth, billingYear);

                if (existingReading != null)
                {
                    return new Response<string> { Message = "Đã có số liệu đọc cho phòng dịch và dịch vụ trong tháng này", Succeeded = false };
                }
                var previousMeterReading = await _meterReadingRepository.GetPreviousMeterReadingAsync(roomId, serviceId, billingMonth, billingYear);
                if(previousMeterReading == null)
                {
                    var newPreviousMeterReading = new MeterReading
                    {
                        RoomId = roomId,
                        ServiceId = serviceId,
                        Reading = 0,
                        BillingMonth = billingMonth == 1 ? 12 : billingMonth - 1,
                        BillingYear = billingMonth == 1 ? billingYear - 1 : billingYear,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };
                    await _meterReadingRepository.AddAsync(newPreviousMeterReading);

                }
                //nếu không có số liệu tháng trước thì sẽ mặc định là 0
                if ((previousMeterReading?.Reading ?? 0)  > reading)
                {
                    return new Response<string> { Message = $"Số liệu tháng {billingMonth} phải lớn hơn hoặc bằng số liệu tháng {billingMonth - 1}", Succeeded = false };
                }
                var meterReading = new MeterReading
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId,
                    ServiceId = serviceId,
                    Reading = reading,
                    BillingMonth = billingMonth,
                    BillingYear = billingYear,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                };

                await _meterReadingRepository.AddAsync(meterReading);


                return new Response<string> { Message = $"Ghi thành công {existingService.ServiceName} của phòng {existingRoom.RoomName} ở tháng {billingMonth}/{billingYear}", Succeeded = true };
            }
            catch(Exception ex)
            {
                return new Response<string> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
