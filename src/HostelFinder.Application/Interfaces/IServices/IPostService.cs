using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.Filter;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Enums;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace HostelFinder.Application.Interfaces.IServices;

public interface IPostService
{
    Task<Response<PostResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId);
    Task<Response<AddPostRequestDto>> AddRoomAsync(AddPostRequestDto postDto);
    Task<Response<UpdatePostRequestDto>> UpdateRoomAsync(UpdatePostRequestDto postDto, Guid roomId);
    Task<Response<bool>> DeleteRoomAsync(Guid roomId);
}