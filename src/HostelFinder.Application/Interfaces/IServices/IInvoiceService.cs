using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.DTOs.InVoice.Responses;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IInvoiceService
    {
        Task<Response<List<InvoiceResponseDto>>> GetAllAsync();
        Task<Response<InvoiceResponseDto>> GetByIdAsync(Guid id);
        Task<Response<InvoiceResponseDto>> CreateAsync(AddInVoiceRequestDto invoiceDto);
        Task<Response<InvoiceResponseDto>> UpdateAsync(Guid id, UpdateInvoiceRequestDto invoiceDto);
        Task<Response<bool>> DeleteAsync(Guid id);
        Task<Response<InvoiceResponseDto>> GenerateMonthlyInvoicesAsync(Guid roomId, int billingMonth, int billingYear);

        Task<RoomInvoiceHistoryDetailsResponseDto?> GetInvoiceDetailInRoomLastestAsyc(Guid roomId);
    }
}
