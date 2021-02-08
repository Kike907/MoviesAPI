using System.ComponentModel.DataAnnotations;
using MoviesAPI.Validations;

namespace MoviesAPI.DTO
{
    public class GenreCreationDTO
    {
        
        [Required]
        [StringLength(40)]
        public string Name { get; set; }

    }
}