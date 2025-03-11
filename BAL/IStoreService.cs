using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;

namespace MaterialGatePassTracker.BAL
{
    public interface IStoreService
    {
        Task<List<T_Gate_Pass>> GetGatePassesByUserIdAsync(int userId);
        Task UpdateGatePassStatusAsync(GatePassStatusRequest request);
    }
}
