using Microsoft.AspNetCore.Http;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IS3Service
{
    public Task<string> UploadFileAsync(IFormFile file);

}