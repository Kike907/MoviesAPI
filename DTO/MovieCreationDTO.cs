using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTO;
using MoviesAPI.Helper;
using MoviesAPI.Validations;

namespace MoviesAPI.DTO
{
    public class MovieCreationDTO : MoviePatchDTO
    {
        [FileSizeValidator(maxFileSizeInMbs: 4)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; } 

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorDTO>>))]
        public List<ActorDTO> Actors { get; set; } 

    }
}