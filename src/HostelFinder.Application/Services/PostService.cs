using AutoMapper;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.DTOs.Image.Responses;
using HostelFinder.Application.DTOs.Post.Requests;
using HostelFinder.Application.DTOs.Post.Responses;
using HostelFinder.Application.DTOs.Room.Requests;
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
    private readonly IRoomRepository _roomRepository;
    private readonly IMembershipService _membershipService;
    private readonly IS3Service _s3Service;

    public PostService(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository,
        IHostelRepository hostelRepository, IImageRepository imageRepository, IMembershipService membershipService, IS3Service s3Service, IRoomRepository roomRepository)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _hostelRepository = hostelRepository;
        _imageRepository = imageRepository;
        _membershipService = membershipService;
        _s3Service = s3Service;
        _roomRepository = roomRepository;
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

    public async Task<Response<AddPostRequestDto>> AddPostAsync(AddPostRequestDto request, List<string> imageUrls,
        Guid userId)
    {
        if (_membershipService == null)
        {
            return new Response<AddPostRequestDto>
            {
                Succeeded = false,
                Message = "Membership service not initialized."
            };
        }

        var postCountResponse = await _membershipService.UpdatePostCountAsync(userId);

        if (!postCountResponse.Succeeded)
        {
            return new Response<AddPostRequestDto>
            {
                Succeeded = false,
                Message = postCountResponse.Message
            };
        }

        var post = _mapper.Map<Post>(request);
        post.CreatedBy = userId.ToString();
        post.CreatedOn = DateTime.Now;
        
        try
        {
            using (var transaction = await _postRepository.BeginTransactionAsync())
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

                await transaction.CommitAsync();
            }

            var postResponseDto = _mapper.Map<AddPostRequestDto>(post);
            return new Response<AddPostRequestDto>
            {
                Data = postResponseDto,
                Succeeded = true,
                Message = "Post added successfully"
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

    public async Task<Response<List<ListPostsResponseDto>>> FilterPostsAsync(FilterPostsRequestDto filter)
    {
        var posts = await _postRepository.FilterPostsAsync(
            filter.Province,
            filter.District,
            filter.Commune,
            filter.minSize,
            filter.maxSize,
            filter.minPrice,
            filter.maxPrice,
            filter.RoomType
        );

        var postDtos = _mapper.Map<List<ListPostsResponseDto>>(posts);
        return new Response<List<ListPostsResponseDto>>
        {
            Data = postDtos,
            Succeeded = true
        };
    }

    public async Task<Response<PostResponseDto>> PushPostOnTopAsync(Guid postId, DateTime newDate, Guid userId)
    {
        if (_membershipService == null)
        {
            return new Response<PostResponseDto>
            {
                Succeeded = false,
                Message = "Membership service not initialized."
            };
        }

        var pushCountResponse = await _membershipService.UpdatePushTopCountAsync(userId);
        if (!pushCountResponse.Succeeded)
        {
            return new Response<PostResponseDto>
            {
                Succeeded = false,
                Message = pushCountResponse.Message
            };
        }

        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            return new Response<PostResponseDto>
            {
                Succeeded = false,
                Message = "Post not found."
            };
        }

        try
        {
            using (var transaction = await _postRepository.BeginTransactionAsync())
            {
                post.CreatedOn = newDate;
                post.LastModifiedOn = newDate;
                await _postRepository.UpdateAsync(post);

                await transaction.CommitAsync();
            }

            var postResponseDto = _mapper.Map<PostResponseDto>(post);
            return new Response<PostResponseDto>
            {
                Succeeded = true,
                Message = "Post pushed to the top successfully.",
                Data = postResponseDto
            };
        }
        catch (Exception ex)
        {
            return new Response<PostResponseDto>
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }

    public async Task<Response<List<ListPostsResponseDto>>> GetPostsOrderedByPriorityAsync()
    {
        var posts = await _postRepository.GetPostsOrderedByMembershipPriceAndCreatedOnAsync();
        var postDtos = _mapper.Map<List<ListPostsResponseDto>>(posts);

        return new Response<List<ListPostsResponseDto>>
        {
            Data = postDtos,
            Succeeded = true
        };
    }

    public async Task<PagedResponse<List<ListPostsResponseDto>>> GetPostsOrderedByPriorityAsync(int pageIndex, int pageSize)
    {
        var pagedPosts = await _postRepository.GetPostsOrderedByMembershipPriceAndCreatedOnAsync(pageIndex, pageSize);

        var postDtos = _mapper.Map<List<ListPostsResponseDto>>(pagedPosts.Data);

        return PaginationHelper.CreatePagedResponse(
            postDtos,
            pageIndex,
            pageSize,
            pagedPosts.TotalRecords
        );
    }

    public async Task<Response<UpdatePostRequestDto>> UpdatePostAsync(Guid postId, UpdatePostRequestDto request, List<string> imageUrls)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
        {
            return new Response<UpdatePostRequestDto>
            {
                Succeeded = false,
                Message = "Post not found."
            };
        }

        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null || room.HostelId != post.HostelId)
        {
            return new Response<UpdatePostRequestDto>("The specified room does not belong to the hostel associated with this post.");
        }

        _mapper.Map(request, post);
        post.LastModifiedOn = DateTime.Now;

        try
        {
            using (var transaction = await _postRepository.BeginTransactionAsync())
            {
                var existingImages = await _imageRepository.GetImagesByPostIdAsync(postId);

                foreach (var image in existingImages)
                {
                    await _s3Service.DeleteFileAsync(image.Url);
                    await _imageRepository.DeletePermanentAsync(image.Id);
                }

                foreach (var imageUrl in imageUrls)
                {
                    var newImage = new Image
                    {
                        PostId = post.Id,
                        HostelId = post.HostelId,
                        Url = imageUrl,
                        CreatedOn = DateTime.Now,
                    };
                    await _imageRepository.AddAsync(newImage);
                }

                await _postRepository.UpdateAsync(post);
                await transaction.CommitAsync();
            }

            var postResponseDto = _mapper.Map<UpdatePostRequestDto>(post);
            return new Response<UpdatePostRequestDto>
            {
                Data = postResponseDto,
                Succeeded = true,
                Message = "Post updated successfully"
            };
        }
        catch (Exception ex)
        {
            return new Response<UpdatePostRequestDto>
            {
                Succeeded = false,
                Message = ex.Message
            };
        }
    }

    public async Task<Response<List<ListPostsResponseDto>>> GetAllPostWithPriceAndStatusAndTime()
    {
        // Fetch posts from the repository
        var posts = await _postRepository.GetAllPostsOrderedAsync();

        // Map posts to DTOs or initialize with an empty list if null/empty
        var postDtos = posts != null && posts.Any()
            ? _mapper.Map<List<ListPostsResponseDto>>(posts)
            : new List<ListPostsResponseDto>();

        // Return the response with appropriate success status
        return new Response<List<ListPostsResponseDto>>
        {
            Data = postDtos,
            Succeeded = postDtos.Any(),
            Message = postDtos.Any() ? "Posts retrieved successfully" : "No posts found"
        };
    }

    public async Task<PagedResponse<List<ListPostsResponseDto>>> GetAllPostWithPriceAndStatusAndTime(int pageIndex, int pageSize)
    {
        var pagedPosts = await _postRepository.GetAllPostsOrderedAsync(pageIndex, pageSize);

        var postDtos = _mapper.Map<List<ListPostsResponseDto>>(pagedPosts.Data);

        return PaginationHelper.CreatePagedResponse(
            postDtos,
            pageIndex,
            pageSize,
            pagedPosts.TotalRecords
        );
    }

}

