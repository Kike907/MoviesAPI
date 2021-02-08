using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MoviesAPI.Services.Interface
{
    public interface IFileStorageService
    {
        Task<string> SaveFile(IFormFile image);
        Task<string> DeleteFile(string publicId);
        Task<string> EditFile(string publicId, IFormFile image);
    }
}