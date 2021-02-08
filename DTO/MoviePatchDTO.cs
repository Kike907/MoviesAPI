using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MoviesAPI.Validations;

namespace MoviesAPI.DTO
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        public string Summary { get; set; }

        public bool InTheatres { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}