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
using MaterialGatePassTracker.BAL;

namespace MaterialGatePassTracker.Controllers
{
    [Route("Reporting")]
    public class ReportingController : Controller
    {
        private readonly IReportingService _gatePassService;
        private readonly ICompositeViewEngine _viewEngine;

        public ReportingController(IReportingService gatePassService, ICompositeViewEngine viewEngine)
        {
            _gatePassService = gatePassService;
            _viewEngine = viewEngine;
        }

        [HttpGet("Page/{pageNumber?}")]
        public async Task<IActionResult> Index(int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                ViewBag.SOUs = await _gatePassService.GetActiveSOUsAsync();
                ViewBag.PageSize = pageSize;

                var paginatedList = await _gatePassService.GetFilteredGatePassesAsync(null, null, null, null, pageSize, pageNumber);
                return View(paginatedList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching the data.");
            }
        }

        [HttpGet("GetProjects/{souId}")]
        public async Task<JsonResult> GetProjects(int souId)
        {
            try
            {
                var projects = await _gatePassService.GetProjectsBySOUAsync(souId);
                return Json(projects);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error fetching projects" });
            }
        }

        [HttpGet("GetGates/{projectId}")]
        public async Task<JsonResult> GetGates(int projectId)
        {
            try
            {
                var gates = await _gatePassService.GetGatesByProjectAsync(projectId);
                return Json(gates);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error fetching gates" });
            }
        }

        [HttpGet("FilterData")]
        public async Task<JsonResult> FilterData(int? souId, int? projectId, int? gateId, string dateRange, int pageSize, int pageNumber = 1)
        {
            try
            {
                var paginatedList = await _gatePassService.GetFilteredGatePassesAsync(souId, projectId, gateId, dateRange, pageSize, pageNumber);
                var tableHtml = await RenderViewToStringAsync("_GatePassTable", paginatedList);

                return Json(new
                {
                    tableHtml,
                    pageIndex = paginatedList.PageIndex,
                    totalPages = paginatedList.TotalPages
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error fetching filtered data" });
            }
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error rendering view", ex);
            }
        }
    }


}
