using Azure.Core;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.BAL;
using MaterialGatePassTracker.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace MaterialGatePassTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _logFile;

        private readonly ILogger<HomeController> _logger;
        private readonly MaterialGatePassTacker.MaterialDbContext _materialDbContext;
        private readonly HomeBusinessLogicClass _businessClass;

        public HomeController(ILogger<HomeController> logger,IConfiguration config, MaterialGatePassTacker.MaterialDbContext dbContext, HomeBusinessLogicClass businessClass)
        {
            try
            {
                _logFile = config["Logging:LogFilePath"].ToString();
                _logger = logger;
                _materialDbContext = dbContext;
                _businessClass = businessClass;


            }
            catch (Exception exc)
            {
                LogWriterClass.LogWrite("HomeController Constructor:"+exc.Message.ToString(), _logFile);

            }

        }

        public IActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AllUsers()
        {
            IEnumerable<D_User> newList = null;
            try
            {
                newList = _businessClass.AllUsers();
               
                LogWriterClass.LogWrite("AllUser Action:Get User Data Successfully", _logFile);
            }
            catch(Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AllUser Action:" + exc.Message.ToString(), _logFile);
            }

            return View(newList);

            // return View(_materialDbContext.Users.ToList());
        }

        [HttpGet]
        public IActionResult AllRoles()
        {
            IEnumerable<M_Role> newList = null;
            try
            {
                newList = _businessClass.AllRoles();
                LogWriterClass.LogWrite("AllRoles Action:Get User Roles Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AllRoles Action:" + exc.Message.ToString(), _logFile);
            }
            return View(newList);
            // return View(_materialDbContext.Users.ToList());
        }

        [HttpGet]
        public IActionResult AllProjects()
        {
            IEnumerable<M_Project> newList = null;
            try { 
             newList= _businessClass.AllProjects();
             LogWriterClass.LogWrite("AllProjects Action:Get Projects Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AllProjects Action:" + exc.Message.ToString(), _logFile);
            }
            return View(newList);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(D_UserViewModel userModel)
        {
            IEnumerable<D_User> newList = null;
            try
            {
                if (ModelState.IsValid)
                {
                    LogWriterClass.LogWrite("AddUser Action:Model is Valid", _logFile);

                    newList = _businessClass.AddUser(userModel);

                    LogWriterClass.LogWrite("AddUser Action:Insert UserData Successfully", _logFile);                   
                    //for list return

                }
                else
                {

                    return PartialView("AddUser", userModel);
                    //  return PartialView("AddUser", userModel);

                }

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddUser Action:" + exc.Message.ToString(), _logFile);
            }
            return RedirectToAction("AllUsers", newList);
            //return PartialView("AddUser", userModel);

            // return RedirectToAction("AllUsers");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRole(M_RoleViewModel userModel)
        {
            IEnumerable<M_Role> newList = null;
            try
            {
                if (ModelState.IsValid)
                {
                    LogWriterClass.LogWrite("AddRole Action:Model is Valid", _logFile);

                    newList = _businessClass.AddRole(userModel);
                   
                    LogWriterClass.LogWrite("AddRole Action:Insert Role Successfully ", _logFile);
                   //for list return

                }
                else
                {

                    return PartialView("AddRole", userModel);

                }

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddRole Action:" + exc.Message.ToString(), _logFile);

            }
           // IEnumerable<M_Role> newList = _materialDbContext.Roles.ToList();
            return RedirectToAction("AllRoles", newList);

            //return PartialView("AddUser", userModel);
            // return RedirectToAction("AllUsers");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProject(M_ProjectViewModel userModel)
        {
            IEnumerable<M_Project> newList = null;
            try
            {
                if (ModelState.IsValid)
                {
                    LogWriterClass.LogWrite("AddProject Action:Model is Valid", _logFile);

                    newList = _businessClass.AddProject(userModel);

                    LogWriterClass.LogWrite("AddProject Action:Insert Project Successfully ", _logFile);

                    //for list return

                }
                else
                {

                    return PartialView("AddProject", userModel);

                }

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddProject Action:" + exc.Message.ToString(), _logFile);

            }
            return RedirectToAction("AllProjects", newList);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(D_UserViewModel model)
        {
            IEnumerable<D_User> newList = null;
            try { 

                newList= _businessClass.EditUser(model);
           
                LogWriterClass.LogWrite("EditUser Action:Edit User Successfully ", _logFile);

                
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditUser Action:" + exc.Message.ToString(), _logFile);

            }

            return RedirectToAction("AllUsers", newList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProject(M_ProjectViewModel model)
        {
            IEnumerable<M_Project> newList = null;
            try { 
            
                newList = _businessClass.EditProject(model);
                LogWriterClass.LogWrite("EditProject Action:Edit Project Successfully ", _logFile);

            
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditProject Action:" + exc.Message.ToString(), _logFile);

            }

           // IEnumerable<M_Project> newList = _materialDbContext.Projects.ToList();
            return RedirectToAction("AllProjects", newList);
        }

        [HttpPost]
        public JsonResult EditUserData(int ID)
        {
            D_UserViewModel model = new D_UserViewModel();

            try
            {
                model = _businessClass.EditUserData(ID);

               
            LogWriterClass.LogWrite("EditUserData Action:Edit UserData Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditUserData Action:" + exc.Message.ToString(), _logFile);

            }
            return Json(model);

        }

        [HttpPost]
        public JsonResult EditProjectData(int ID)
        {
            M_ProjectViewModel model = new M_ProjectViewModel(); 
            try { 
            
                model = _businessClass.EditProjectData(ID);
            LogWriterClass.LogWrite("EditProjectData Action:Edit ProjectData Successfully", _logFile);

        }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditProjectData Action:" + exc.Message.ToString(), _logFile);

            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult RoleList()
        {
            M_Role model = new M_Role();

            try { 
           
                model = _businessClass.RoleList();
            LogWriterClass.LogWrite("RoleList Action:Get RoleList Successfully", _logFile);

        }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("RoleList Action:" + exc.Message.ToString(), _logFile);

            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GateList()
        {
            M_Gate model = new M_Gate();
            try { 
           
                model = _businessClass.GateList();
            LogWriterClass.LogWrite("GateList Action:Get GateList Successfully", _logFile);

        }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("GateList Action:" + exc.Message.ToString(), _logFile);

            }

            return Json(model);
        }

        public IActionResult DeleteUser(int id)
        {
            IEnumerable<D_User> newList = null;


                try { 
                newList = _businessClass.DeleteUser(id);
               
                    LogWriterClass.LogWrite("DeleteUser Action:User details deleted successfully", _logFile);
                   
                  }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("DeleteUser Action:" + exc.Message.ToString(), _logFile);

            }
           return RedirectToAction("AllUsers", newList);

           
        }

        public IActionResult DeleteProject(int id)
        {
            IEnumerable<M_Project> newList = null;
            try
            {
            newList = _businessClass.DeleteProject(id);
            LogWriterClass.LogWrite("DeleteProject Action:Project details deleted successfully", _logFile);
             
            }

            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("DeleteUser Action:" + exc.Message.ToString(), _logFile);
                return RedirectToAction("Error1");
            }
            return RedirectToAction("AllProjects", newList);

        }

        public IActionResult Sidebar1()
        {
            return View();
        }
        public IActionResult AddUserRole()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            IEnumerable<SelectListItem> Soulist = null;
            T_Gate_Pass t_Gate_Pass = new();
            try
            {

                t_Gate_Pass = _businessClass.Dashboard();
                LogWriterClass.LogWrite("Dashboard Action:Get Soulist Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("Dashboard Action:" + exc.Message.ToString(), _logFile);

            }

            return View(t_Gate_Pass);

        }

        [HttpPost]
        public JsonResult AjaxMethod_CascadingList(string type, int IDCast)
        {
            T_Gate_Pass model = new();

            try
            {
               model = _businessClass.AjaxMethod_CascadingList(type, IDCast);
            }
            catch (Exception exc) {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxMethod_CascadingList Action:" + exc.Message.ToString(), _logFile);

            }
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxMethod(DateTime StartDate, DateTime EndDate)
        {
            var result = (dynamic)null;

            try
            {
                result = _businessClass.AjaxMethod(StartDate, EndDate);
                LogWriterClass.LogWrite("AjaxMethod Action:AjaxMethod Success", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxMethod Action:" + exc.Message.ToString(), _logFile);

            }


            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxSouData(DateTime StartDate, DateTime EndDate, int Selected_SOU)
        {

            var result = (dynamic)null;

            try
            {
                result = _businessClass.AjaxSouData(StartDate, EndDate, Selected_SOU);
                LogWriterClass.LogWrite("AjaxSouData Action:Get AjaxSouData Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxSouData Action:" + exc.Message.ToString(), _logFile);

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxProjectData(DateTime StartDate, DateTime EndDate, int Selected_PID)
        {

            var result = (dynamic)null;

            try
            {
                result = _businessClass.AjaxProjectData(StartDate, EndDate, Selected_PID);  

                LogWriterClass.LogWrite("AjaxProjectData Action:Get AjaxProjectData Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxProjectData Action:" + exc.Message.ToString(), _logFile);

            }

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxGateData(DateTime StartDate, DateTime EndDate, int Selected_GID)
        {

            var result = (dynamic)null;

            try
            {
                result = _businessClass.AjaxGateData(StartDate, EndDate, Selected_GID);

                LogWriterClass.LogWrite("AjaxGateData Action:Get AjaxGateData Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxGateData Action:" + exc.Message.ToString(), _logFile);

            }


            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxSouData1(DateTime StartDate, DateTime EndDate)
        {
            var result = (dynamic)null;

            try
            {
                result = _businessClass.AjaxSouData1(StartDate, EndDate);
                LogWriterClass.LogWrite("AjaxSouData1 Action:Get AjaxSouData1 Successfully", _logFile);


            }
            catch (Exception exc) {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxSouData1 Action:" + exc.Message.ToString(), _logFile);

            }
            

            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxProjectData1(DateTime StartDate, DateTime EndDate)
        {

            var result = (dynamic)null;

            try
            {
                 result = _businessClass.AjaxProjectData1(StartDate, EndDate);

                LogWriterClass.LogWrite("AjaxProjectData1 Action:Get AjaxProjectData1 Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxProjectData1 Action:" + exc.Message.ToString(), _logFile);

            }


            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxGateData1(DateTime StartDate, DateTime EndDate)
        {
            var result = (dynamic)null;

            try
            {

                result = _businessClass.AjaxGateData1(StartDate, EndDate);

                LogWriterClass.LogWrite("AjaxGateData1 Action:Get AjaxGateData1 Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxGateData1 Action:" + exc.Message.ToString(), _logFile);

            }


            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Login model)
        {
            LogWriterClass.LogWrite("Start Application", _logFile);

            try
            {
                if (ModelState.IsValid)
                {
                   var model1 = _businessClass.Index(model);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    LogWriterClass.LogWrite("Login Action:Model is Invalid", _logFile);
                    return View(model);

                }

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("Login Action:" + exc.Message.ToString(), _logFile);


            }
            return View(model);

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
            string result = new String(decoded_char);
            return result;
        }

        [HttpGet]
        /* public IActionResult CheckPassIsAvailable(D_User model)
         {

             bool myUser = _materialDbContext.Users
         .FirstOrDefault(u => u.User_Name == model.User_Name
                      && u.Password == model.Password) != null;

             return View(model);

         }
         */

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


    public class LogWriterClass
    {
        private static string m_exePath = string.Empty;
        public static void LogWrite(string logMessage, string path)
        {
           // m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            m_exePath = path;
            string fullpath = m_exePath + "\\" + "log_"+ DateTime.Now.ToString("dd-MMM-yyyy") +".txt";

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
                txtWriter.WriteLine("{0},{1}", DateTime.Now.ToLongDateString()+" ", DateTime.Now.ToLongTimeString()+" :" +logMessage);
               // txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                txtWriter.Write(ex.Message);
            }
        }
    }

   
    public sealed record GraphData
    {
        public DateTime Date { get; set; }

        public int Completed { get; set; }

        public int Pending { get; set; }
    }
}
