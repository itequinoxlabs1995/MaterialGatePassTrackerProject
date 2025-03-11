using MaterialGatePassTacker;
using MaterialGatePassTracker.BAL;
using MaterialGatePassTracker.DAL;
using MaterialGatePassTracker.Interfaces;
using MaterialGatePassTracker.Models;
using MaterialGatePassTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;

namespace MaterialGatePassTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : Controller
    {


        private readonly MaterialDbContext _context;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IStoreService _storeAPI;

        public StoreController(MaterialDbContext context, EmailService emailService, IConfiguration configuration,IStoreService storeAPI)
        {
                _context = context;
                _emailService = emailService;
                 _configuration = configuration;
                  _storeAPI = storeAPI;
        }

        [HttpGet("GetGatePasses/{userId}")]
        public async Task<IActionResult> GetGatePasses(int userId)
        {
            try 
            {
                var result = await _storeAPI.GetGatePassesByUserIdAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Controller] Error geeting gate pass data: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPost("UpdateGatePassStatus")]
        public async Task<IActionResult> UpdateGatePassStatus([FromBody] GatePassStatusRequest request)
        {
            try
            {
                await _storeAPI.UpdateGatePassStatusAsync(request);
                return Ok(new { message = "Gate pass status updated successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Controller] Error updating gate pass status: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
