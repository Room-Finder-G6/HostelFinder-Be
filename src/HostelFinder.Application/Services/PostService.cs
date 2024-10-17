using AutoMapper;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.DTOs.Review.Response;
using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.RoomDetails.Response;
using HostelFinder.Application.DTOs.ServiceCost.Responses;
using HostelFinder.Application.DTOs.Users.Response;
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

    public PostService(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository, IHostelRepository hostelRepository)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _userRepository = userRepository;
        _hostelRepository = hostelRepository;
    }

    public async Task<Response<PostResponseDto>> GetAllRoomFeaturesByIdAsync(Guid roomId)
    {
        var room = await _postRepository.GetAllRoomFeaturesByRoomId(roomId);
        if (room == null)
        {
            return new Response<PostResponseDto>("Post not found");
        }

        var roomDto = _mapper.Map<PostResponseDto>(room);
        roomDto.RoomDetailsDto = _mapper.Map<RoomDetailsResponseDto>(room.RoomDetails);
        roomDto.ServiceCostsDto = _mapper.Map<List<ServiceCostResponseDto>>(room.ServiceCosts);

        var response = new Response<PostResponseDto>(roomDto);
        return response;
    }

    public async Task<Response<AddPostRequestDto>> AddRoomAsync(AddPostRequestDto postDto)
    {
        try
        {
            var roomDomain = _mapper.Map<Post>(postDto);

            roomDomain.RoomDetails = _mapper.Map<RoomDetails>(postDto.RoomDetails);
            roomDomain.ServiceCosts = _mapper.Map<List<ServiceCost>>(postDto.ServiceCosts);
            roomDomain.CreatedOn = DateTime.Now;
            roomDomain.CreatedBy = "System";

            roomDomain = await _postRepository.AddAsync(roomDomain);

            foreach (var amenityDto in postDto.AddRoomAmenity)
            {
                if (amenityDto.IsSelected)
                {
                    var roomAmenity = new RoomAmenities
                    {
                        PostId = roomDomain.Id,
                        AmenityId = amenityDto.Id
                    };

                    // Thêm tiện nghi vào phòng
                    await _postRepository.AddRoomAmenitiesAsync(roomAmenity);
                }
            }

            var roomResponseDto = _mapper.Map<AddPostRequestDto>(roomDomain);
            return new Response<AddPostRequestDto>(roomResponseDto);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while adding room", ex);
        }
    }

    public async Task<Response<UpdatePostRequestDto>> UpdateRoomAsync(UpdatePostRequestDto postDto, Guid roomId)
    {
        var existingRoom = await _postRepository.GetAllRoomFeaturesByRoomId(roomId);

        if (existingRoom == null)
        {
            return new Response<UpdatePostRequestDto>("Post not found");
        }

        _mapper.Map(postDto, existingRoom);

        if (existingRoom.RoomDetails == null)
        {
            existingRoom.RoomDetails = new RoomDetails { PostId = roomId };
        }

        _mapper.Map(postDto.UpdateRoomDetailsDto, existingRoom.RoomDetails);

        _mapper.Map(postDto.AddRoomAmenityDto, existingRoom.RoomAmenities);

        /*if (postDto.ServiceCosts != null)
        {
            existingRoom.ServiceCosts = _mapper.Map<List<ServiceCost>>(postDto.ServiceCosts);
        }*/

        existingRoom.LastModifiedOn = DateTime.Now;
        existingRoom.LastModifiedBy = "System";

        await _postRepository.UpdateAsync(existingRoom);
        var roomResponseDto = _mapper.Map<UpdatePostRequestDto>(existingRoom);
        return new Response<UpdatePostRequestDto>(roomResponseDto);
    }

    public async Task<Response<bool>> DeleteRoomAsync(Guid roomId)
    {
        var room = await _postRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            return new Response<bool> { Succeeded = false, Message = "Post not found" };
        }

        await _postRepository.DeleteAsync(room.Id);

        return new Response<bool> { Succeeded = true, Message = "Delete Post Successfully" };
    }

    public async Task<LandlordResponseDto> GetLandlordByPostIdAsync(Guid postId)
    {
        var hostel = await _userRepository.GetHostelByPostIdAsync(postId);

        if (hostel == null || hostel.Landlord == null)
        {
            return null; 
        }

        var landlordDto = new LandlordResponseDto
        {
            Id = hostel.Landlord.Id,
            FullName = hostel.Landlord.Username,
            Email = hostel.Landlord.Email,
            Phone = hostel.Landlord.Phone,
            AvatarUrl = hostel.Landlord.AvatarUrl
        };

        return landlordDto;
    }

    public async Task<HostelResponseDto> GetHostelByPostIdAsync(Guid postId)
    {
        var hostel = await _hostelRepository.GetHostelWithReviewsByPostIdAsync(postId);

        if (hostel == null)
        {
            throw new NullReferenceException("Hostel not found for the provided Post ID.");
        }

        if (hostel.Reviews == null)
        {
            hostel.Reviews = new List<Review>(); 
        }

        float averageRating = (float)(hostel.Reviews.Any() ? hostel.Reviews.Average(r => r.rating) : 0);

        var reviewsDto = hostel.Reviews.Select(review => new ReviewResponseDto
        {
            Id = review.Id,
            Comment = review.Comment,
            Rating = review.rating,
            ReviewDate = review.ReviewDate,
            UserId = review.UserId,
            HostelId = review.HostelId
        }).ToList();

        var hostelResponseDto = new HostelResponseDto
        {
            Id = hostel.Id,
            HostelName = hostel.HostelName,
            Description = hostel.Description,
            Address = hostel.Address.ToString(),
            NumberOfRooms = hostel.NumberOfRooms,
            Rating = averageRating,
            Image = hostel.Images.ToString()
            //Reviews = reviewsDto
        };

        return hostelResponseDto;
    }

}