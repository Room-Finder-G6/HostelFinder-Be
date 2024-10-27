using AutoMapper;
using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.DTOs.InVoice.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInVoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public InvoiceService(IInVoiceRepository invoiceRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
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
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
                return new Response<bool>("Invoice not found.");

            await _invoiceRepository.DeleteAsync(id);
            return new Response<bool>(true, "Invoice deleted successfully.");
        }
    }
}
