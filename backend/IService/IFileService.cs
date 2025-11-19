namespace Backend.IService.IFileService;

public interface IFileService
{
    public Task<string> SaveFileToDisk(IFormFile file);
    public Task<byte[]> GetFileFromDisk(string fileName);

    public List<string> GetAllFiles();
}