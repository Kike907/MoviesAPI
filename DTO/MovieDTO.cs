using System;

namespace MoviesAPI.DTO
{
    public class MovieDTO
    {
        public int id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public bool InTheatres { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Poster { get; set; }
    
    }
}