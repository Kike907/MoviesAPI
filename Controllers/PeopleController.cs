using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTO;
using MoviesAPI.Entities;
using MoviesAPI.Helper;
using MoviesAPI.Services.Interface;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        public PeopleController(IMapper mapper, ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

         [HttpGet]
        public async Task<ActionResult<List<PersonDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable  = _context.People.AsQueryable();
            await HttpContext.InsertPaginationParameters(queryable, pagination.RecordPerPage);
            var people =  await queryable.Paginate(pagination).ToListAsync();;
            var personDTOs = _mapper.Map<List<PersonDTO>>(people);
            
            return personDTOs;
        }

        [HttpGet]
        [Route("{id}", Name="getPerson")]
         public async Task<ActionResult<PersonDTO>> Get(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(X => X.Id == id);
            
            if (person == null)
            {
                 return NotFound();
            }
            var personDTO = _mapper.Map<PersonDTO>(person);
            return personDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PersonCreationDTO personCreation)
        {
            var person = _mapper.Map<Person>(personCreation);
            if (personCreation.Picture != null)
            {
                person.Picture = 
                    await _fileStorageService.SaveFile(personCreation.Picture);
            }
            _context.Add(person);
           await _context.SaveChangesAsync();
           var personDTO = _mapper.Map<PersonDTO>(person);
            return new CreatedAtRouteResult("getPerson", new {person.Id}, personDTO); 
        }

        // [HttpPut("{id}")]
        // public async Task<ActionResult> Put(int id, [FromForm] PersonCreationDTO personCreation)
        // {
        //     var personDB = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
        //     if (personDB == null)
        //         return NotFound();
            
        //     personDB = _mapper.Map(personCreation, personDB);
        //     if (personCreation.Picture != null)
        //     {
        //         personDB.Picture = 
        //             await _fileStorageService.EditFile(personDB.Picture, personCreation.Picture);
        //     }
        //     await _context.SaveChangesAsync();
        //     return NoContent();
        // }

        // [HttpPatch("{id}")]
        // public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<PersonPatchDTO> patchDocument)
        // {
        //     if (patchDocument == null)
        //         return NotFound();
        //     var entityFromDB = await _context.People.FirstOrDefaultAsync(p => p.Id == id);

        //     if (entityFromDB == null)
        //         return NotFound();

        //     var entityDTO = _mapper.Map<PersonPatchDTO>(entityFromDB);
        //     patchDocument.ApplyTo(entityDTO, ModelState);

        //     var isValid = TryValidateModel(entityDTO);
        //     if (!isValid)
        //         return BadRequest(ModelState);
            
        //     _mapper.Map(entityDTO, entityFromDB);
        //     await _context.SaveChangesAsync();
        //     return NoContent();
        // }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await _context.People.AnyAsync(x => x.Id == id);
            if(!exists)
            {
                return NotFound();
            }
            _context.Remove(new Person() {Id = id});
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}