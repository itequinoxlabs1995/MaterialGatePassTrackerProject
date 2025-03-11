using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;

namespace MaterialGatePassTracker.BAL
{
    public interface IAuthService
    {
        Task<(bool, string)> LoginAsync(D_User model);
        Task<Response> GatePassEntryAsync(T_Gate_Pass request, List<string> filePaths);
        Task<IEnumerable<object>> GetGatePassEntriesAsync(DateTime startDate, DateTime endDate);
        Task<T_Gate_Pass?> GetGatePassByIdAsync(int gpid);
    }
}
