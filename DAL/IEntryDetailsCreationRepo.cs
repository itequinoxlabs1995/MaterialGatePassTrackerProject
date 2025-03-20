namespace MaterialGatePassTracker.DAL
{
    public interface IEntryDetailsCreationRepo
    {
        Task<List<object>> UploadFilesToBlob(List<IFormFile> files, string unit, string project, string gate);
        Task<List<object>> UploadFilesLocally(List<IFormFile> files, string unit, string project, string gate);
    }
}
