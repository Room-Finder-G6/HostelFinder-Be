using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.DTOs.InVoice.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IInvoiceService
    {
        Task<Response<List<InvoiceResponseDto>>> GetAllAsync();
        Task<Response<InvoiceResponseDto>> GetByIdAsync(Guid id);
        Task<Response<InvoiceResponseDto>> CreateAsync(AddInVoiceRequestDto invoiceDto);
        Task<Response<InvoiceResponseDto>> UpdateAsync(Guid id, UpdateInvoiceRequestDto invoiceDto);
        Task<Response<bool>> DeleteAsync(Guid id);
    }
}
