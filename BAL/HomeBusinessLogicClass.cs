using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Controllers;
using MaterialGatePassTracker.DAL;
using MaterialGatePassTracker.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
        private readonly HomeDataAccessLayer _DataAccessClass;
        private readonly string _logFile;

        public HomeBusinessLogicClass(HomeDataAccessLayer homeDataAccess, IConfiguration config)
        {
            _logFile = config["Logging:LogFilePath"].ToString();
            _DataAccessClass = homeDataAccess;

        }

        public IEnumerable<D_User> AllUsers()
        {
            IEnumerable<D_User> newList = null;
            try
            {
                newList = _DataAccessClass.AllUsers();
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
                newList = _DataAccessClass.AllRoles();
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
                newList = _DataAccessClass.AllProjects();
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
            IEnumerable<D_User> newList = null;

            try
            {
                newList = _DataAccessClass.AddUser(userModel);

                LogWriterClass.LogWrite("AddUser BAL:Insert UserData Successfully", _logFile);
                
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddUser BAL:" + exc.Message.ToString(), _logFile);
            }

            return newList;
        }


        public IEnumerable<M_Role> AddRole(M_RoleViewModel userModel)
        {
            IEnumerable<M_Role> newList = null;
            try
            {

                newList = _DataAccessClass.AddRole(userModel);
                LogWriterClass.LogWrite("AddRole BAL:Insert Role Successfully ", _logFile);
                    //for list return


            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddRole BAL:" + exc.Message.ToString(), _logFile);

            }
            return  newList;
        }


        public IEnumerable<M_Project> AddProject(M_ProjectViewModel userModel)
        {
            IEnumerable<M_Project> newList = null;
            try
            {
                newList = _DataAccessClass.AddProject(userModel);
                LogWriterClass.LogWrite("AddProject BAL:Insert Project Successfully ", _logFile);

                 //for list return

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AddProject BAL:" + exc.Message.ToString(), _logFile);

            }
            return newList;


        }


        public IEnumerable<D_User> EditUser(D_UserViewModel model) 
        {
            IEnumerable<D_User> newList= null;
            try
            {
                newList = _DataAccessClass.EditUser(model);
                LogWriterClass.LogWrite("EditUser BAL:Edit User Successfully ", _logFile);
                
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditUser BAL:" + exc.Message.ToString(), _logFile);

            }

            return newList;

        }


        public IEnumerable<M_Project> EditProject(M_ProjectViewModel model)
        {
            IEnumerable<M_Project> newList= null;
            try
            {
            
              newList = _DataAccessClass.EditProject(model);
              LogWriterClass.LogWrite("EditProject BAL:Edit Project Successfully ", _logFile);
               
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("EditProject BAL:" + exc.Message.ToString(), _logFile);

            }

            return newList;

        }


        public D_UserViewModel EditUserData(int ID)
        {

            D_UserViewModel model = new D_UserViewModel();

            try
            {
                model = _DataAccessClass.EditUserData(ID);
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
                model = _DataAccessClass.EditProjectData(ID);
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
                model = _DataAccessClass.RoleList();
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
                model = _DataAccessClass.GateList();
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
            IEnumerable<D_User> newList = null;
            try
            {
                newList = _DataAccessClass.DeleteUser(id);
                LogWriterClass.LogWrite("DeleteUser BAL:User details deleted successfully", _logFile);

                
            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("DeleteUser BAL:" + exc.Message.ToString(), _logFile);

            }
            return newList;
        }


        public IEnumerable<M_Project> DeleteProject(int id)       
        {
            IEnumerable<M_Project> newList = null;
            try
            {

             newList = _DataAccessClass.DeleteProject(id);
                LogWriterClass.LogWrite("DeleteProject BAL:Project details deleted successfully", _logFile);
           
            }

            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("DeleteUser BAL:" + exc.Message.ToString(), _logFile);


            }
            return newList;
        }

        public T_Gate_Pass Dashboard()
        {
            T_Gate_Pass t_Gate_Pass = new();
            try
            {

                t_Gate_Pass = _DataAccessClass.Dashboard();
                LogWriterClass.LogWrite("Dashboard BAL:Get Soulist Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("Dashboard BAL:" + exc.Message.ToString(), _logFile);

            }

            return t_Gate_Pass;

        }

        public dynamic TodayCountCompleted()
        {
            var TodayCountCompleted = (dynamic)null;
            try
            {
                TodayCountCompleted = _DataAccessClass.TodayCountCompleted();

            }
            catch (Exception exc)
            {
                LogWriterClass.LogWrite("TodayCountCompleted BAL:" + exc.Message.ToString(), _logFile);

            }
            return TodayCountCompleted;

        }

        public dynamic WeeklyCountCompleted()
        {
            var WeeklyCountCompleted = (dynamic)null;
            try
            {
                WeeklyCountCompleted = _DataAccessClass.WeeklyCountCompleted();

            }
            catch (Exception exc)
            {
                LogWriterClass.LogWrite("WeeklyCountCompleted BAL:" + exc.Message.ToString(), _logFile);

            }
            return WeeklyCountCompleted;

        }

        public dynamic MonthCountCompleted()
        {
            var MonthCountCompleted = (dynamic)null;
            try
            {
                MonthCountCompleted = _DataAccessClass.MonthCountCompleted();

            }
            catch (Exception exc)
            {
                LogWriterClass.LogWrite("MonthCountCompleted BAL:" + exc.Message.ToString(), _logFile);

            }
            return MonthCountCompleted;

        }

        public dynamic TodayCountPending()
        {
            var TodayCountPending = (dynamic)null;
            try
            {
                TodayCountPending = _DataAccessClass.TodayCountPending();

            }
            catch (Exception exc)
            {
                LogWriterClass.LogWrite("TodayCountPending BAL:" + exc.Message.ToString(), _logFile);

            }
            return TodayCountPending;

        }

        public dynamic WeeklyCountPending()
        {
            var WeeklyCountPending = (dynamic)null;
            try
            {
                WeeklyCountPending = _DataAccessClass.WeeklyCountPending();

            }
            catch (Exception exc)
            {
                LogWriterClass.LogWrite("WeeklyCountPending BAL:" + exc.Message.ToString(), _logFile);

            }
            return WeeklyCountPending;

        }

        public dynamic MonthCountPending()
        {
            var MonthCountPending = (dynamic)null;
            try
            {
                MonthCountPending = _DataAccessClass.MonthCountPending();

            }
            catch (Exception exc)
            {
                LogWriterClass.LogWrite("MonthCountPending BAL:" + exc.Message.ToString(), _logFile);

            }
            return MonthCountPending;

        }

        public T_Gate_Pass AjaxMethod_CascadingList(string type, int IDCast)
        {
            T_Gate_Pass model = new();

            try
            {

                model = _DataAccessClass.AjaxMethod_CascadingList(type, IDCast);
                LogWriterClass.LogWrite("AjaxMethod_CascadingList BAL:Get AjaxMethod_CascadingList Successfully", _logFile);

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
               
                result = _DataAccessClass.AjaxMethod(StartDate, EndDate);
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
                
                result = _DataAccessClass.AjaxSouData(StartDate, EndDate, Selected_SOU);
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

                result = _DataAccessClass.AjaxProjectData(StartDate, EndDate, Selected_PID);
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

                result = _DataAccessClass.AjaxGateData(StartDate, EndDate, Selected_GID);
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

                result = _DataAccessClass.AjaxSouData1(StartDate, EndDate);
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

                result = _DataAccessClass.AjaxProjectData1(StartDate, EndDate);
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
                result = _DataAccessClass.AjaxGateData1(StartDate, EndDate);
                LogWriterClass.LogWrite("AjaxGateData1 BAL:Get AjaxGateData1 Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("AjaxGateData1 BAL:" + exc.Message.ToString(), _logFile);

            }


            return result;
        }

        public dynamic Index(Login model)
        {
            var Uservalid = (dynamic)null;
            try
            {
                Uservalid = _DataAccessClass.Index(model);
                LogWriterClass.LogWrite("Index BAL: Login Successfully", _logFile);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();
                LogWriterClass.LogWrite("Index BAL:" + exc.Message.ToString(), _logFile);

            }

            return Uservalid;
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
    public sealed record GraphData
    {
        public DateTime Date { get; set; }

        public int Completed { get; set; }

        public int Pending { get; set; }
    }

}
