using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MaterialGatePassTracker.BAL
{
    public interface IAuthBusinessLogicClass
    {
        
    }
    public class AuthBusinessLogicClass: IAuthBusinessLogicClass
    {
        private readonly IConfiguration _configuration;
        private readonly MaterialDbContext _materialDbContext;
        private readonly string _logFile;

        public AuthBusinessLogicClass(MaterialDbContext materialDbContext, IConfiguration config)
        {
            _materialDbContext = materialDbContext;
            _logFile = config["Logging:LogFilePath"].ToString();
            _configuration = config;

        }

        public dynamic Login(Login model)
        {
            var user = (dynamic)null;

            try
            {
                 if (model.User_Name != null && model.Password !=null)
                {
                    string Encypass = EncodePasswordToBase64(model.Password);

                    
                        user = _materialDbContext.Users.FirstOrDefault(u => u.User_Name == model.User_Name);

                    if (user == null)
                    {
                        return "User not found.";
                    }

                    if (user.Password != Encypass)
                    {
                        return "Incorrected password";
                    }

                    bool Loginuser = _materialDbContext.Users
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

                        return new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                    }
                    else
                    {
                        return new Models.Response
                        { Status = "Error", Message = "Please Enter Valid Username and Password" }
                        ;
                        

                    }

                }


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
            }
            return user;
        }
        public string EncodePasswordToBase64(string password)
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }


    }

    
}
