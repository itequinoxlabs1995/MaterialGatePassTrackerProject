using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MaterialGatePassTracker.Services;
using Azure;

namespace MaterialGatePassTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryDetailsCreationController : Controller
    {

        private readonly string _connectionString = "Your_Storage_Account_Connection_String";
        private readonly string _containerName = "your-container-name";
        private readonly string _baseLocalPath = "E:\\Uploads"; // Change to your preferred local path
        private readonly EmailService _emailService;

        public EntryDetailsCreationController(EmailService emailService)
        {
            _emailService = emailService;
        }

        public class Response
        {
            public string Status { get; set; }
            public string Message { get; set; }
        }

        [HttpPost]
        [Route("UploadFiles")]
        public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files,
                                                         [FromQuery] string unit,
                                                         [FromQuery] string project,
                                                         [FromQuery] string gate)
        {
            if (files == null || files.Count != 4)
                return BadRequest("Exactly four files are required.");

            if (string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(gate))
                return BadRequest("Unit, Project, and Gate parameters are required.");

            var blobClient = new BlobContainerClient(_connectionString, _containerName);
            await blobClient.CreateIfNotExistsAsync();

            string date = DateTime.UtcNow.ToString("yyyyMMdd");
            string folderPath = $"{unit}/{project}/{gate}/{date}/"; // Dynamic folder path

            var uploadResults = new List<object>();

            foreach (var file in files)
            {
                var blob = blobClient.GetBlobClient(folderPath + file.FileName);
                using var stream = file.OpenReadStream();
                await blob.UploadAsync(stream, overwrite: true);

                uploadResults.Add(new { FileName = file.FileName, Url = blob.Uri.ToString() });
            }

            return Ok(uploadResults);
        }

        [HttpPost]
        [Route("UploadLocalFiles")]
        public async Task<IActionResult> UploadLocalFiles([FromForm] List<IFormFile> files,
                                                     [FromQuery] string unit,
                                                     [FromQuery] string project,
                                                     [FromQuery] string gate)
        {
            if (files == null)
                return BadRequest("files are required.");

            if (string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(gate))
                return BadRequest("Unit, Project, and Gate parameters are required.");

            string date = DateTime.UtcNow.ToString("yyyyMMdd");
            string folderPath = Path.Combine(_baseLocalPath, unit, project, gate, date);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uploadResults = new List<object>();

            try
            {
                foreach (var file in files)
                {
                    string filePath = Path.Combine(folderPath, file.FileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    uploadResults.Add(new { FileName = file.FileName, Path = filePath });
                }

                return Ok(uploadResults);
            }
            catch (Exception ex)
            {


                return Ok(new Response
                { Status = "Error", Message = "Files upload is Unsuccessfull" }
                         );
            }


        }

    }
}
