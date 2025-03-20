using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Helpers;
using MaterialGatePassTracker.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MaterialGatePassTracker.DAL
{
    public class ReportingRepo : IReportingRepo
    {
        private readonly MaterialDbContext _context;

        public ReportingRepo(MaterialDbContext context)
        {
            _context = context;
        }

        public async Task<List<M_SOU>> GetActiveSOUsAsync()
        {
            try
            {
                return await _context.SOUs.Where(s => s.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching SOUs", ex);
            }
        }

        public async Task<List<M_Project>> GetProjectsBySOUAsync(int souId)
        {
            try
            {
                return await _context.Projects.Where(p => p.SOUID == souId && p.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching projects", ex);
            }
        }

        public async Task<List<M_Gate>> GetGatesByProjectAsync(int projectId)
        {
            try
            {
                return await _context.Gates.Where(g => g.PID == projectId && g.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching gates", ex);
            }
        }

        public async Task<PaginatedList<T_Gate_Pass>> GetFilteredGatePassesAsync(int? souId, int? projectId, int? gateId, string dateRange, int pageSize, int pageNumber)
        {
            try
            {
                var query = _context.GatesPasses.AsQueryable();

                if (souId.HasValue)
                    query = query.Where(gp => _context.Projects.Any(p => p.PID == gp.PID && p.SOUID == souId));

                if (projectId.HasValue)
                    query = query.Where(gp => gp.PID == projectId);

                if (gateId.HasValue)
                    query = query.Where(gp => gp.Gate_ID == gateId);

                if (!string.IsNullOrEmpty(dateRange))
                {
                    var dates = dateRange.Split(" - ");
                    if (DateTime.TryParse(dates[0], out DateTime fromDate) && DateTime.TryParse(dates[1], out DateTime toDate))
                    {
                        query = query.Where(gp => gp.CreatedOn >= fromDate && gp.CreatedOn <= toDate);
                    }
                }

                return await PaginatedList<T_Gate_Pass>.CreateAsync(query.OrderByDescending(g => g.CreatedOn), pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("Error filtering gate pass data", ex);
            }

        }
    }
}
