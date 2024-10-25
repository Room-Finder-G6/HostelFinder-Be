using AutoMapper;
using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Application.DTOs.MembershipService.Responses;
using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Entities;

namespace HostelFinder.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMapper _mapper;

        public MembershipService(IMembershipRepository membershipRepository, IMapper mapper)
        {
            _membershipRepository = membershipRepository;
            _mapper = mapper;
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

                        _membershipRepository.Update(existingService);
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

        public async Task<Response<string>> DeleteMembershipAsync(Guid id)
        {
            var membership = await _membershipRepository.GetByIdAsync(id);
            if (membership == null)
            {
                return new Response<string>("Membership not found.");
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
            return new Response<string> { Data = "Gói thành viên đã xóa thành công!" };
        }

    }
}
