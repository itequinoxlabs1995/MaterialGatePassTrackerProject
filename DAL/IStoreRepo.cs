using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;

namespace MaterialGatePassTracker.DAL
{
    public interface IStoreRepo
    {
        Task<List<T_Gate_Pass>> GetGatePassesByUserIdAsync(int userId);
        Task UpdateGatePassStatusAsync(GatePassStatusRequest request);
        Task<string?> GetUserEmailByUserNameAsync(string userName);
    }
}
