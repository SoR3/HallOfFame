using HallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;
        private readonly ILogger<PersonController> _logger;

        public PersonController(PersonContext context, ILogger<PersonController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/v1/person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            _logger.LogInformation($"api/v1/persons GET request at {DateTime.Now:hh:mm:ss}");

            try
            {
                var persons = await _context.Persons.Include(p => p.Skills).ToListAsync();

                if (!persons.Any())
                {
                    _logger.LogError("Not found persons");
                    return NotFound("Not found Persons");
                }

                return Ok(persons);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            
        }

        // GET: api/v1/person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            _logger.LogInformation($"api/v1/person GET request at {DateTime.Now:hh:mm:ss}");
            try
            {
                var person = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(x => x.Id == id);

                if (person == null)
                {
                    _logger.LogError($"Not found person {id}");
                    return NotFound($"Not found Person {id}");
                }

                return Ok(person);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
           
        }

        // PUT: api/v1/person/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            _logger.LogInformation($"api/v1/person PUT request at {DateTime.Now:hh:mm:ss}");

            if (id != person.Id)
            {
                _logger.LogError($"BadRequest: person {id} not found");
                return BadRequest("BadRequest");
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    _logger.LogError($"Not found Person {id}");
                    return NotFound($"Not found Person {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Data base updated!");
        }

        // POST: api/v1/person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _logger.LogInformation($"api/v1/person POST request at {DateTime.Now:hh:mm:ss}");

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/v1/person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            _logger.LogInformation($"api/v1/person DELET request at {DateTime.Now:hh:mm:ss}");

            try
            {
                var person = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(x => x.Id == id);
                if (person == null)
                {
                    _logger.LogError($"Not found person {id}");
                    return NotFound($"Not found Person {id}");
                }

                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();

                return Ok($"Person {id} has been deleted");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            
        }

        private bool PersonExists(long id)
        {
            return _context.Persons.Include(p => p.Skills).Any(e => e.Id == id);
        }
    }
}
