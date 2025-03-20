namespace MaterialGatePassTracker.BAL
{
    public interface IEntryDetailsCreationService
    {
        Task<List<object>> HandleFileUploadToBlob(List<IFormFile> files, string unit, string project, string gate);
        Task<List<object>> HandleFileUploadLocally(List<IFormFile> files, string unit, string project, string gate);
    }
}

