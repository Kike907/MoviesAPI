
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTO;
using MoviesAPI.Entities;
using MoviesAPI.Services.Interface;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        public MoviesController(IMapper mapper, ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

         [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get()
        {
            // var queryable  = _context.Movies.AsQueryable();
            // await HttpContext.InsertPaginationParameters(queryable, pagination.RecordPerPage);
            // var Movies =  await queryable.Paginate(pagination).ToListAsync();
            var movies = await _context.Movies.ToListAsync();
            var moviesDTOs = _mapper.Map<List<MovieDTO>>(movies);
            
            return moviesDTOs;
        }

         [HttpGet("{Id:int}", Name="getMovie")]
         public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(X => X.id == id);
            
            if (movie == null)
            {
                 return NotFound();
            }
            var movieDTO = _mapper.Map<MovieDTO>(movie);
            return movieDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreation)
        {
            var movie = _mapper.Map<Movie>(movieCreation);
            if (movieCreation.Poster != null)
            {
                movie.Poster = 
                    await _fileStorageService.SaveFile(movieCreation.Poster);
            
            }
            AnnotateActorsOrder(movie);

            _context.Add(movie);
           await _context.SaveChangesAsync();
           var movieDTO = _mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new {movie.id}, movieDTO); 
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreation)
        {
            var movieDB = await _context.Movies.FirstOrDefaultAsync(x => x.id == id);
            if (movieDB == null)
                return NotFound();
            
            movieDB = _mapper.Map(movieCreation, movieDB);
            if (movieCreation.Poster != null)
            {
                movieDB.Poster = 
                    await _fileStorageService.EditFile(movieDB.Poster, movieCreation.Poster);
                
            }
            await _context.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movieDB.id}; delete from MoviesGenres where MovieId = {movieDB.id}");
            AnnotateActorsOrder(movieDB);

            await _context.SaveChangesAsync();
            return NoContent();        
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            if (patchDocument == null)
                return NotFound();
            var entityFromDB = await _context.Movies.FirstOrDefaultAsync(p => p.id == id);

            if (entityFromDB == null)
                return NotFound();

            var entityDTO = _mapper.Map<MoviePatchDTO>(entityFromDB);
            patchDocument.ApplyTo(entityDTO, ModelState);

            var isValid = TryValidateModel(entityDTO);
            if (!isValid)
                return BadRequest(ModelState);
            
            _mapper.Map(entityDTO, entityFromDB);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _context.Movies.AnyAsync(x => x.id == id);
            if(!exists)
            {
                return NotFound();
            }
            _context.Remove(new Movie() {id = id});
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private static void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for(int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }
    }
}