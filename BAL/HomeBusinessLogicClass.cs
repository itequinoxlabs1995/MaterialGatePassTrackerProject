using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Net;

namespace MaterialGatePassTracker.BAL
{
    public interface IHomeBusinessLogicClass
    {
        // Task<IEnumerable<D_User>> GetUsersData();
       // IEnumerable<M_Role> AllRoles();
    }


    public class HomeBusinessLogicClass : IHomeBusinessLogicClass
    {
        private readonly MaterialDbContext _materialDbContext;
        private readonly string _logFile;

        public HomeBusinessLogicClass(MaterialDbContext materialDbContext, IConfiguration config)
        {
            _materialDbContext = materialDbContext;
            _logFile = config["Logging:LogFilePath"].ToString();

        }
        /* public async Task<IEnumerable<D_User>> GetUsersData()
         {
             return await _materialDbContext.Users.ToListAsync();
         }
         */

        public IEnumerable<D_User> AllUsers()
        {
            IEnumerable<D_User> newList = null;
            try
            {
                newList = _materialDbContext.Users.Where(o => o.IsActive == true).ToList();
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
                LogWriterClass.LogWrite("AllUser BAL:Get User Data Successfully", _logFile);
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AllUser BAL:" + exc.Message.ToString(), _logFile);
            }
            return newList;
        }

        public IEnumerable<M_Role> AllRoles()
        {
            IEnumerable<M_Role> newList = null;
            try
            {
                newList =  _materialDbContext.Roles.ToList();
                LogWriterClass.LogWrite("AllRoles BAL Method:Get User Roles Successfully", _logFile);
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AllRoles BAL Method:" + exc.Message.ToString(), _logFile);
            }
            return newList;

        }

        public IEnumerable<M_Project> AllProjects() 
        {
            IEnumerable<M_Project> newList = null;
            try
            {
                newList = _materialDbContext.Projects.Where(o => o.IsActive == true).ToList();
                LogWriterClass.LogWrite("AllProjects BAL:Get Projects Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AllProjects BAL:" + exc.Message.ToString(), _logFile);
            }

            return newList;

        }



        public IEnumerable<D_User> AddUser(D_UserViewModel userModel)
        {

            try
            {
                if (userModel != null)
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
                    LogWriterClass.LogWrite("AddUser BAL:Insert UserData Successfully", _logFile);
                }

           
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddUser BAL:" + exc.Message.ToString(), _logFile);
            }

            IEnumerable<D_User> newList = _materialDbContext.Users.ToList();
            return newList;
        }


        public IEnumerable<M_Role> AddRole(M_RoleViewModel userModel)
        {
            try
            {
                if (userModel != null)
                {

                    LogWriterClass.LogWrite("AddRole BAL:Model is Valid", _logFile);

                    M_Role USertbl = new M_Role();
                    USertbl.Role_Name = userModel.m_Role.Role_Name;
                    USertbl.Role_Description = userModel.m_Role.Role_Description;
                    USertbl.CreatedOn = DateTime.Now;
                    _materialDbContext.Roles.Add(USertbl);
                    _materialDbContext.SaveChanges();
                    LogWriterClass.LogWrite("AddRole BAL:Insert Role Successfully ", _logFile);
                    //for list return


                }

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddRole BAL:" + exc.Message.ToString(), _logFile);

            }
            IEnumerable<M_Role> newList = _materialDbContext.Roles.ToList();
            return  newList;
        }


        public IEnumerable<M_Project> AddProject(M_ProjectViewModel userModel)
        {
            try
            {
                if (userModel != null)
                {
                    LogWriterClass.LogWrite("AddProject BAL:Model is Valid", _logFile);
                    M_Project USertbl = new M_Project();
                    USertbl.Project_Name = userModel.m_Project.Project_Name;
                    USertbl.Project_Description = userModel.m_Project.Project_Description;
                    USertbl.GID = userModel.m_Project.GID;
                    USertbl.IsActive = userModel.m_Project.IsActive;
                    USertbl.CreatedOn = DateTime.Now;
                    _materialDbContext.Projects.Add(USertbl);
                    _materialDbContext.SaveChanges();
                    LogWriterClass.LogWrite("AddProject BAL:Insert Project Successfully ", _logFile);

                    //for list return

                }
                
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddProject BAL:" + exc.Message.ToString(), _logFile);

            }
            IEnumerable<M_Project> newList = _materialDbContext.Projects.ToList();
            return newList;


        }


        public IEnumerable<D_User> EditUser(D_UserViewModel model) 
        {
            D_User USertbl = new D_User();
            try
            {
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
                    LogWriterClass.LogWrite("EditUser BAL:Edit User Successfully ", _logFile);

                }
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditUser BAL:" + exc.Message.ToString(), _logFile);

            }

            IEnumerable<D_User> newList = _materialDbContext.Users.ToList();
            return newList;

        }


        public IEnumerable<M_Project> EditProject(M_ProjectViewModel model)
        {
            M_Project USertbl = new M_Project();
            try
            {
                USertbl = _materialDbContext.Projects.SingleOrDefault(b => b.PID == model.m_Project1.PID);
                if (USertbl != null)
                {
                    USertbl.Project_Name = model.m_Project1.Project_Name;
                    USertbl.Project_Description = model.m_Project1.Project_Description;
                    USertbl.GID = model.m_Project1.GID;
                    USertbl.IsActive = model.m_Project1.IsActive;
                    USertbl.ModifiedOn = DateTime.Now;
                    _materialDbContext.SaveChanges();
                    LogWriterClass.LogWrite("EditProject BAL:Edit Project Successfully ", _logFile);

                }
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditProject BAL:" + exc.Message.ToString(), _logFile);

            }

            IEnumerable<M_Project> newList = _materialDbContext.Projects.ToList();
            return newList;

        }


        public D_UserViewModel EditUserData(int ID)
        {

            D_UserViewModel model = new D_UserViewModel();

            try
            {
                model.d_user1 = _materialDbContext.Users.SingleOrDefault(mytable => mytable.UID == ID);
                model.Rolelist = (from Table in _materialDbContext.Roles
                                  select new SelectListItem
                                  {
                                      Selected = true,
                                      Text = Table.Role_Name,
                                      Value = Table.RID.ToString()
                                  }).ToList();

                LogWriterClass.LogWrite("EditUserData BAL:Edit UserData Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditUserData BAL:" + exc.Message.ToString(), _logFile);

            }
            return model;
        }


        public M_ProjectViewModel EditProjectData(int ID) 
        {
            M_ProjectViewModel model = new M_ProjectViewModel();
            try
            {
                model.m_Project1 = _materialDbContext.Projects.SingleOrDefault(mytable => mytable.PID == ID);
                model.Gatelist = (from Table in _materialDbContext.Gates
                                  select new SelectListItem
                                  {
                                      // Selected = true,
                                      Text = Table.Gate_Location,
                                      Value = Table.GID.ToString()
                                  }).ToList();
                LogWriterClass.LogWrite("EditProjectData BAL:Edit ProjectData Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditProjectData BAL:" + exc.Message.ToString(), _logFile);

            }
            return model;


        }


        public M_Role RoleList()
        {
            M_Role model = new M_Role();

            try
            {
                model.Rolelist = (from Table in _materialDbContext.Roles
                                  select new SelectListItem
                                  {
                                      Text = Table.Role_Name,
                                      Value = Table.RID.ToString()
                                  }).ToList();
                LogWriterClass.LogWrite("RoleList BAL:Get RoleList Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("RoleList BAL:" + exc.Message.ToString(), _logFile);

            }

            return model;
        }


        public M_Gate GateList()
        {
            M_Gate model = new M_Gate();
            try
            {
                model.Gatelist = (from Table in _materialDbContext.Gates
                                  select new SelectListItem
                                  {
                                      Text = Table.Gate_Location,
                                      Value = Table.GID.ToString()
                                  }).ToList();

                LogWriterClass.LogWrite("GateList BAL:Get GateList Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("GateList BAL:" + exc.Message.ToString(), _logFile);

            }

            return model;
        }


        public IEnumerable<D_User> DeleteUser(int id)
        {
            D_User USertbl = new D_User();
            try
            {
                USertbl = _materialDbContext.Users.SingleOrDefault(b => b.UID == id);
                if (USertbl != null)
                {
                    USertbl.IsActive = false;
                    USertbl.ModifiedOn = DateTime.Now;
                    _materialDbContext.SaveChanges();

                    LogWriterClass.LogWrite("DeleteUser BAL:User details deleted successfully", _logFile);

                }
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("DeleteUser BAL:" + exc.Message.ToString(), _logFile);

            }
            IEnumerable<D_User> newList = _materialDbContext.Users.ToList();
            return newList;
        }


        public IEnumerable<M_Project> DeleteProject(int id)       
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
                    LogWriterClass.LogWrite("DeleteProject BAL:Project details deleted successfully", _logFile);

                }             
            }

            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("DeleteUser BAL:" + exc.Message.ToString(), _logFile);


            }
            IEnumerable<M_Project> newList = _materialDbContext.Projects.ToList();
            return newList;
        }

        public T_Gate_Pass Dashboard()
        {
            IEnumerable<SelectListItem> Soulist = null;
            T_Gate_Pass t_Gate_Pass = new();
            try
            {
                Soulist = _materialDbContext.SOUs.Select(s => new SelectListItem
                {
                    //Selected = false,
                    Text = s.Sou_code,
                    Value = s.SOUID.ToString()
                }).ToList();



                t_Gate_Pass.Soulist = Soulist ?? [];
                LogWriterClass.LogWrite("Dashboard BAL:Get Soulist Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("Dashboard BAL:" + exc.Message.ToString(), _logFile);

            }

            return t_Gate_Pass;

        }

        public T_Gate_Pass AjaxMethod_CascadingList(string type, int IDCast)
        {
            T_Gate_Pass model = new();

            try
            {
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

                        LogWriterClass.LogWrite("AjaxMethod_CascadingList BAL:Get Projectlist Successfully", _logFile);

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

                        LogWriterClass.LogWrite("AjaxMethod_CascadingList BAL:Get Gatelist Successfully", _logFile);
                        break;


                }
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxMethod_CascadingList BAL:" + exc.Message.ToString(), _logFile);

            }
            return model;

        }

        public dynamic AjaxMethod(DateTime StartDate, DateTime EndDate)
        {
            var result = (dynamic)null;

            try
            {
                Task.Run(async () =>
                {
                result = await (from Table in _materialDbContext.GatesPasses
                               where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                               select new GraphData
                               {
                                   // Table.GPID,
                                   Date = Table.CreatedOn.Date,
                                   Pending = 20,
                                   Completed = 30
                                   // Table.Vendor_Name
                               }).ToListAsync().ConfigureAwait(false);
            }).GetAwaiter().GetResult();
            LogWriterClass.LogWrite("AjaxMethod BAL:AjaxMethod Success", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxMethod BAL:" + exc.Message.ToString(), _logFile);

            }


            return result;
        }

        public dynamic AjaxSouData(DateTime StartDate, DateTime EndDate, int Selected_SOU)
        {

            var result = (dynamic)null;


            try
            {
                //string getData = null;
                Task.Run(async () =>
                {
                    result = await (from Table in _materialDbContext.SOUs
                                                    where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                                    && Table.SOUID == Selected_SOU
                                                    select new 
                                                    {
                                                        Date = Table.CreatedOn.Date,
                                                        data = Table.SOUID,
                                                        labels = Table.Sou_code
                                                    }).ToListAsync().ConfigureAwait(false);
                }).GetAwaiter().GetResult();
                
                
              /*  result = (from Table in _materialDbContext.SOUs
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                && Table.SOUID == Selected_SOU
                                select new 
                                {
                                    Date = Table.CreatedOn.Date,
                                    data = Table.SOUID,
                                    labels = Table.Sou_code
                                }).ToList();
              */
                
                LogWriterClass.LogWrite("AjaxSouData BAL:Get AjaxSouData Successfully", _logFile);


            }
            catch (Exception exc)
            {
                
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxSouData BAL:" + exc.Message.ToString(), _logFile);
               
            }

            return result;
        }


        public dynamic AjaxProjectData(DateTime StartDate, DateTime EndDate, int Selected_PID)
        {

            var result = (dynamic)null;

            try
            {
                Task.Run(async () =>
                {
                    result = await (from Table in _materialDbContext.Projects
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                && Table.PID == Selected_PID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Project_Name,
                                    data = Table.GID
                                }).ToListAsync().ConfigureAwait(false);
            }).GetAwaiter().GetResult();

            LogWriterClass.LogWrite("AjaxProjectData BAL:Get AjaxProjectData Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxProjectData BAL:" + exc.Message.ToString(), _logFile);

            }

            return result;
        }

        public dynamic AjaxGateData(DateTime StartDate, DateTime EndDate, int Selected_GID)
        {

            var result = (dynamic)null;

            try
            {
                Task.Run(async () =>
                {
                    result = await (from Table in _materialDbContext.Gates
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                 && Table.GID == Selected_GID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Gate_Location,
                                    data = Table.PID
                                }).ToListAsync().ConfigureAwait(false);
                }).GetAwaiter().GetResult();

                LogWriterClass.LogWrite("AjaxGateData BAL:Get AjaxGateData Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxGateData BAL:" + exc.Message.ToString(), _logFile);

            }


            return result;
        }

        public dynamic AjaxSouData1(DateTime StartDate, DateTime EndDate)
        {
            var result = (dynamic)null;

            try
            {
                Task.Run(async () =>
                {
                    result = await (from Table in _materialDbContext.SOUs
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                // && Table.SOUID == Selected_SOU
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    data = Table.SOUID,
                                    labels = Table.Sou_code
                                }).ToListAsync().ConfigureAwait(false);
                }).GetAwaiter().GetResult();
                LogWriterClass.LogWrite("AjaxSouData1 BAL:Get AjaxSouData1 Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxSouData1 BAL:" + exc.Message.ToString(), _logFile);

            }


            return result;
        }


        public dynamic AjaxProjectData1(DateTime StartDate, DateTime EndDate)
        {

            var result = (dynamic)null;

            try
            {
                Task.Run(async () =>
                {
                    result = await (from Table in _materialDbContext.Projects
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                // && Table.PID == Selected_PID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Project_Name,
                                    data = Table.GID
                                }).ToListAsync().ConfigureAwait(false);
                }).GetAwaiter().GetResult();

                LogWriterClass.LogWrite("AjaxProjectData1 BAL:Get AjaxProjectData1 Successfully", _logFile);


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxProjectData1 BAL:" + exc.Message.ToString(), _logFile);

            }


            return result;
        }

        public dynamic AjaxGateData1(DateTime StartDate, DateTime EndDate)
        {
            var result = (dynamic)null;

            try
            {
                Task.Run(async () =>
                {
                    result = await (from Table in _materialDbContext.Gates
                                where Table.CreatedOn.Date >= StartDate.Date && Table.CreatedOn.Date <= EndDate.Date
                                // && Table.GID == Selected_GID
                                select new
                                {
                                    Date = Table.CreatedOn.Date,
                                    labels = Table.Gate_Location,
                                    data = Table.PID
                                }).ToListAsync().ConfigureAwait(false);
                }).GetAwaiter().GetResult();

                LogWriterClass.LogWrite("AjaxGateData1 BAL:Get AjaxGateData1 Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxGateData1 BAL:" + exc.Message.ToString(), _logFile);

            }


            return result;
        }

        public Login Index(Login model)
        {

            LogWriterClass.LogWrite("Login BAL:Model is valid", _logFile);


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
                LogWriterClass.LogWrite("Login BAL:Login Successfully", _logFile);

            }
            else
            {
                LogWriterClass.LogWrite("Login BAL:Validation Error", _logFile);
                return model;
            }
            return model;
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
        //business class closed
    }
    public class LogWriterClass
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

    public sealed record GraphData
    {
        public DateTime Date { get; set; }

        public int Completed { get; set; }

        public int Pending { get; set; }
    }

}
