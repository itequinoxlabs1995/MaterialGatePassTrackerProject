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
using MaterialGatePassTracker.DAL;

namespace MaterialGatePassTracker.BAL
{
    public interface IAuthBusinessLogicClass
    {
        
    }
    public class AuthBusinessLogicClass: IAuthBusinessLogicClass
    {
        private readonly AuthDataAccessLayer _DataAccessClass;

        public AuthBusinessLogicClass(AuthDataAccessLayer authDataAccess)
        {
            _DataAccessClass = authDataAccess;
        }

        public dynamic Login(Login model)
        {
            var user = (dynamic)null;

            try
            {

                user = _DataAccessClass.Login(model);

            }
            catch (Exception exc)
            {
                exc.Message.ToString();

            }
           
            return user;
        }


    }

    
}
