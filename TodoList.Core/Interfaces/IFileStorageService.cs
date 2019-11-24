using System.IO;
using System.Threading.Tasks;

namespace TodoList.Core.Interfaces
{
    public interface IFileStorageService
    {
        Task<bool> SaveFileAsync(string path, Stream stream);
        Task<bool> DeleteFileAsync(string path, string containingFolder);
        Task<bool> ExistsAsync(string path);
        Task<Stream> GetFileStreamAsync(string path);
        Task<bool> CleanDirectoryAsync(string targetPath);
    }
}