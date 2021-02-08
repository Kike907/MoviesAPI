using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Entities
{
    public class Movie
    {
        public int id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        public string Summary { get; set; }

        public bool InTheatres { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Poster { get; set; }
    
        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
    }
}