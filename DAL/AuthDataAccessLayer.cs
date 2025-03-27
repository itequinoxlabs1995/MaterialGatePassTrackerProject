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
using Microsoft.Extensions.FileProviders;
using MaterialGatePassTracker.Middleware;
namespace MaterialGatePassTracker.DAL
{
    public class AuthDataAccessLayer
    {
        private readonly IConfiguration _configuration;
        private readonly MaterialDbContext _materialDbContext;
        private readonly string _logFile;

        public AuthDataAccessLayer(MaterialDbContext materialDbContext, IConfiguration config)
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
                if (model.User_Name != null && model.Password != null)
                {
                    string Encypass = EncodePasswordToBase64(model.Password);


                    user = _materialDbContext.Users.FirstOrDefault(u => u.User_Name == model.User_Name);

                    if (user == null)
                    {
                        LogWriterClass.LogWrite("Login DAL:User not found.", _logFile);
                        return "User not found.";
                    }

                    if (user.Password != Encypass)
                    {
                        LogWriterClass.LogWrite("Login DAL:Incorrected password.", _logFile);
                        return "Incorrected password";
                    }

                    bool Loginuser = _materialDbContext.Users
                            .FirstOrDefault(u => u.User_Name == model.User_Name
                                         && u.Password == Encypass) != null;
                    //Db check
                    if (Loginuser == true)
                    {
                        LogWriterClass.LogWrite("Login DAL:Token Logic start.", _logFile);
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
                        LogWriterClass.LogWrite("Login DAL:Please Enter Valid Username and Password", _logFile);
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
   /* public class LogWriterClass
    {
        private static string m_exePath = string.Empty;
        public static void LogWrite(string logMessage, string path)
        {
            // m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())).Root + $@"";

            m_exePath = filepath + path;
            string fullpath = m_exePath + "\\" + "log_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            if (!File.Exists(fullpath))
            {
                File.Create(fullpath);
            }

            try
            {

                FileStream fs = new FileStream(fullpath, FileMode.Append);

                using (StreamWriter w = new StreamWriter(fs))
                    AppendLog(logMessage, w);



            }
            catch (Exception ex)
            {
                //AppendLog(ex.ToString());
            }

        }

        private static void AppendLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                // txtWriter.Write("Log Start : ");
                // txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                //txtWriter.WriteLine("  :");
                txtWriter.WriteLine("{0},{1}", DateTime.Now.ToLongDateString() + " ", DateTime.Now.ToLongTimeString() + " :" + logMessage);
                // txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                txtWriter.Write(ex.Message);
            }
        }
    }
    */

}
