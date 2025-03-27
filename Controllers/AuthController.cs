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
using MaterialGatePassTracker.Middleware;



namespace MaterialGatePassTackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly AuthBusinessLogicClass _businessClass;
       private readonly string _logFile;
     private readonly IConfiguration _configuration;

     



 public AuthController(IAuthService authService, AuthBusinessLogicClass authBusinessLogic, IConfiguration configuration)
{
     _authService = authService;
    _businessClass = authBusinessLogic;
    _logFile = configuration["Logging:LogFilePath"].ToString();

}

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
           var user = (dynamic)null;
            try
            {
                if (ModelState.IsValid)
                {
                    user= _businessClass.Login(model);
                    LogWriterClass.LogWrite("Login Action:Login Successfully", _logFile);

                }
                else
                {
                    LogWriterClass.LogWrite("Login Action:Please Enter Valid Username and Password", _logFile);
                    return Ok(new
                    { Status = "Error", Message = "Please Enter Valid Username and Password" }
                    );

                }

              

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
            }

            return Ok(user);

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
  /*  public class LogWriterClass
    {
        private static string m_exePath = string.Empty;
        public static void LogWrite(string logMessage, string path)
        {
           var filepath =new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())).Root + $@"";

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
