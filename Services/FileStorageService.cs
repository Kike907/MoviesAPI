using System;
using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using MoviesAPI.Services.Interface;

namespace MoviesAPI.Services
{
    public class FileStorageService : IFileStorageService
    {
        public async Task<string> SaveFile(IFormFile image)
        {
            var account = new Account(
                Environment.GetEnvironmentVariable("CLOUDINARY_CLOUDNAME"),
                Environment.GetEnvironmentVariable("CLOUDINARY_APIKEY"),
                Environment.GetEnvironmentVariable("CLOUDINARY_SECRET")
            );
           Cloudinary cloudinary = new Cloudinary(account);
            var path = Path.GetFullPath(image.FileName);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(image.FileName, image.OpenReadStream()),
                PublicId = $"Osip/{image.FileName}"
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.Url.ToString();
        }

        public async Task<string> DeleteFile(string publicId)
        {
            var account = new Account(
                Environment.GetEnvironmentVariable("CLOUDINARY_CLOUDNAME"),
                Environment.GetEnvironmentVariable("CLOUDINARY_SECRET"),
                Environment.GetEnvironmentVariable("CLOUDINARY_APIKEY")
            );
           Cloudinary cloudinary = new Cloudinary(account);
            var deletionParams = new DeletionParams(publicId);
            var deletionResult = cloudinary.Destroy(deletionParams);
            return deletionResult.Result;
        }

        public async Task<string> EditFile(string publicId, IFormFile image)
        {
            await DeleteFile(publicId);
            return await SaveFile(image);
        }
    }
}