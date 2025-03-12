using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MaterialGatePassTacker.Models;
using MaterialGatePassTacker;
using MaterialGatePassTracker.BAL;
using MaterialGatePassTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.ServiceBus.Primitives;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MaterialGatePassTracker.Services;
using MaterialGatePassTracker.Interfaces;


namespace MaterialGatePassTackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Invalid input.");

                var (isSuccess, result) = await _authService.LoginAsync(model);
                if (!isSuccess) return Unauthorized(result);

                return Ok(new { token = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("GatePassEntry")]
        public async Task<IActionResult> GatePassEntry([FromBody] T_Gate_Pass request, [FromQuery] List<string> filePaths)
        {
            try
            {
                var response = await _authService.GatePassEntryAsync(request, filePaths);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetGatePassEntries")]
        public async Task<IActionResult> GetGatePassEntries([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _authService.GetGatePassEntriesAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetGatePassById/{gpid}")]
        public async Task<IActionResult> GetGatePassById(int gpid)
        {
            try
            {
                var gatePass = await _authService.GetGatePassByIdAsync(gpid);
                if (gatePass == null)
                    return NotFound(new { Status = "Error", Message = "Gate Pass not found" });

                return Ok(gatePass);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }

}
