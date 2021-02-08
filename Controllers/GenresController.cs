
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesAPI.DTO;
using MoviesAPI.Entities;
using MoviesAPI.Filters;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {  
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GenresController(IMapper mapper, ApplicationDbContext context, ILogger<GenresController> logger)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes =  JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var genres =  await _context.Genres.AsNoTracking().ToListAsync();;
            var genreDTOs = _mapper.Map<List<GenreDTO>>(genres);
            _logger.LogInformation("Gets all genre information");
            return genreDTOs;
        }

        [HttpGet]
        [Route("{id}", Name="getGenre")]
         public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            
            if (genre == null)
            {
                _logger.LogError("genre not found");
                 return NotFound();
            }
            var genreDTO = _mapper.Map<GenreDTO>(genre);
            return genreDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreation)
        {
            var genre = _mapper.Map<Genre>(genreCreation);
            _context.Add(genre);
           await _context.SaveChangesAsync();
           var genreDTO = _mapper.Map<GenreDTO>(genre);
            return new CreatedAtRouteResult("getGenre", new {genreDTO.Id}, genreDTO); 
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreation)
        {

            var genre = _mapper.Map<Genre>(genreCreation);
            genre.Id = id;
            _context.Entry(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _context.Genres.AnyAsync(x => x.Id == id);
            if(!exists)
            {
                return NotFound();
            }
            _context.Remove(new Genre() {Id = id});
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}