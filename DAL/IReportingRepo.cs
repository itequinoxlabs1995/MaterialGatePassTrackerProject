using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaterialGatePassTracker.DAL
{
    public interface IReportingRepo
    {
        Task<List<M_SOU>> GetActiveSOUsAsync();
        Task<List<M_Project>> GetProjectsBySOUAsync(int souId);
        Task<List<M_Gate>> GetGatesByProjectAsync(int projectId);
        Task<PaginatedList<T_Gate_Pass>> GetFilteredGatePassesAsync(int? souId, int? projectId, int? gateId, string dateRange, int pageSize, int pageNumber);
    }
}
