using MaterialGatePassTacker.Models;
using MaterialGatePassTacker;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;

namespace MaterialGatePassTracker.DAL
{
    public class EntryDetailsCreationRepo : IEntryDetailsCreationRepo
    {
        private readonly string _connectionString = "Your_Storage_Account_Connection_String";
        private readonly string _containerName = "your-container-name";
        private readonly string _baseLocalPath = "E:\\Uploads"; // Change to your preferred local path

        public async Task<List<object>> UploadFilesToBlob(List<IFormFile> files, string unit, string project, string gate)
        {
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

            return uploadResults;
        }

        public async Task<List<object>> UploadFilesLocally(List<IFormFile> files, string unit, string project, string gate)
        {
            string date = DateTime.UtcNow.ToString("yyyyMMdd");
            string folderPath = Path.Combine(_baseLocalPath, unit, project, gate, date);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uploadResults = new List<object>();

            foreach (var file in files)
            {
                string filePath = Path.Combine(folderPath, file.FileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                uploadResults.Add(new { FileName = file.FileName, Path = filePath });
            }

            return uploadResults;
        }
    }
}
