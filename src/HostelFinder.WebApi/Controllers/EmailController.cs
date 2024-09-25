using HostelFinder.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/test-sendEmail")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailAsync(string emailTo, string subject, string body)
        {
            var result = await _emailService.SendEmailAsync(emailTo, subject, body);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}