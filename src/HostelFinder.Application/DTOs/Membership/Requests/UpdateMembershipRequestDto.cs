﻿using HostelFinder.Application.DTOs.MembershipService.Requests;

namespace HostelFinder.Application.DTOs.Membership.Requests
{
    public class UpdateMembershipRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public List<UpdateMembershipServiceReqDto>? MembershipServices { get; set; }
    }
}
