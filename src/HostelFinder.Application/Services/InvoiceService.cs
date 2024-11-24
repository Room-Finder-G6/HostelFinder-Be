using AutoMapper;
using HostelFinder.Application.DTOs.Invoice.Responses;
using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.DTOs.InVoice.Responses;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HostelFinder.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInVoiceRepository _invoiceRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRoomTenancyRepository _roomTenancyRepository;

        public InvoiceService(IInVoiceRepository invoiceRepository,
            IRoomRepository roomRepository,
            IMeterReadingRepository meterReadingRepository,
            IMapper mapper,
            ILogger<InvoiceService> logger,
            IServiceRepository serviceRepository,
            IRoomTenancyRepository roomTenancyRepository)
        {
            _invoiceRepository = invoiceRepository;
            _roomRepository = roomRepository;
            _meterReadingRepository = meterReadingRepository;
            _mapper = mapper;
            _logger = logger;
            _serviceRepository = serviceRepository;
            _roomTenancyRepository = roomTenancyRepository;
        }

        public async Task<Response<List<InvoiceResponseDto>>> GetAllAsync()
        {
            var invoices = await _invoiceRepository.ListAllAsync();
            var result = _mapper.Map<List<InvoiceResponseDto>>(invoices);
            return new Response<List<InvoiceResponseDto>>(result);
        }

        public async Task<Response<InvoiceResponseDto>> GetByIdAsync(Guid id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return new Response<InvoiceResponseDto>("Invoice not found.");

            var result = _mapper.Map<InvoiceResponseDto>(invoice);
            return new Response<InvoiceResponseDto>(result);
        }

        public async Task<Response<InvoiceResponseDto>> CreateAsync(AddInVoiceRequestDto invoiceDto)
        {
            var invoice = _mapper.Map<Invoice>(invoiceDto);
            invoice = await _invoiceRepository.AddAsync(invoice);

            var result = _mapper.Map<InvoiceResponseDto>(invoice);
            return new Response<InvoiceResponseDto>(result, "Invoice created successfully.");
        }

        public async Task<Response<InvoiceResponseDto>> UpdateAsync(Guid id, UpdateInvoiceRequestDto invoiceDto)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return new Response<InvoiceResponseDto>("Invoice not found.");

            _mapper.Map(invoiceDto, invoice);
            invoice = await _invoiceRepository.UpdateAsync(invoice);

            var result = _mapper.Map<InvoiceResponseDto>(invoice);
            return new Response<InvoiceResponseDto>(result, "Invoice updated successfully.");
        }

        public async Task<Response<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var invoice = await _invoiceRepository.GetByIdAsync(id);
                if (invoice == null)
                    return new Response<bool>(false, "Invoice not found.");

                await _invoiceRepository.DeleteAsync(id);
                return new Response<bool>(true, "Invoice deleted successfully.");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, $"Error occurred: {ex.Message}");
            }
        }

        public async Task<Response<InvoiceResponseDto>> GenerateMonthlyInvoicesAsync(Guid roomId, int billingMonth, int billingYear)
        {

            var room = await _roomRepository.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return new Response<InvoiceResponseDto> { Message = "Phòng không tồn tại!", Succeeded = false };
            }
            if (room.IsAvailable)
            {
                return new Response<InvoiceResponseDto> { Message = "Phòng đang trống không cần lập hóa đơn", Succeeded = false };
            }
            var existingInvoice = room.Invoices.FirstOrDefault(i => i.BillingMonth == billingMonth && i.BillingYear == billingYear);

            if (existingInvoice != null)
            {
                return new Response<InvoiceResponseDto> { Message = "Đã có hóa đơn cho tháng này", Succeeded = false };
            }
            using var transaction = await _invoiceRepository.BeginTransactionAsync();
            try
            {

                //Tạo hóa đơn mới

                var invoice = new Invoice
                {
                    RoomId = roomId,
                    BillingMonth = billingMonth,
                    BillingYear = billingYear,
                    TotalAmount = 0,
                    IsPaid = false,
                    CreatedOn = DateTime.Now,
                    InvoiceDetails = new List<InvoiceDetail>()
                };

                var serviceCosts = room.Hostel.ServiceCosts.ToList();

                foreach (var serviceCost in serviceCosts)
                {
                    var service = await _serviceRepository.GetServiceByIdAsync(serviceCost.ServiceId);
                    var invoiceDetail = new InvoiceDetail
                    {
                        InvoiceId = invoice.Id,
                        ServiceId = service.Id,
                        UnitCost = serviceCost.UnitCost,
                        ActualCost = 0,
                        //tạm thời tính là max of renters
                        NumberOfCustomer = await _roomTenancyRepository.CountCurrentTenantsAsync(room.Id),
                        BillingDate = DateTime.Now,
                        CreatedOn = DateTime.Now,
                        IsRentRoom = false,
                    };
                    // tạo hóa đơn chi tiết cho dịch vụ thu phí như điện, nước
                    if (service.IsUsageBased)
                    {
                        var previousReading = await _meterReadingRepository.GetPreviousMeterReadingAsync(roomId, service.Id, billingMonth, billingYear);
                        var currentReading = await _meterReadingRepository.GetCurrentMeterReadingAsync(room.Id, service.Id, billingMonth, billingYear);
                        if (previousReading == null || currentReading == null)
                        {
                            return new Response<InvoiceResponseDto> { Message = $"Thiếu số liệu đọc cho dịch vụ {service.ServiceName}.", Succeeded = false };
                        }

                        if (currentReading.Reading < previousReading.Reading)
                        {
                            return new Response<InvoiceResponseDto> { Message = $"Số đọc hiện tại không thể nhỏ hơn số đọc trước cho dịch vụ {service.ServiceName}.", Succeeded = false };
                        }

                        invoiceDetail.PreviousReading = previousReading.Reading;
                        invoiceDetail.CurrentReading = currentReading.Reading;


                        invoice.InvoiceDetails.Add(invoiceDetail);

                        invoice.TotalAmount += (invoiceDetail.UnitCost * (invoiceDetail.CurrentReading - invoiceDetail.PreviousReading)) + (invoiceDetail.ActualCost * invoiceDetail.NumberOfCustomer) ?? 0;

                    }
                }
                //tạo hóa đơn tiền phòng
                var rentInvoiceDetail = new InvoiceDetail
                {
                    InvoiceId = invoice.Id,
                    Service = null,
                    UnitCost = 0,
                    ActualCost = room.MonthlyRentCost,
                    NumberOfCustomer = room.MaxRenters,
                    BillingDate = DateTime.Now,
                    IsRentRoom = true,
                    CreatedOn = DateTime.Now,
                    CurrentReading = 0,
                    PreviousReading = 0,
                };
                invoice.InvoiceDetails.Add(rentInvoiceDetail);
                invoice.TotalAmount += rentInvoiceDetail.ActualCost;
                var invoiceCreated = await _invoiceRepository.AddAsync(invoice);

                await transaction.CommitAsync();

                //map to Dtos
                var invoiceCreatedDto = new InvoiceResponseDto
                {
                    RoomName = room.RoomName ?? string.Empty,
                    BillingMonth = invoiceCreated.BillingMonth,
                    BillingYear = invoiceCreated.BillingYear,
                    IsPaid = invoiceCreated.IsPaid,
                    TotalAmount = invoiceCreated.TotalAmount,
                    InvoiceDetails = invoiceCreated.InvoiceDetails.Select(details => new InvoiceDetailResponseDto
                    {
                        ActualCost = details.ActualCost,
                        BillingDate = details.BillingDate,
                        CurrentReading = details.CurrentReading,
                        PreviousReading = details.PreviousReading,
                        InvoiceId = details.InvoiceId,
                        NumberOfCustomer = details.NumberOfCustomer,
                        ServiceName = details.Service?.ServiceName ?? (details.IsRentRoom ? "Tiền thuê phòng" : "Không xác định"),
                        UnitCost = details.UnitCost,
                    }).ToList()

                };
                return new Response<InvoiceResponseDto> { Data = invoiceCreatedDto, Message = $"Tạo hóa đơn thành công cho phòng {room.RoomName} vào ngày {DateTime.Now}", Succeeded = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Xảy ra trong quá tình tạo hóa đơn phòng");
                return new Response<InvoiceResponseDto> { Message = "Xảy ra lỗi trong quá trình tạo hóa đơn" };
            }
        }

        public async Task<RoomInvoiceHistoryDetailsResponseDto?> GetInvoiceDetailInRoomLastestAsyc(Guid roomId)
        {
            try
            {
                // lấy ra hóa đơn cuối cùng
                var invoice = await _invoiceRepository.GetLastInvoiceByIdAsync(roomId);
                if (invoice == null)
                {
                    return null;
                }

                RoomInvoiceHistoryDetailsResponseDto invoiceResponseDto = new RoomInvoiceHistoryDetailsResponseDto
                {
                    Id = invoice.Id,
                    TotalAmount = invoice.TotalAmount,
                    BillingMonth = invoice.BillingMonth,
                    BillingYear = invoice.BillingYear,
                    IsPaid = invoice.IsPaid,
                    InvoiceDetails = invoice.InvoiceDetails.Select(details => new InvoiceDetailResponseDto
                    {
                        ActualCost = details.ActualCost,
                        BillingDate = details.BillingDate,
                        CurrentReading = details.CurrentReading,
                        PreviousReading = details.PreviousReading,
                        InvoiceId = details.InvoiceId,
                        NumberOfCustomer = details.NumberOfCustomer,
                        ServiceName = details.Service?.ServiceName ?? (details.IsRentRoom ? "Tiền thuê phòng" : "Không xác định"),
                        UnitCost = details.UnitCost,
                    }).ToList()
                };
                

                return invoiceResponseDto;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
