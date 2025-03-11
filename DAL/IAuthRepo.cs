using MaterialGatePassTacker.Models;

namespace MaterialGatePassTracker.DAL
{
    public interface IAuthRepo
    {
        Task<D_User?> GetUserAsync(string username);
        Task InsertGatePassAsync(T_Gate_Pass request);
        Task<IEnumerable<object>> GetGatePassEntriesAsync(DateTime startDate, DateTime endDate);
        Task<T_Gate_Pass?> GetGatePassByIdAsync(int gpid);
    }
}
