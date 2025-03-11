using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using Microsoft.EntityFrameworkCore;

namespace MaterialGatePassTracker.BAL
{
    public interface IBusinessClass
    { 
        Task<IEnumerable<D_User>> GetUsersData();
    }

    public class BusinessClass : IBusinessClass
    {
        private readonly MaterialDbContext _appDBContext;

        public BusinessClass(MaterialDbContext materialDbContext)
        {
            _appDBContext = materialDbContext;
        }
        public async Task<IEnumerable<D_User>> GetUsersData()
        {
            return await _appDBContext.Users.ToListAsync();
        }

    }
}
