

using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IMeterReadingService
    {
        Task<Response<string>> AddMeterReadingAsync(Guid roomId, Guid serviceId, int reading, int billingMonth, int billingYear);
    }
}
