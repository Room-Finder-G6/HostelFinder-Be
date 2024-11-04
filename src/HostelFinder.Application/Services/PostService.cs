using AutoMapper;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.DTOs.Image.Responses;
using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.DTOs.Post.Responses;
using HostelFinder.Application.DTOs.Users.Response;
using HostelFinder.Application.Helpers;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services;

public class PostService : IPostService
{
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHostelRepository _hostelRepository;
    private readonly IImageRepository _imageRepository;

    public PostService(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository,
        IHostelRepository hostelRepository, IImageRepository imageRepository)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _hostelRepository = hostelRepository;
        _imageRepository = imageRepository;
    }

    public async Task<Response<AddPostRequestDto>> AddPostAsync(AddPostRequestDto request, List<string> imageUrls)
    {
        //map to Domain Post
        var post = _mapper.Map<Post>(request);

        try
        {
            await _postRepository.AddAsync(post);
            foreach (var imageUrl in imageUrls)
            {
                await _imageRepository.AddAsync(new Image
                {
                    PostId = post.Id,
                    HostelId = post.HostelId,
                    Url = imageUrl,
                    CreatedOn = DateTime.Now,
                });
            }

            //map to Dtos Post
            var postResponseDto = _mapper.Map<AddPostRequestDto>(post);
            return new Response<AddPostRequestDto>
            {
                Data = postResponseDto,
                Succeeded = true,
                Message = "Add Post Successfully"
            };
        }
        catch (Exception ex)
        {
            return new Response<AddPostRequestDto>
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }

    public async Task<Response<bool>> DeletePostAsync(Guid postId, Guid userId)
    {
        var post = await _postRepository.GetPostByIdWithHostelAsync(postId);
        if (post == null)
        {
            return new Response<bool>
            {
                Succeeded = false,
                Errors = ["Bài đăng không tồn tại."]
            };
        }

        if (post.Hostel == null || post.Hostel.LandlordId != userId)
        {
            return new Response<bool>
            {
                Succeeded = false,
                Errors = ["Bạn không có quyền xóa bài đăng này."]
            };
        }

        await _postRepository.DeletePermanentAsync(postId);
        return new Response<bool> { Succeeded = true, Message = "Xóa bài đăng thành công." };
    }

    public async Task<LandlordResponseDto> GetLandlordByPostIdAsync(Guid postId)
    {
        var hostel = await _userRepository.GetHostelByPostIdAsync(postId);

        if (hostel == null || hostel.Landlord == null)
        {
            return null;
        }

        var landlordDto = _mapper.Map<LandlordResponseDto>(hostel.Landlord);

        return landlordDto;
    }

    public async Task<HostelResponseDto> GetHostelByPostIdAsync(Guid postId)
    {
        var hostel = await _hostelRepository.GetHostelWithReviewsByPostIdAsync(postId);

        if (hostel == null)
        {
            throw new NullReferenceException("Hostel not found for the provided Post ID.");
        }

        var hostelResponseDto = _mapper.Map<HostelResponseDto>(hostel);

        if (hostel.Images != null && hostel.Images.Any())
        {
            hostelResponseDto.Image = _mapper.Map<List<ImageResponseDto>>(hostel.Images);
        }

        return hostelResponseDto;
    }

    public async Task<PagedResponse<List<ListPostsResponseDto>>> GetAllPostAysnc(GetAllPostsQuery request)
    {
        try
        {
            var posts = await _postRepository.GetAllMatchingAsync(request.SearchPhrase, request.PageSize,
                request.PageNumber, request.SortBy, request.SortDirection);

            var postsDtos = _mapper.Map<List<ListPostsResponseDto>>(posts.Data);

            var pagedResponse = PaginationHelper.CreatePagedResponse(postsDtos, request.PageNumber, request.PageSize,
                posts.TotalRecords);
            return pagedResponse;
        }
        catch (Exception ex)
        {
            return new PagedResponse<List<ListPostsResponseDto>> { Succeeded = false, Errors = { ex.Message } };
        }
    }

    public async Task<Response<PostResponseDto>> GetPostByIdAsync(Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post == null)
        {
            return new Response<PostResponseDto>
            {
                Succeeded = false,
                Errors = new List<string> { "Bài đăng không tồn tại." }
            };
        }

        var postDto = _mapper.Map<PostResponseDto>(post);
        return new Response<PostResponseDto>
        {
            Data = postDto,
            Succeeded = true,
        };
    }

    public async Task<Response<List<ListPostsResponseDto>>> GetPostsByUserIdAsync(Guid userId)
    {
        var posts = await _postRepository.GetPostsByUserIdAsync(userId);

        if (posts == null || !posts.Any())
        {
            return new Response<List<ListPostsResponseDto>>
            {
                Succeeded = false,
                Errors = new List<string> { "Bạn chưa có bài đăng nào." }
            };
        }

        var postDtos = _mapper.Map<List<ListPostsResponseDto>>(posts);
        return new Response<List<ListPostsResponseDto>>
        {
            Data = postDtos,
            Succeeded = true
        };
    }
}