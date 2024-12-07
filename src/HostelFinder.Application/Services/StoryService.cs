using AutoMapper;
using HostelFinder.Application.DTOs.AddressStory;
using HostelFinder.Application.DTOs.Story.Requests;
using HostelFinder.Application.DTOs.Story.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _storyRepository;
        private readonly IAddressStoryRepository _addressStoryRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IS3Service _s3Service;
        private readonly IMapper _mapper;

        public StoryService(IStoryRepository storyRepository, IAddressStoryRepository addressStoryRepository,
                            IImageRepository imageRepository, IMapper mapper, IS3Service s3Service)
        {
            _storyRepository = storyRepository;
            _addressStoryRepository = addressStoryRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
            _s3Service = s3Service;
        }

        public async Task<Response<string>> AddStoryAsync(AddStoryRequestDto request)
        {
            try
            {
                var story = _mapper.Map<Story>(request);
                story.CreatedBy = request.UserId.ToString();
                story.CreatedOn = DateTime.Now;
                var address = _mapper.Map<AddressStory>(request.AddressStory);
                story.AddressStory = address;

                await _addressStoryRepository.AddAsync(address);
                await _storyRepository.AddAsync(story);

                var images = new List<Image>();
                if (request.Images != null && request.Images.Any())
                {
                    foreach (var imageFile in request.Images)
                    {
                        var imageUrl = await _s3Service.UploadFileAsync(imageFile);
                        var image = new Image
                        {
                            Url = imageUrl,
                            StoryId = story.Id
                        };
                        images.Add(image);
                    }
                }
                foreach (var image in images)
                {
                    await _imageRepository.AddAsync(image); 
                }
                story.Images = images;

                return new Response<string>
                {
                    Succeeded = true,
                    Message = "Thêm bài viết thành công",
                    Data = story.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = $"Lỗi: {ex.Message}"
                };
            }
        }

        public async Task<Response<StoryResponseDto>> GetStoryByIdAsync(Guid id)
        {
            try
            {
                var story = await _storyRepository.GetStoryByIdAsync(id);

                if (story == null)
                {
                    return new Response<StoryResponseDto>
                    {
                        Succeeded = false,
                        Message = "Bài viết không tồn tại."
                    };
                }

                var storyDto = _mapper.Map<StoryResponseDto>(story);

                return new Response<StoryResponseDto>
                {
                    Succeeded = true,
                    Message = "Lấy bài viết thành công.",
                    Data = storyDto
                };
            }
            catch (Exception ex)
            {
                return new Response<StoryResponseDto>
                {
                    Succeeded = false,
                    Message = $"Lỗi: {ex.Message}"
                };
            }
        }
        public async Task<Response<List<ListStoryResponseDto>>> GetAllStoryAsync()
        {
            try
            {
                var stories = await _storyRepository.GetAllStories();

                if (!stories.Any())
                {
                    return new Response<List<ListStoryResponseDto>>
                    {
                        Succeeded = false,
                        Message = "Không có bài viết nào thỏa mãn điều kiện.",
                        Data = new List<ListStoryResponseDto>()
                    };
                }

                var storyDtos = _mapper.Map<List<ListStoryResponseDto>>(stories);

                return new Response<List<ListStoryResponseDto>>
                {
                    Succeeded = true,
                    Message = "Lấy danh sách bài viết thành công.",
                    Data = storyDtos
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ListStoryResponseDto>>
                {
                    Succeeded = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<Response<List<ListStoryResponseDto>>> GetAllStoryForAdminAsync()
        {
            try
            {
                var stories = await _storyRepository.GetAllStoriesNoCondition();

                if (!stories.Any())
                {
                    return new Response<List<ListStoryResponseDto>>
                    {
                        Succeeded = false,
                        Message = "Không có bài viết nào thỏa mãn điều kiện.",
                        Data = new List<ListStoryResponseDto>()
                    };
                }

                var storyDtos = _mapper.Map<List<ListStoryResponseDto>>(stories);

                return new Response<List<ListStoryResponseDto>>
                {
                    Succeeded = true,
                    Message = "Lấy danh sách bài viết thành công.",
                    Data = storyDtos
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ListStoryResponseDto>>
                {
                    Succeeded = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<Response<List<ListStoryResponseDto>>> GetStoryByUserIdAsync(Guid userId)
        {
            try
            {
                var stories = await _storyRepository.GetStoriesByUserId(userId);

                if (!stories.Any())
                {
                    return new Response<List<ListStoryResponseDto>>
                    {
                        Succeeded = false,
                        Message = "Không có bài viết nào của người dùng này.",
                        Data = new List<ListStoryResponseDto>()
                    };
                }

                var storyDtos = _mapper.Map<List<ListStoryResponseDto>>(stories);

                return new Response<List<ListStoryResponseDto>>
                {
                    Succeeded = true,
                    Message = "Lấy danh sách bài viết thành công.",
                    Data = storyDtos
                };
            }
            catch (Exception ex)
            {
                return new Response<List<ListStoryResponseDto>>
                {
                    Succeeded = false,
                    Message = $"Lỗi: {ex.Message}",
                    Data = null
                };
            }
        }


    }
}
