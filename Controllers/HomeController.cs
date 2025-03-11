using Azure.Core;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace MaterialGatePassTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MaterialGatePassTacker.MaterialDbContext _materialDbContext;

        public HomeController(ILogger<HomeController> logger, MaterialGatePassTacker.MaterialDbContext dbContext)
        {
            _logger = logger;
            _materialDbContext = dbContext;

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
            IEnumerable<D_User> newList = _materialDbContext.Users.Where(o => o.IsActive == true).ToList();
            /*foreach (var item in _materialDbContext.Users.ToList())
            {
                D_User listItem = new D_User();
                listItem.User_Name = item.User_Name;
                listItem.Password = item.Password;
                listItem.Email_ID = item.Email_ID;
                listItem.IsActive = item.IsActive;
                newList.Add(listItem);
            }
			*/

            return View(newList);



            // return View(_materialDbContext.Users.ToList());
        }

        [HttpGet]
        public IActionResult AllRoles()
        {
            IEnumerable<M_Role> newList = _materialDbContext.Roles.ToList();

            return View(newList);

            // return View(_materialDbContext.Users.ToList());
        }



        [HttpGet]
        public IActionResult AllProjects()
        {
            IEnumerable<M_Project> newList = _materialDbContext.Projects.Where(o => o.IsActive == true).ToList();

            return View(newList);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUser(D_UserViewModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    D_User USertbl = new D_User();
                    USertbl.User_Name = userModel.d_user.User_Name;
                    USertbl.Password = EncodePasswordToBase64(userModel.d_user.Password);
                    USertbl.Email_ID = userModel.d_user.Email_ID;
                    USertbl.Mobile_No = userModel.d_user.Mobile_No;
                    USertbl.RID = userModel.d_user.RID;
                    USertbl.IsActive = userModel.d_user.IsActive;
                    USertbl.CreatedOn = DateTime.Now;
                    _materialDbContext.Users.Add(USertbl);
                    _materialDbContext.SaveChanges();

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
            }
            IEnumerable<D_User> newList = _materialDbContext.Users.ToList();
            return RedirectToAction("AllUsers", newList);
            //return PartialView("AddUser", userModel);

            // return RedirectToAction("AllUsers");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRole(M_RoleViewModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    M_Role USertbl = new M_Role();
                    USertbl.Role_Name = userModel.m_Role.Role_Name;
                    USertbl.Role_Description = userModel.m_Role.Role_Description;
                    USertbl.CreatedOn = DateTime.Now;
                    _materialDbContext.Roles.Add(USertbl);
                    _materialDbContext.SaveChanges();

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
            }
            IEnumerable<M_Role> newList = _materialDbContext.Roles.ToList();
            return RedirectToAction("AllRoles", newList);

            //return PartialView("AddUser", userModel);
            // return RedirectToAction("AllUsers");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProject(M_ProjectViewModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    M_Project USertbl = new M_Project();
                    USertbl.Project_Name = userModel.m_Project.Project_Name;
                    USertbl.Project_Description = userModel.m_Project.Project_Description;
                    USertbl.GID = userModel.m_Project.GID;
                    USertbl.IsActive = userModel.m_Project.IsActive;
                    USertbl.CreatedOn = DateTime.Now;
                    _materialDbContext.Projects.Add(USertbl);
                    _materialDbContext.SaveChanges();

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
            }
            IEnumerable<M_Project> newList = _materialDbContext.Projects.ToList();
            return RedirectToAction("AllProjects", newList);


        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(D_UserViewModel model)
        {

            D_User USertbl = new D_User();
            USertbl = _materialDbContext.Users.SingleOrDefault(b => b.UID == model.d_user1.UID);
            if (USertbl != null)
            {
                USertbl.User_Name = model.d_user1.User_Name;
                USertbl.Password = EncodePasswordToBase64(model.d_user1.Password);
                USertbl.Email_ID = model.d_user1.Email_ID;
                USertbl.Mobile_No = model.d_user1.Mobile_No;
                USertbl.IsActive = model.d_user1.IsActive;
                USertbl.ModifiedOn = DateTime.Now;
                _materialDbContext.SaveChanges();
            }

            IEnumerable<D_User> newList = _materialDbContext.Users.ToList();
            return RedirectToAction("AllUsers", newList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProject(M_ProjectViewModel model)
        {

            M_Project USertbl = new M_Project();
            USertbl = _materialDbContext.Projects.SingleOrDefault(b => b.PID == model.m_Project1.PID);
            if (USertbl != null)
            {
                USertbl.Project_Name = model.m_Project1.Project_Name;
                USertbl.Project_Description = model.m_Project1.Project_Description;
                USertbl.GID = model.m_Project1.GID;
                USertbl.IsActive = model.m_Project1.IsActive;
                USertbl.ModifiedOn = DateTime.Now;
                _materialDbContext.SaveChanges();
            }

            IEnumerable<M_Project> newList = _materialDbContext.Projects.ToList();
            return RedirectToAction("AllProjects", newList);
        }

        [HttpPost]
        public JsonResult EditUserData(int ID)
        {
            D_UserViewModel model = new D_UserViewModel();

            model.d_user1 = _materialDbContext.Users.SingleOrDefault(mytable => mytable.UID == ID);
            model.Rolelist = (from Table in _materialDbContext.Roles
                              select new SelectListItem
                              {
                                  Selected = true,
                                  Text = Table.Role_Name,
                                  Value = Table.RID.ToString()
                              }).ToList();

            return Json(model);
        }


        [HttpPost]
        public JsonResult EditProjectData(int ID)
        {
            M_ProjectViewModel model = new M_ProjectViewModel();

            model.m_Project1 = _materialDbContext.Projects.SingleOrDefault(mytable => mytable.PID == ID);
            model.Gatelist = (from Table in _materialDbContext.Gates
                              select new SelectListItem
                              {
                                  // Selected = true,
                                  Text = Table.Gate_Location,
                                  Value = Table.GID.ToString()
                              }).ToList();

            return Json(model);
        }



        [HttpPost]
        public JsonResult RoleList()
        {
            M_Role model = new M_Role();

            model.Rolelist = (from Table in _materialDbContext.Roles
                              select new SelectListItem
                              {
                                  Text = Table.Role_Name,
                                  Value = Table.RID.ToString()
                              }).ToList();


            return Json(model);
        }


        [HttpPost]
        public JsonResult GateList()
        {
            M_Gate model = new M_Gate();

            model.Gatelist = (from Table in _materialDbContext.Gates
                              select new SelectListItem
                              {
                                  Text = Table.Gate_Location,
                                  Value = Table.GID.ToString()
                              }).ToList();


            return Json(model);
        }


        public IActionResult DeleteUser(int id)
        {
            try
            {

                D_User USertbl = new D_User();
                USertbl = _materialDbContext.Users.SingleOrDefault(b => b.UID == id);
                if (USertbl != null)
                {
                    USertbl.IsActive = false;
                    USertbl.ModifiedOn = DateTime.Now;
                    _materialDbContext.SaveChanges();
                    ViewBag.Message = "User details deleted successfully";

                }


                IEnumerable<D_User> newList = _materialDbContext.Users.ToList();
                return RedirectToAction("AllUsers", newList);
            }

            catch (Exception exc)
            {
                exc.Message.ToString();
                return RedirectToAction("Error1");


            }
        }

        public IActionResult DeleteProject(int id)
        {
            try
            {

                M_Project USertbl = new M_Project();
                USertbl = _materialDbContext.Projects.SingleOrDefault(b => b.PID == id);
                if (USertbl != null)
                {
                    USertbl.IsActive = false;
                    USertbl.ModifiedOn = DateTime.Now;
                    _materialDbContext.SaveChanges();
                    ViewBag.Message = "Project details deleted successfully";

                }


                IEnumerable<M_Project> newList = _materialDbContext.Projects.ToList();
                return RedirectToAction("AllProjects", newList);
            }

            catch (Exception exc)
            {
                exc.Message.ToString();
                return RedirectToAction("Error1");


            }
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
            IEnumerable<SelectListItem> Soulist = _materialDbContext.SOUs.Select(s => new SelectListItem
            {
                //Selected = false,
                Text = s.Sou_code,
                Value = s.SOUID.ToString()
            }).ToList();


            T_Gate_Pass t_Gate_Pass = new();
            t_Gate_Pass.Soulist = Soulist ?? [];


            return View(t_Gate_Pass);
        }


        [HttpPost]
        public JsonResult AjaxMethod_CascadingList(string type, int IDCast)
        {
            T_Gate_Pass model = new();

            switch (type)
            {
                case "SOUID":


                    model.Projectlist = (from Table in _materialDbContext.Projects
                                         where Table.SOUID == IDCast
                                         select new SelectListItem
                                         {
                                             // Selected = false,
                                             Text = Table.Project_Name,
                                             Value = Table.PID.ToString()
                                         }).ToList();


                    break;



                case "PID":

                    model.Gatelist = (from Table in _materialDbContext.Gates
                                      where Table.PID == IDCast
                                      select new SelectListItem
                                      {
                                          // Selected = false,
                                          Text = Table.Gate_Location,
                                          Value = Table.GID.ToString()
                                      }).ToList();


                    break;


            }
            return Json(model);
        }





        [HttpPost]
        public async Task<JsonResult> AjaxMethod(DateTime StartDate, DateTime EndDate)
        {


            var result = await (from Table in _materialDbContext.GatesPasses
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                select new GraphData
                                {
                                    // Table.GPID,
                                    Date = Table.CreatedOn.Date,
                                    Pending = 20,
                                    Completed = 30
                                    // Table.Vendor_Name
                                }).ToListAsync();


            //List<int> resultBrands = result.ToList(); // it should be select just Table but I will show the results to be clear.

            return Json(result);
        }




        [HttpPost]
        public async Task<JsonResult> AjaxSouData(DateTime StartDate, DateTime EndDate, int Selected_SOU)
        {


            var result = await (from Table in _materialDbContext.SOUs
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                && Table.SOUID == Selected_SOU
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    data = Table.SOUID,
                                    labels = Table.Sou_code
                                }).ToListAsync();

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> AjaxProjectData(DateTime StartDate, DateTime EndDate, int Selected_PID)
        {


            var result = await (from Table in _materialDbContext.Projects
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                && Table.PID == Selected_PID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Project_Name,
                                    data = Table.GID
                                }).ToListAsync();



            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxGateData(DateTime StartDate, DateTime EndDate, int Selected_GID)
        {


            var result = await (from Table in _materialDbContext.Gates
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                 && Table.GID == Selected_GID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Gate_Location,
                                    data = Table.PID
                                }).ToListAsync();



            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> AjaxSouData1(DateTime StartDate, DateTime EndDate)
        {


            var result = await (from Table in _materialDbContext.SOUs
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                // && Table.SOUID == Selected_SOU
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    data = Table.SOUID,
                                    labels = Table.Sou_code
                                }).ToListAsync();

            return Json(result);
        }


        [HttpPost]
        public async Task<JsonResult> AjaxProjectData1(DateTime StartDate, DateTime EndDate)
        {


            var result = await (from Table in _materialDbContext.Projects
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                // && Table.PID == Selected_PID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Project_Name,
                                    data = Table.GID
                                }).ToListAsync();



            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AjaxGateData1(DateTime StartDate, DateTime EndDate)
        {


            var result = await (from Table in _materialDbContext.Gates
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                // && Table.GID == Selected_GID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Gate_Location,
                                    data = Table.PID
                                }).ToListAsync();



            return Json(result);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Login model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    string Encypass = EncodePasswordToBase64(model.Password);
                    //  string decpass = DecodeFrom64(Encypass);

                    bool Loginuser = _materialDbContext.Users
                            .FirstOrDefault(u => u.User_Name == model.User_Name
                                         && u.Password == Encypass) != null;
                    //Db check
                    // if (_materialDbContext.Users.Where(u => u.User_Name == model.User_Name).Any())
                    if (Loginuser == true)
                    {
                        //JWT Token Logic
                        return RedirectToAction("Dashboard");

                    }
                    else
                    {
                        return View(model);
                    }

                }
                else
                {

                }

            }
            catch (Exception exc)
            {
                exc.Message.ToString();

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

    public sealed record GraphData
    {
        public DateTime Date { get; set; }

        public int Completed { get; set; }

        public int Pending { get; set; }
    }
}
