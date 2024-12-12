using HostelFinder.Application.DTOs.Room.Requests;
using HostelFinder.Application.DTOs.Room.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.Domain.Enums;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace XUnitTestHostelFinder.Controllers
{
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> _roomServiceMock;
        private readonly Mock<ITenantService> _tenantServiceMock;
        private readonly Mock<IRoomTenancyService> _roomTenacyServiceMock;
        private readonly RoomController _controller;

        public RoomControllerTests()
        {
            _roomServiceMock = new Mock<IRoomService>();
            _controller = new RoomController(_roomServiceMock.Object, _tenantServiceMock.Object, _roomTenacyServiceMock.Object);

        }

        
    }
}
