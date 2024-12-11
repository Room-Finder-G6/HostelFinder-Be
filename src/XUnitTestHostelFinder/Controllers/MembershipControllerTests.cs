using HostelFinder.Application.DTOs.Membership.Requests;
using HostelFinder.Application.DTOs.Membership.Responses;
using HostelFinder.Application.DTOs.MembershipService.Requests;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HostelFinder.UnitTests.Controllers
{
    public class MembershipControllerTests
    {
        private readonly MembershipController _controller;
        private readonly Mock<IMembershipService> _membershipServiceMock;
        private readonly Mock<IWalletService> _walletServiceMock;

        public MembershipControllerTests()
        {
            _membershipServiceMock = new Mock<IMembershipService>();
            _controller = new MembershipController(_membershipServiceMock.Object, _walletServiceMock.Object);
        }

       

    }
}
