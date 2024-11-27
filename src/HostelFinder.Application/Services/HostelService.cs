﻿using AutoMapper;
using HostelFinder.Application.DTOs.Hostel.Requests;
using HostelFinder.Application.DTOs.Hostel.Responses;
using HostelFinder.Application.Helpers;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using System.Linq;

namespace HostelFinder.Application.Services
{
    public class HostelService : IHostelService
    {
        private readonly IHostelRepository _hostelRepository;
        private readonly IMapper _mapper;
        private readonly IHostelServiceRepository _hostelServiceRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IS3Service _s3Service;

        public HostelService(IHostelRepository hostelRepository, IMapper mapper,
            IHostelServiceRepository hostelServiceRepository, IImageRepository imageRepository, IS3Service s3Service, IAddressRepository addressRepository)
        {
            _hostelRepository = hostelRepository;
            _mapper = mapper;
            _hostelServiceRepository = hostelServiceRepository;
            _imageRepository = imageRepository;
            _s3Service = s3Service;
            _addressRepository = addressRepository;
        }

        public async Task<Response<HostelResponseDto>> AddHostelAsync(AddHostelRequestDto request, string imageUrl)
        {
            // Kiểm tra trọ có bị trùng địa chỉ không
            var isDuplicate = await _hostelRepository.CheckDuplicateHostelAsync(
                request.HostelName,
                request.Address.Province,
                request.Address.District,
                request.Address.Commune,
                request.Address.DetailAddress
            );

            if (isDuplicate)
            {
                return new Response<HostelResponseDto>("Hostel đã tồn tại với cùng địa chỉ.");
            }

            var hostel = _mapper.Map<Hostel>(request);
            hostel.CreatedOn = DateTime.Now;
            hostel.CreatedBy = request.LandlordId.ToString();

            try
            {
                // Thêm Hostel vào cơ sở dữ liệu
                var hostelAdded = await _hostelRepository.AddAsync(hostel);

                // Thêm các dịch vụ vào Hostel
                foreach (var serviceId in request.ServiceId)
                {
                    HostelServices hostelServices = new HostelServices
                    {
                        ServiceId = serviceId ?? Guid.Empty,
                        HostelId = hostelAdded.Id,
                        CreatedBy = hostelAdded.LandlordId.ToString(),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                    };
                    await _hostelServiceRepository.AddAsync(hostelServices);
                }

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    await _imageRepository.AddAsync(new Image
                    {
                        HostelId = hostelAdded.Id,
                        Url = imageUrl
                    });
                }

                // Map domain object to DTO
                var hostelResponseDto = _mapper.Map<HostelResponseDto>(hostel);

                return new Response<HostelResponseDto>
                {
                    Data = hostelResponseDto,
                    Message = "Thêm trọ mới thành công."
                };
            }
            catch (Exception ex)
            {
                return new Response<HostelResponseDto>(message: ex.Message);
            }
        }


        public async Task<Response<HostelResponseDto>> UpdateHostelAsync(Guid hostelId, UpdateHostelRequestDto request, List<string> imageUrls)
        {
            var hostel = await _hostelRepository.GetByIdAsync(hostelId);
            if (hostel == null)
            {
                return new Response<HostelResponseDto>("Hostel not found.");
            }

            // Update the basic hostel properties
            _mapper.Map(request, hostel);
            hostel.LastModifiedOn = DateTime.Now;

            try
            {
                using (var transaction = await _hostelRepository.BeginTransactionAsync())
                {
                    // Update or Add Address using AutoMapper
                    var address = await _addressRepository.GetAddressByHostelIdAsync(hostelId);
                    if (address != null)
                    {
                        // Update existing address by mapping request.Address onto it
                        _mapper.Map(request.Address, address);
                        await _addressRepository.UpdateAsync(address);
                    }
                    else
                    {
                        // Map request.Address to a new Address object and add it
                        var newAddress = _mapper.Map<Address>(request.Address);
                        newAddress.HostelId = hostelId;
                        newAddress.CreatedOn = DateTime.Now;
                        await _addressRepository.AddAsync(newAddress);
                    }

                    // Update services
                    var existingServices = await _hostelServiceRepository.GetServicesByHostelIdAsync(hostelId);
                    var existingServiceIds = existingServices.Select(s => s.ServiceId).ToList();
                    var newServiceIds = request.ServiceId.Where(id => id.HasValue).Select(id => id.Value).Except(existingServiceIds).ToList();

                    // Add new services
                    foreach (var serviceId in newServiceIds)
                    {
                        var newService = new HostelServices
                        {
                            ServiceId = serviceId,
                            HostelId = hostelId,
                            CreatedBy = hostel.LandlordId.ToString(),
                            CreatedOn = DateTime.Now,
                            IsDeleted = false,
                        };
                        await _hostelServiceRepository.AddAsync(newService);
                    }

                    // Remove old services
                    var removedServiceIds = existingServiceIds.Except(request.ServiceId.Where(id => id.HasValue).Select(id => id.Value)).ToList();
                    foreach (var serviceId in removedServiceIds)
                    {
                        var serviceToRemove = existingServices.FirstOrDefault(s => s.ServiceId == serviceId);
                        if (serviceToRemove != null)
                        {
                            await _hostelServiceRepository.DeleteAsync(serviceToRemove.Id);
                        }
                    }

                    // Update images
                    var existingImages = await _imageRepository.GetImagesByHostelIdAsync(hostelId);
                    foreach (var image in existingImages)
                    {
                        await _s3Service.DeleteFileAsync(image.Url);
                        await _imageRepository.DeletePermanentAsync(image.Id);
                    }

                    foreach (var imageUrl in imageUrls)
                    {
                        var newImage = new Image
                        {
                            HostelId = hostelId,
                            Url = imageUrl,
                            CreatedOn = DateTime.Now,
                        };
                        await _imageRepository.AddAsync(newImage);
                    }

                    // Save hostel details
                    await _hostelRepository.UpdateAsync(hostel);

                    // Commit transaction
                    await transaction.CommitAsync();
                }

                // Map to response DTO
                var hostelResponseDto = _mapper.Map<HostelResponseDto>(hostel);
                return new Response<HostelResponseDto>
                {
                    Data = hostelResponseDto,
                    Message = "Hostel updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response<HostelResponseDto>(message: ex.Message);
            }
        }


        public async Task<Response<bool>> DeleteHostelAsync(Guid hostelId)
        {
            var hostel = await _hostelRepository.GetByIdAsync(hostelId);
            if (hostel == null)
            {
                return new Response<bool>(false, "Hostel not found");
            }

            try
            {
                await _hostelRepository.DeleteAsync(hostel.Id);
                return new Response<bool>(true, "Delete successful.");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, message: ex.Message);
            }
        }

        public async Task<Response<List<ListHostelResponseDto>>> GetHostelsByUserIdAsync(Guid landlordId)
        {
            var hostels = await _hostelRepository.GetHostelsByUserIdAsync(landlordId);

            var response = new Response<List<ListHostelResponseDto>>()
            {
                Data = _mapper.Map<List<ListHostelResponseDto>>(hostels)
            };

            return response;
        }

        public async Task<Response<HostelResponseDto>> GetHostelByIdAsync(Guid hostelId)
        {
            var hostel = await _hostelRepository.GetHostelByIdAsync(hostelId);
            if (hostel == null)
            {
                return new Response<HostelResponseDto>("Hostel not found.");
            }

            var hostelDto = _mapper.Map<HostelResponseDto>(hostel);

            return new Response<HostelResponseDto>(hostelDto);
        }

        public async Task<PagedResponse<List<ListHostelResponseDto>>> GetAllHostelAsync(GetAllHostelQuery request)
        {
            try
            {
                var hostels = await _hostelRepository.GetAllMatchingAsync(request.SearchPhrase, request.PageSize,
                    request.PageNumber, request.SortBy, request.SortDirection);

                var hostelDtos = _mapper.Map<List<ListHostelResponseDto>>(hostels.Data);

                var pagedResponse = PaginationHelper.CreatePagedResponse(hostelDtos, request.PageNumber,
                    request.PageSize, hostels.TotalRecords);
                return pagedResponse;
            }
            catch (Exception ex)
            {
                return new PagedResponse<List<ListHostelResponseDto>> { Succeeded = false, Errors = { ex.Message } };
            }
        }
    }
}