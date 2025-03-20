using MaterialGatePassTacker.Models;
using MaterialGatePassTracker.Models;
using MaterialGatePassTracker.DAL;
using MaterialGatePassTracker.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MaterialGatePassTracker.BAL
{
    public class EntryDetailsCreationService : IEntryDetailsCreationService
    {
        private readonly IEntryDetailsCreationRepo _fileRepository;

        public EntryDetailsCreationService(IEntryDetailsCreationRepo fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<List<object>> HandleFileUploadToBlob(List<IFormFile> files, string unit, string project, string gate)
        {
            if (files == null || files.Count != 4)
                throw new ArgumentException("Exactly four files are required.");

            if (string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(gate))
                throw new ArgumentException("Unit, Project, and Gate parameters are required.");

            return await _fileRepository.UploadFilesToBlob(files, unit, project, gate);
        }

        public async Task<List<object>> HandleFileUploadLocally(List<IFormFile> files, string unit, string project, string gate)
        {
            if (files == null)
                throw new ArgumentException("Files are required.");

            if (string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(project) || string.IsNullOrEmpty(gate))
                throw new ArgumentException("Unit, Project, and Gate parameters are required.");

            return await _fileRepository.UploadFilesLocally(files, unit, project, gate);
        }
    }
}
