using HostelFinder.Application.DTOs.InVoice.Requests;
using HostelFinder.Application.DTOs.InVoice.Responses;
using HostelFinder.Application.Interfaces.IServices;
using HostelFinder.Application.Wrappers;
using HostelFinder.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace XUnitTestHostelFinder.Controllers
{
    public class InvoiceControllerTests
    {
        private readonly Mock<IInvoiceService> _invoiceServiceMock;
        private readonly InvoiceController _controller;

        public InvoiceControllerTests()
        {
            _invoiceServiceMock = new Mock<IInvoiceService>();
            _controller = new InvoiceController(_invoiceServiceMock.Object);
        }

        [Fact]
        public async Task GetInvoices_ReturnsOkResult_WhenInvoicesExist()
        {
            // Arrange
            var mockResponse = new Response<List<InvoiceResponseDto>>
            {
                Data = new List<InvoiceResponseDto>
                {
                    new InvoiceResponseDto { Id = Guid.NewGuid(), TotalAmount = 100 },
                    new InvoiceResponseDto { Id = Guid.NewGuid(), TotalAmount = 200 }
                },
                Succeeded = true
            };

            _invoiceServiceMock
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetInvoices();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<InvoiceResponseDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetInvoice_ReturnsOkResult_WhenInvoiceExists()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var mockResponse = new Response<InvoiceResponseDto>
            {
                Data = new InvoiceResponseDto { Id = invoiceId, TotalAmount = 100 },
                Succeeded = true
            };

            _invoiceServiceMock
                .Setup(service => service.GetByIdAsync(invoiceId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetInvoice(invoiceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<InvoiceResponseDto>(okResult.Value);
            Assert.Equal(100, returnValue.TotalAmount);
        }

        [Fact]
        public async Task GetInvoice_ReturnsNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var mockResponse = new Response<InvoiceResponseDto>
            {
                Data = null,
                Succeeded = false,
                Message = "Invoice not found."
            };

            _invoiceServiceMock
                .Setup(service => service.GetByIdAsync(invoiceId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.GetInvoice(invoiceId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Invoice not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateInvoice_ReturnsOkResult_WhenCreationSucceeds()
        {
            // Arrange
            var invoiceDto = new AddInVoiceRequestDto
            {
                TotalAmount = 100,
                DueDate = DateTime.Now.AddDays(10),
                Status = true
            };

            var mockResponse = new Response<InvoiceResponseDto>
            {
                Data = new InvoiceResponseDto { Id = Guid.NewGuid(), TotalAmount = 100 },
                Succeeded = true
            };

            _invoiceServiceMock
                .Setup(service => service.CreateAsync(invoiceDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateInvoice(invoiceDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<InvoiceResponseDto>(okResult.Value);
            Assert.Equal(100, returnValue.TotalAmount);
        }

        [Fact]
        public async Task CreateInvoice_ReturnsBadRequest_WhenCreationFails()
        {
            // Arrange
            var invoiceDto = new AddInVoiceRequestDto
            {
                TotalAmount = 100,
                DueDate = DateTime.Now.AddDays(10),
                Status = true
            };

            var mockResponse = new Response<InvoiceResponseDto>
            {
                Data = null,
                Succeeded = false,
                Message = "Invoice creation failed."
            };

            _invoiceServiceMock
                .Setup(service => service.CreateAsync(invoiceDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateInvoice(invoiceDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invoice creation failed.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateInvoice_ReturnsOkResult_WhenUpdateSucceeds()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoiceDto = new UpdateInvoiceRequestDto
            {
                TotalAmount = 150,
                DueDate = DateTime.Now.AddDays(15),
                Status = false
            };

            var mockResponse = new Response<InvoiceResponseDto>
            {
                Data = new InvoiceResponseDto { Id = invoiceId, TotalAmount = 150 },
                Succeeded = true
            };

            _invoiceServiceMock
                .Setup(service => service.UpdateAsync(invoiceId, invoiceDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateInvoice(invoiceId, invoiceDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<InvoiceResponseDto>(okResult.Value);
            Assert.Equal(150, returnValue.TotalAmount);
        }

        [Fact]
        public async Task UpdateInvoice_ReturnsNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var invoiceDto = new UpdateInvoiceRequestDto
            {
                TotalAmount = 150,
                DueDate = DateTime.Now.AddDays(15),
                Status = false
            };

            var mockResponse = new Response<InvoiceResponseDto>
            {
                Data = null,
                Succeeded = false,
                Message = "Invoice not found."
            };

            _invoiceServiceMock
                .Setup(service => service.UpdateAsync(invoiceId, invoiceDto))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.UpdateInvoice(invoiceId, invoiceDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Invoice not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteInvoice_ReturnsOkResult_WhenDeletionSucceeds()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = true,
                Succeeded = true
            };

            _invoiceServiceMock
                .Setup(service => service.DeleteAsync(invoiceId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteInvoice(invoiceId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Response<bool>>(okResult.Value);
            Assert.True(returnValue.Data);
        }

        [Fact]
        public async Task DeleteInvoice_ReturnsNotFound_WhenInvoiceDoesNotExist()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var mockResponse = new Response<bool>
            {
                Data = false,
                Succeeded = false,
                Message = "Invoice not found."
            };

            _invoiceServiceMock
                .Setup(service => service.DeleteAsync(invoiceId))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.DeleteInvoice(invoiceId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Invoice not found.", notFoundResult.Value);
        }

        // 1. Test for Bad Request scenarios with invalid inputs
        [Fact]
        public async Task CreateInvoice_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var invalidDto = new AddInVoiceRequestDto(); // Missing required fields like ServiceCostId, TotalAmount
            _controller.ModelState.AddModelError("ServiceCostId", "Required");

            // Act
            var result = await _controller.CreateInvoice(invalidDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        // 2. Test for Internal Server Error
        [Fact]
        public async Task CreateInvoice_ReturnsInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            var invoiceDto = new AddInVoiceRequestDto
            {
                TotalAmount = 100,
                DueDate = DateTime.Now.AddDays(10),
                Status = true
            };

            _invoiceServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<AddInVoiceRequestDto>()))
                .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _controller.CreateInvoice(invoiceDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error: Internal server error", objectResult.Value);
        }

        // 4. Parameterized Tests with InlineData for various input scenarios
        [Theory]
        [InlineData(100, true)]  // Valid input
        [InlineData(0, false)]   // Invalid: TotalAmount should not be 0
        [InlineData(-50, false)] // Invalid: TotalAmount should not be negative
        public async Task CreateInvoice_WithDifferentInputs_ReturnsExpectedResult(decimal totalAmount, bool expectedSuccess)
        {
            // Arrange
            var invoiceDto = new AddInVoiceRequestDto
            {
                TotalAmount = totalAmount,
                DueDate = DateTime.Now.AddDays(10),
                Status = true
            };

            // Expected response based on input validity
            Response<InvoiceResponseDto> mockResponse;
            if (expectedSuccess)
            {
                mockResponse = new Response<InvoiceResponseDto>
                {
                    Data = new InvoiceResponseDto { TotalAmount = totalAmount },
                    Succeeded = true
                };
            }
            else
            {
                mockResponse = new Response<InvoiceResponseDto>
                {
                    Data = null,
                    Succeeded = false,
                    Message = "Invalid invoice data."
                };
            }

            _invoiceServiceMock
                .Setup(service => service.CreateAsync(It.IsAny<AddInVoiceRequestDto>()))
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.CreateInvoice(invoiceDto);

            // Assert
            if (expectedSuccess)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<InvoiceResponseDto>(okResult.Value);
                Assert.Equal(totalAmount, returnValue.TotalAmount);
            }
            else
            {
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid invoice data.", badRequestResult.Value);
            }
        }
    }
}
