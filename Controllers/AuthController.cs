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
        private readonly IConfiguration _configuration;
        private readonly IBusinessClass _contextClass;
        private readonly MaterialDbContext _context;
        private readonly EmailService _emailService;        

        public class Response
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }

        public AuthController(MaterialDbContext materialDbContext, IConfiguration configuration, EmailService emailService)
        {
            _context = materialDbContext;
            _configuration = configuration;
            _emailService = emailService;
        }



        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] D_User model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string Encypass = EncodePasswordToBase64(model.Password);

                    var user = _context.Users.FirstOrDefault(u => u.User_Name == model.User_Name);

                    if (user == null)
                    {
                        return Unauthorized("User not found.");
                    }

                    if(user.Password != Encypass)
                    {
                        return Unauthorized("Incorrected password");
                    }

                    bool Loginuser = _context.Users
                            .FirstOrDefault(u => u.User_Name == model.User_Name
                                         && u.Password == Encypass) != null;
                    //Db check
                    if (Loginuser == true)
                    {
                        //JWT Token Logic
                        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.User_Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                        var token = new JwtSecurityToken(
                            issuer: _configuration["JWT:ValidIssuer"],
                            audience: _configuration["JWT:ValidAudience"],
                            expires: DateTime.Now.AddMinutes(30),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });

                    }
                    else
                    {
                        return Ok(new Response
                        { Status = "Error", Message = "Please Enter Valid Username and Password" }
                        );

                    }

                }


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
            }

            return Unauthorized();

        }

        [HttpPost]
        [Route("GatePassEntry")]
        public async Task<IActionResult> GatePassEntry([FromBody] T_Gate_Pass request, [FromQuery] List<string> filePaths)
        {
            try
            {
                request.CreatedOn = DateTime.Now;
                request.FilePaths = string.Join(",", filePaths);
                _context.GatesPasses.Add(request);
                await _context.SaveChangesAsync();                

                return Ok(new Response
                { Status = "Success", Message = "Gate Pass Data Inserted successfully" }
                                 );

            }
            catch (Exception ex)
            {

                await _emailService.SendEmailAsync(
                    "Insert Gate Pass Data",
                   $@"
                    <p>Dear Manager,</p>
                    <p>Failed to Insert Gate Pass Data</p>
                    <p><strong>Error:</strong> {ex.Message}</p>
                    <p>Best regards,<br/>IT Support</p>",
                   request,
                  filePaths
                );

                return StatusCode(500, "Failed to Insert Gate Pass Data. Notification sent.");

            }
        }

        [HttpGet]
        [Route("GetGatePassEntries")]
        public async Task<IActionResult> GetGatePassEntries([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var gatePasses = await _context.GatesPasses
                .Where(g => g.CreatedOn >= startDate && g.CreatedOn <= endDate)
                .Select(g => new
                {
                    UniqueId = g.GPID,
                    VehicleNumber = g.Vehicle_No
                })
                .ToListAsync();

            return Ok(gatePasses);
        }

        [HttpGet]
        [Route("GetGatePassById/{gpid}")]
        public async Task<IActionResult> GetGatePassById(int gpid)
        {
            var gatePass = await _context.GatesPasses.FindAsync(gpid);

            if (gatePass == null)
            {
                return NotFound(new Response
                { Status = "Error", Message = "Gate Pass not found" });
            }

            return Ok(gatePass);
        }


        //this function Convert to Encord your Password
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        //this function Convert to Decord your Password
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new string(decoded_char);
            return result;
        }


        /* [HttpPost]
         [Route("AddUser")]
         public async Task<IActionResult> AddUser([FromBody]D_User model)
         {         
              _context.Users.Add(model);
              await _context.SaveChangesAsync();       

             return Ok();
         }


         [HttpGet]
         [Route("GetUsers")]
         public async Task<IActionResult> GetUsers()
         {
             return Ok(await _context.Users.ToListAsync());
         }

         */



    }
}
