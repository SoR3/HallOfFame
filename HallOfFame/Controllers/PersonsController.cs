using HallOfFame.Core.Models;
using HallOfFame.Infastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HallOfFame.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        /// <summary>
        /// База данных.
        /// </summary>
        private readonly PersonContext _context;

        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger<PersonsController> _logger;

        /// <summary>
        /// Конструктор контроллера для работы с сотрудниками
        /// </summary>
        /// <param name="context">База данныз</param>
        /// <param name="logger">Логгер</param>
        public PersonsController(PersonContext context, ILogger<PersonsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Показать список сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet("persons")]
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

        /// <summary>
        /// Показать сотрудника по идентификатору
        /// </summary>
        /// <param name="id">идентификатор сотрудника</param>
        /// <returns></returns>
        [HttpGet("person/{id}")]
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

        /// <summary>
        /// Изменить данные о сотруднике по идентификатору
        /// </summary>
        /// <param name="id">идентификатор сотрудника</param>
        /// <param name="person">сотрудник</param>
        /// <returns></returns>
        [HttpPut("person/{id}")]
        public async Task<IActionResult> PutPerson(long id,[FromBody] Person person)
        {
            _logger.LogInformation($"api/v1/person PUT request at {DateTime.Now:hh:mm:ss}");

            try
            {
                if (id != person.Id)
                {
                    _logger.LogError($"BadRequest: person {id} not found");
                    return BadRequest("BadRequest");
                }

                person.Name = person.Name;
                person.DisplayName = person.DisplayName;
                person.Skills.AddRange(person.Skills);
                _context.Skills.UpdateRange();
                _context.Persons.Update(person);
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

        /// <summary>
        /// Добавить нового сотрудника
        /// </summary>
        /// <param name="person">сотрудник</param>
        /// <returns></returns>
        [HttpPost("person")]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _logger.LogInformation($"api/v1/person POST request at {DateTime.Now:hh:mm:ss}");

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        /// <summary>
        /// Удаление сотрудника по идентификатору
        /// </summary>
        /// <param name="id">идентификатор сотрудника</param>
        /// <returns></returns>
        [HttpDelete("person/{id}")]
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

                _context.RemoveRange(person.Skills);
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

        /// <summary>
        /// Проверка на существование сотрдуника в БД
        /// </summary>
        /// <param name="id">идентификатор сотрудника</param>
        /// <returns></returns>
        private bool PersonExists(long id)
        {
            return _context.Persons.Include(p => p.Skills).Any(e => e.Id == id);
        }
    }
}
