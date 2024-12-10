using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using HostelFinder.Application.DTOs.MeterReading.Request;
using HostelFinder.Application.DTOs.MeterReading.Response;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceCostService _serviceCostService;
       
        public MeterReadingService(
            IMeterReadingRepository meterReadingRepository, 
            IRoomRepository roomRepository, 
            IServiceRepository serviceRepository,
            IServiceCostService serviceCostService)
        {
            _meterReadingRepository = meterReadingRepository;
            _roomRepository = roomRepository;
            _serviceRepository = serviceRepository;
            _serviceCostService = serviceCostService;
        }
        public async Task<Response<string>> AddMeterReadingAsync(Guid roomId, Guid serviceId, int? previousReading, int currentReading  , int billingMonth, int billingYear)
        {
            try
            {
                var existingRoom = await _roomRepository.GetRoomByIdAsync(roomId);
                if (existingRoom == null)
                {
                    return new Response<string> { Message = "Không tìm thấy phòng để ghi số liệu", Succeeded = false }; 
                }

                var existingService = await _serviceRepository.GetByIdAsync(serviceId);
                if (existingService == null)
                {
                    return new Response<string> { Succeeded = false, Message = "Không có dịch vụ trong phòng trọ" };
                }
           
                if (currentReading < 0 && previousReading < 0)
                {
                    return new Response<string> { Message = "Số liệu phải lớn hơn 0", Succeeded = false };
                }

                if (billingMonth < 1 || billingMonth > 12)
                {
                    return new Response<string> { Message = "Tháng lập hóa đơn phải nằm trong khoảng từ 1 đến 12.", Succeeded = false };
                }

                if (billingYear < 0)
                {
                    return new Response<string> { Message = $"Năm lập hóa đơn không hợp lệ. Phải lớn hơn 2000 và nhỏ hơn {DateTime.Now.Year}", Succeeded = false };
                }
                var existingReading = await _meterReadingRepository.GetCurrentMeterReadingAsync(roomId, serviceId, billingMonth, billingYear);

                if (existingReading != null)
                {
                    return new Response<string> { Message = "Đã có số liệu đọc cho phòng dịch và dịch vụ trong tháng này", Succeeded = false };
                }
                // lấy số liệu tháng trước
                var previousMeterReading = await _meterReadingRepository.GetPreviousMeterReadingAsync(roomId, serviceId, billingMonth, billingYear);
                //nếu không có số liệu tháng trước thì sẽ mặc định là 0
                if (currentReading < previousMeterReading.Reading)
                {
                    return new Response<string>(){Message = "Số liệu tháng này phải lớn hơn hoặc bằng số liệu tháng trước", Succeeded = false};
                }
                if (previousMeterReading == null)
                {
                    var newPreviousMeterReading = new MeterReading
                    {
                        RoomId = roomId,
                        ServiceId = serviceId,
                        Reading = previousReading ?? 0,
                        BillingMonth = billingMonth == 1 ? 12 : billingMonth - 1,
                        BillingYear = billingMonth == 1 ? billingYear - 1 : billingYear,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };
                    await _meterReadingRepository.AddAsync(newPreviousMeterReading);

                }
                if ((previousMeterReading?.Reading ?? 0)  > currentReading)
                {
                    return new Response<string> { Message = $"Số liệu tháng {billingMonth} phải lớn hơn hoặc bằng số liệu tháng {billingMonth - 1}", Succeeded = false };
                }
                var meterReading = new MeterReading
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId,
                    ServiceId = serviceId,
                    Reading = currentReading,
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

        public async Task<Response<string>> AddMeterReadingListAsync(List<CreateMeterReadingDto> createMeterReadingDtos)
        {
            try
            {
                foreach (var meterReading in createMeterReadingDtos.ToList())
                {
                    var service = await _serviceRepository.GetByIdAsync(meterReading.serviceId);
                    if(service.ChargingMethod != Domain.Enums.ChargingMethod.PerUsageUnit )
                    {
                        return new Response<string> { Message = "Dịch vụ này không phải là dịch vụ đo số liệu", Succeeded = false };
                    }
                    await AddMeterReadingAsync(meterReading.roomId, meterReading.serviceId, meterReading.previousReading, meterReading.currentReading, meterReading.billingMonth, meterReading.billingYear);
                }
                return new Response<string> { Message = "Ghi số liệu thành công", Succeeded = true };
            }
            catch (Exception ex)
            {
                return new Response<string> { Message = ex.Message, Succeeded = false };
            }
        }

        public async Task<Response<List<ServiceCostReadingResponse>>> GetServiceCostReadingAsync(Guid hostelId, Guid roomId, int? billingMonth, int? billingYear)
        {
            try
            {
                // lấy ra tất cả các dịch vụ của trọ
                var serviceCosts = await _serviceCostService.GetAllServiceCostByHostel(hostelId);
                var serviceCostReadingResponses = new List<ServiceCostReadingResponse>();
                foreach (var serviceCost in serviceCosts.Data)
                {
                    if (serviceCost.ChargingMethod == ChargingMethod.PerUsageUnit)
                    {
                        MeterReading meterPreviousReading = null;
                        MeterReading meterCurrentReading = null;
                        if (billingMonth.HasValue && billingYear.HasValue)
                        {
                            meterPreviousReading = await _meterReadingRepository.GetPreviousMeterReadingAsync(roomId, serviceCost.ServiceId, billingMonth.Value, billingYear.Value);
                            meterCurrentReading = await _meterReadingRepository.GetCurrentMeterReadingAsync(roomId, serviceCost.ServiceId, billingMonth.Value, billingYear.Value);
                        }
                        else
                        {
                            meterPreviousReading = await _meterReadingRepository.GetPreviousMeterReadingAsync(roomId, serviceCost.ServiceId, DateTime.Now.Month, DateTime.Now.Year);
                            meterCurrentReading = await _meterReadingRepository.GetCurrentMeterReadingAsync(roomId, serviceCost.ServiceId, DateTime.Now.Month, DateTime.Now.Year);
                        }
                        var serviceCostReadingResponse = new ServiceCostReadingResponse
                        {
                            ServiceName = serviceCost.ServiceName,
                            UnitCost = serviceCost.UnitCost,
                            ChargingMethod = serviceCost.ChargingMethod,
                            Unit = serviceCost.Unit,
                            ServiceId = serviceCost.ServiceId,
                            PreviousReading = meterPreviousReading == null ? 0 : meterPreviousReading.Reading,  
                            CurrentReading = meterCurrentReading == null ? 0 : meterCurrentReading.Reading,
                        };
                        serviceCostReadingResponses.Add(serviceCostReadingResponse);
                    }
                }

                return new Response<List<ServiceCostReadingResponse>>
                    { Data = serviceCostReadingResponses, Succeeded = true };
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Response<List<ServiceCostReadingResponse>> { Message = ex.Message, Succeeded = false });
            }
        }
    }
}
