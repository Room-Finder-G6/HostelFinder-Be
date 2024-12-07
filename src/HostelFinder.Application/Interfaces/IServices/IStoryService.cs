
using HostelFinder.Application.DTOs.Story.Requests;
using HostelFinder.Application.DTOs.Story.Responses;
using HostelFinder.Application.Wrappers;

namespace HostelFinder.Application.Interfaces.IServices
{
    public interface IStoryService
    {
        Task<Response<string>> AddStoryAsync(AddStoryRequestDto request);
        Task<Response<StoryResponseDto>> GetStoryByIdAsync(Guid id);
        Task<Response<List<ListStoryResponseDto>>> GetAllStoryAsync();
        Task<Response<List<ListStoryResponseDto>>> GetAllStoryForAdminAsync();
        Task<Response<List<ListStoryResponseDto>>> GetStoryByUserIdAsync(Guid userId);
    }
}
