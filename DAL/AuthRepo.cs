using MaterialGatePassTacker.Models;
using MaterialGatePassTacker;
using Microsoft.EntityFrameworkCore;

namespace MaterialGatePassTracker.DAL
{
    public class AuthRepo : IAuthRepo
    {
        private readonly MaterialDbContext _context;

        public AuthRepo(MaterialDbContext context)
        {
            _context = context;
        }

        public async Task<D_User?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.User_Name == username);
        }

        public async Task InsertGatePassAsync(T_Gate_Pass request)
        {
            _context.GatesPasses.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> GetGatePassEntriesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.GatesPasses
                .Where(g => g.CreatedOn >= startDate && g.CreatedOn <= endDate)
                .Select(g => new { g.GPID, g.Vehicle_No })
                .ToListAsync();
        }

        public async Task<T_Gate_Pass?> GetGatePassByIdAsync(int gpid)
        {
            return await _context.GatesPasses.FindAsync(gpid);
        }
    }

}
