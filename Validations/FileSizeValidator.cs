using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MoviesAPI.Validations
{
    public class FileSizeValidator : ValidationAttribute
    {
        private readonly int _maxFileSizeInMbs;
        public FileSizeValidator(int maxFileSizeInMbs)
        {
            _maxFileSizeInMbs = maxFileSizeInMbs;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;
            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (formFile.Length > _maxFileSizeInMbs * 1024 * 1024)
            {
                return new ValidationResult($"File size cannot be bigger than {_maxFileSizeInMbs} megabytes");
            }

            return ValidationResult.Success;
        }
    }
}