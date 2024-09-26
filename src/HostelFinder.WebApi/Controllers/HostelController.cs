using HostelFinder.Application.Interfaces.IRepositories;
using HostelFinder.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HostelFinder.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HostelController : ControllerBase
    {
        private readonly IHostelRepository _hostelRepository;

        public HostelController(IHostelRepository hostelRepository)
        {
            _hostelRepository = hostelRepository;
        }

        // GET: api/Hostel/GetAllHostels
        [HttpGet]
        public async Task<IActionResult> GetAllHostels()
        {
            var hostels = await _hostelRepository.GetAllHostelsAsync();
            return Ok(hostels);
        }

        // GET: api/Hostel/GetHostelById/{id}
        [HttpGet]
        public async Task<IActionResult> GetHostelById(Guid id)
        {
            var hostel = await _hostelRepository.GetHostelWithDetailsAsync(id);
            if (hostel == null)
            {
                return NotFound();
            }
            return Ok(hostel);
        }

        // GET: api/Hostel/GetHostelsByLandlordId/{landlordId}
        [HttpGet]
        public async Task<IActionResult> GetHostelsByLandlordId(Guid landlordId)
        {
            var hostels = await _hostelRepository.GetHostelsByLandlordIdAsync(landlordId);
            return Ok(hostels);
        }

        // GET: api/Hostel/GetHostelsByName?name={name}
        [HttpGet]
        public async Task<IActionResult> GetHostelsByName(string name)
        {
            var hostels = await _hostelRepository.GetHostelsByNameAsync(name);
            return Ok(hostels);
        }

        // GET: api/Hostel/GetHostelsByAddress?address={address}
        [HttpGet]
        public async Task<IActionResult> GetHostelsByAddress(string address)
        {
            var hostels = await _hostelRepository.GetHostelsByAddressAsync(address);
            return Ok(hostels);
        }

        // POST: api/Hostel/AddHostel
        [HttpPost]
        public async Task<IActionResult> AddHostel(Hostel hostel)
        {
            var addedHostel = await _hostelRepository.AddHostelAsync(hostel);
            return Ok(addedHostel);
        }

        // PUT: api/Hostel/UpdateHostel/{id}
        [HttpPut]
        public async Task<IActionResult> UpdateHostel(Guid id, Hostel hostel)
        {
            if (id != hostel.Id)
            {
                return BadRequest("Hostel ID mismatch.");
            }

            var updatedHostel = await _hostelRepository.UpdateHostelAsync(hostel);
            return Ok(updatedHostel);
        }

        // DELETE: api/Hostel/DeleteHostel/{id}
        [HttpDelete]
        public async Task<IActionResult> DeleteHostel(Guid id)
        {
            var deletedHostel = await _hostelRepository.DeleteHostelAsync(id);
            if (deletedHostel == null)
            {
                return NotFound();
            }
            return Ok(deletedHostel);
        }
    }
}
