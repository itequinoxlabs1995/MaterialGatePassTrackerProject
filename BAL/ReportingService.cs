using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;
using MaterialGatePassTracker.DAL;
using MaterialGatePassTracker.Services;
using MaterialGatePassTracker.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaterialGatePassTracker.BAL
{
    public class ReportingService : IReportingService
    {
        private readonly IReportingRepo _repository;

        public ReportingService(IReportingRepo repository)
        {
            _repository = repository;
        }

        public async Task<List<M_SOU>> GetActiveSOUsAsync()
        {
            try
            {
                return await _repository.GetActiveSOUsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Business error: Unable to fetch SOUs", ex);
            }
        }

        public async Task<List<M_Project>> GetProjectsBySOUAsync(int souId)
        {
            try
            {
                return await _repository.GetProjectsBySOUAsync(souId);
            }
            catch (Exception ex)
            {
                throw new Exception("Business error: Unable to fetch projects", ex);
            }
        }

        public async Task<List<M_Gate>> GetGatesByProjectAsync(int projectId)
        {
            try
            {
                return await _repository.GetGatesByProjectAsync(projectId);
            }
            catch (Exception ex)
            {
                throw new Exception("Business error: Unable to fetch gates", ex);
            }
        }

        public async Task<PaginatedList<T_Gate_Pass>> GetFilteredGatePassesAsync(int? souId, int? projectId, int? gateId, string dateRange, int pageSize, int pageNumber)
        {
            try
            {
                return await _repository.GetFilteredGatePassesAsync(souId, projectId, gateId, dateRange, pageSize, pageNumber);
            }
            catch (Exception ex)
            {
                throw new Exception("Business error: Unable to fetch filtered gate pass data", ex);
            }
        }
    }
}
