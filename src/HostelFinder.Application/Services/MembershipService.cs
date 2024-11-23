﻿using AutoMapper;
using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Application.DTOs.MembershipService.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;
using HostelFinder.Domain.Enums;

namespace HostelFinder.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserMembershipRepository _userMembershipRepository;
        private readonly IMapper _mapper;

        public MembershipService(IMembershipRepository membershipRepository, IMapper mapper, IUserMembershipRepository userMembershipRepository, IUserRepository userRepository)
        {
            _membershipRepository = membershipRepository;
            _mapper = mapper;
            _userMembershipRepository = userMembershipRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<List<MembershipResponseDto>>> GetAllMembershipWithMembershipService()
        {
            var memberships = await _membershipRepository.GetAllMembershipWithMembershipService();

            if (memberships == null || !memberships.Any())
            {
                return new Response<List<MembershipResponseDto>>
                {
                    Succeeded = false,
                    Errors = new List<string> { "No memberships found." }
                };
            }

            var response = new Response<List<MembershipResponseDto>>
            {
                Data = _mapper.Map<List<MembershipResponseDto>>(memberships),
                Succeeded = true
            };

            return response;
        }


        public async Task<Response<MembershipResponseDto>> AddMembershipAsync(AddMembershipRequestDto membershipDto)
        {
            var membership = _mapper.Map<Membership>(membershipDto);

            membership.CreatedOn = DateTime.Now;
            membership.CreatedBy = "System";

            try
            {
                var membershipServiceRequests = _mapper.Map<List<AddMembershipServiceReqDto>>(membershipDto.MembershipServices);

                await _membershipRepository.AddMembershipWithServicesAsync(membership, membershipServiceRequests);

                var membershipResponseDto = _mapper.Map<MembershipResponseDto>(membership);
                membershipResponseDto.MembershipServices = _mapper.Map<List<MembershipServiceResponseDto>>(membership.MembershipServices);

                return new Response<MembershipResponseDto>
                {
                    Data = membershipResponseDto,
                    Message = "Gói thành viện đã tạo thành công!"
                };
            }
            catch (Exception ex)
            {
                return new Response<MembershipResponseDto>(message: ex.Message);
            }
        }

        public async Task<Response<MembershipResponseDto>> EditMembershipAsync(Guid id, UpdateMembershipRequestDto membershipDto)
        {
            var membership = await _membershipRepository.GetMembershipWithServicesAsync(id);
            if (membership == null)
            {
                return new Response<MembershipResponseDto>("Membership not found.");
            }

            _mapper.Map(membershipDto, membership);
            membership.LastModifiedOn = DateTime.Now;
            membership.LastModifiedBy = "System";

            if (membershipDto.MembershipServices != null && membershipDto.MembershipServices.Any())
            {
                var existingServices = membership.MembershipServices.ToList();

                foreach (var newServiceDto in membershipDto.MembershipServices)
                {
                    if (string.IsNullOrWhiteSpace(newServiceDto.ServiceName))
                    {
                        continue; 
                    }

                    var existingService = existingServices
                        .FirstOrDefault(s => s.Id == newServiceDto.Id);

                    if (existingService != null)
                    {
                        existingService.ServiceName = newServiceDto.ServiceName;
                        existingService.LastModifiedOn = DateTime.Now;
                        existingService.LastModifiedBy = "System";
                    }
                    else
                    {
                        var newService = new MembershipServices
                        {
                            ServiceName = newServiceDto.ServiceName,
                            Membership = membership,
                            CreatedOn = DateTime.Now,
                            CreatedBy = "System"
                        };
                        membership.MembershipServices.Add(newService);
                        await _membershipRepository.Add(newService); 
                    }
                }

                foreach (var existingService in existingServices)
                {
                    if (!membershipDto.MembershipServices.Any(ms => ms.Id == existingService.Id))
                    {
                        await _membershipRepository.DeletePermanentAsync(existingService.Id);
                    }
                }
            }

            await _membershipRepository.UpdateAsync(membership);

            var membershipResponseDto = _mapper.Map<MembershipResponseDto>(membership);

            return new Response<MembershipResponseDto>
            {
                Data = membershipResponseDto,
                Message = "Gói thành viên được cập nhật thành công!"
            };
        }

        public async Task<Response<bool>> DeleteMembershipAsync(Guid id)
        {
            var membership = await _membershipRepository.GetByIdAsync(id);
            if (membership == null)
            {
                return new Response<bool>(false, "Membership not found.");
            }

            var membershipServices = await _membershipRepository.GetMembershipServicesByMembershipIdAsync(id);
            if (membershipServices != null)
            {
                foreach (var service in membershipServices)
                {
                    await _membershipRepository.DeletePermanentAsync(service.Id);
                }
            }

            await _membershipRepository.DeletePermanentAsync(membership.Id);
            return new Response<bool>(true, "Membership deleted successfully.");
        }

        public async Task<Response<string>> UpdatePostCountAsync(Guid userId)
        {
            var userMembership = await _userMembershipRepository.GetByUserIdAsync(userId);
            if (userMembership != null && userMembership.Membership != null)
            {
                var membershipService = userMembership.Membership.MembershipServices
                    .FirstOrDefault(ms => ms.MaxPostsAllowed > userMembership.PostsUsed);

                if (membershipService != null && userMembership.PostsUsed < membershipService.MaxPostsAllowed)
                {
                    userMembership.PostsUsed++;
                    await _userMembershipRepository.UpdateAsync(userMembership);

                    return new Response<string>
                    {
                        Succeeded = true,
                        Message = "Post count updated successfully."
                    };
                }
                else
                {
                    return new Response<string>
                    {
                        Succeeded = false,
                        Message = "You have reached the maximum number of posts allowed for your membership."
                    };
                }
            }

            return new Response<string>
            {
                Succeeded = false,
                Message = "User membership not found."
            };
        }

        public async Task<Response<string>> UpdatePushTopCountAsync(Guid userId)
        {
            var userMembership = await _userMembershipRepository.GetByUserIdAsync(userId);
            if (userMembership != null && userMembership.Membership != null)
            {
                // Find the membership service that supports push-to-top functionality
                var membershipService = userMembership.Membership.MembershipServices
                    .FirstOrDefault(ms => ms.MaxPushTopAllowed.HasValue && ms.MaxPushTopAllowed > userMembership.PushTopUsed);

                if (membershipService != null && userMembership.PushTopUsed < membershipService.MaxPushTopAllowed)
                {
                    // Increment the push count for the user
                    userMembership.PushTopUsed++;
                    await _userMembershipRepository.UpdateAsync(userMembership);

                    return new Response<string>
                    {
                        Succeeded = true,
                        Message = "Push-to-top count updated successfully."
                    };
                }
                else
                {
                    return new Response<string>
                    {
                        Succeeded = false,
                        Message = "You have reached the maximum number of push-to-top actions allowed for your membership."
                    };
                }
            }

            return new Response<string>
            {
                Succeeded = false,
                Message = "User membership not found."
            };
        }

        public async Task<Response<string>> AddUserMembershipAsync(AddUserMembershipRequestDto userMembershipDto)
        {
            var existingUserMembership = await _userMembershipRepository.GetByUserIdAsync(userMembershipDto.UserId);
            if (existingUserMembership != null && existingUserMembership.MembershipId == userMembershipDto.MembershipId)
            {
                return new Response<string>
                {
                    Succeeded = false,
                    Message = "User is already assigned to this membership."
                };
            }

            var userMembership = new UserMembership
            {
                UserId = userMembershipDto.UserId,
                MembershipId = userMembershipDto.MembershipId,
                PostsUsed = 0,
                PushTopUsed = 0,
                CreatedBy = "System",
                CreatedOn = DateTime.Now
            };

            await _userMembershipRepository.AddAsync(userMembership);

            var user = await _userRepository.GetByIdAsync(userMembershipDto.UserId);
            if (user != null)
            {
                user.Role = UserRole.Landlord;
                await _userRepository.UpdateAsync(user);
            }

            return new Response<string>
            {
                Succeeded = true,
                Data = "User membership added successfully."
            };
        }

    }
}
