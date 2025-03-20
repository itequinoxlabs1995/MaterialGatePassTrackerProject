using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MaterialGatePassTracker.Services;
using MaterialGatePassTracker.BAL;
using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;

namespace MaterialGatePassTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryDetailsCreationController : ControllerBase
    {
        private readonly IEntryDetailsCreationService _fileService;
        private readonly EmailService _emailService;

        public EntryDetailsCreationController(IEntryDetailsCreationService fileService, EmailService emailService)
        {
            _fileService = fileService;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("UploadFiles")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files,
                                                     [FromQuery] string unit,
                                                     [FromQuery] string project,
                                                     [FromQuery] string gate)
        {
            try
            {
                var uploadResults = await _fileService.HandleFileUploadToBlob(files, unit, project, gate);
                return Ok(uploadResults);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "File upload failed: " + ex.Message
                });
            }
        }

        [HttpPost]
        [Route("UploadLocalFiles")]
        public async Task<IActionResult> UploadLocalFiles([FromForm] List<IFormFile> files,
                                                         [FromQuery] string unit,
                                                         [FromQuery] string project,
                                                         [FromQuery] string gate)
        {
            try
            {
                var uploadResults = await _fileService.HandleFileUploadLocally(files, unit, project, gate);
                return Ok(uploadResults);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "File upload failed: " + ex.Message
                });
            }
        }
    }

}
