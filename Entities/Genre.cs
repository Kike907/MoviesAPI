using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoviesAPI.Validations;

namespace  MoviesAPI.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
    }
}