using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;
using MaterialGatePassTracker.DAL;
using MaterialGatePassTracker.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MaterialGatePassTracker.BAL
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepo _authRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepo authRepository, EmailService emailService, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<(bool, string)> LoginAsync(Login model)
        {
            string encryptedPassword = EncodePasswordToBase64(model.Password);
            var user = await _authRepository.GetUserAsync(model.User_Name);

            if (user == null || user.Password != encryptedPassword)
                return (false, "Invalid username or password.");

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.User_Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            var token = GenerateJwtToken(authClaims);
            return (true, token);
        }

        public async Task<Response> GatePassEntryAsync(T_Gate_Pass request, List<string> filePaths)
        {
            try
            {
                request.CreatedOn = DateTime.Now;
                request.FilePaths = string.Join(",", filePaths);
                await _authRepository.InsertGatePassAsync(request);

                return new Response { Status = "Success", Message = "Gate Pass Data Inserted successfully" };
            }
            catch (Exception ex)
            {
                return new Response { Status = "Error", Message = $"Failed: {ex.Message}" };
            }
        }

        public async Task<IEnumerable<object>> GetGatePassEntriesAsync(DateTime startDate, DateTime endDate)
        {
            return await _authRepository.GetGatePassEntriesAsync(startDate, endDate);
        }

        public async Task<T_Gate_Pass?> GetGatePassByIdAsync(int gpid)
        {
            return await _authRepository.GetGatePassByIdAsync(gpid);
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                expires: DateTime.Now.AddMinutes(30),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string EncodePasswordToBase64(string password) => Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

    }

}
