using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HallOfFame.Models;

namespace HallOfFame.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;

        public PersonController(PersonContext context)
        {
            _context = context;
        }

        // GET: api/v1/person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var persons = await _context.Persons.Include(p => p.Skills).ToListAsync();

            if(!persons.Any())
            {
                //TODO: add logging
                return NotFound("Not found Persons");
            }
            //TODO: add logging
            return Ok(persons);        
        }

        // GET: api/v1/person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(x => x.Id == id);

            if (person == null)
            {
                //TODO: add logging
                return NotFound($"Not found Person {id}");
            }

            //TODO: add logging
            return Ok(person);
        }

        // PUT: api/v1/person/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.Id)
            {
                //TODO: add logging
                return BadRequest($"Not found Person {id}");
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
                    //TODO: add logging
                    return NotFound($"Not found Person {id}");
                }
                else
                {
                    throw;
                }
            }
            //TODO: add logging
            return Ok("Data base updated!");
        }

        // POST: api/v1/person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/v1/person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
            { 
                //TODO: add logging
                return NotFound($"Not found Person {id}");
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            //TODO: add logging
            return Ok($"Person {id} has been deleted");
        }

        private bool PersonExists(long id)
        {
            return _context.Persons.Include(p=>p.Skills).Any(e => e.Id == id);
        }
    }
}
