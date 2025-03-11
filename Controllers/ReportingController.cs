using MaterialGatePassTacker;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MaterialGatePassTracker.Controllers
{
    [Route("Reporting")]
    public class ReportingController : Controller
    {
        private readonly MaterialDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;

        public ReportingController(MaterialDbContext context, ICompositeViewEngine viewEngine)
        {
            _context = context;
            _viewEngine = viewEngine;
        }

        [HttpGet("Page/{pageNumber?}")]
        public async Task<IActionResult> Index(int pageSize = 10, int pageNumber = 1)
        {
            ViewBag.SOUs = await _context.SOUs.Where(s => s.IsActive).ToListAsync();
            ViewBag.PageSize = pageSize;

            var gatePasses = _context.GatesPasses.AsQueryable();

            var paginatedList = await PaginatedList<T_Gate_Pass>.CreateAsync(gatePasses.OrderByDescending(g => g.CreatedOn), pageNumber, pageSize);

            return View(paginatedList);
        }

        // Fetch all projects based on SOU selection
        [HttpGet("GetProjects/{souId}")]
        public async Task<JsonResult> GetProjects(int souId)
        {
            var projects = await _context.Projects.Where(p => p.SOUID == souId && p.IsActive).ToListAsync();
            return Json(projects);
        }

        // Fetch all gates based on project selection
        [HttpGet("GetGates/{projectId}")]
        public async Task<JsonResult> GetGates(int projectId)
        {
            var gates = await _context.Gates.Where(g => g.PID == projectId && g.IsActive).ToListAsync();
            return Json(gates);
        }

        // Filters and loads data based on selected criteria
        [HttpGet("FilterData")]
        public async Task<JsonResult> FilterData(int? souId, int? projectId, int? gateId, string dateRange, int pageSize, int pageNumber = 1)
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

            var paginatedList = await PaginatedList<T_Gate_Pass>.CreateAsync(query, pageNumber, pageSize);

            var tableHtml = await this.RenderViewToStringAsync("_GatePassTable", paginatedList);

            return Json(new
            {
                tableHtml,
                pageIndex = paginatedList.PageIndex,
                totalPages = paginatedList.TotalPages
            });
        }

        public async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            ViewData.Model = model;
            using var writer = new StringWriter();
            var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

            if (!viewResult.Success)
                throw new InvalidOperationException($"View {viewName} not found");

            var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer, new HtmlHelperOptions());
            await viewResult.View.RenderAsync(viewContext);

            return writer.ToString();
        }



    }

}
