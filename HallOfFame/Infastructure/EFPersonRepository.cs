using HallOfFame.Core.Interfaces;
using HallOfFame.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HallOfFame.Infastructure
{
    public class EFPersonRepository : IPersonRepository
    {
        private readonly PersonContext _dbContext;

        public EFPersonRepository(PersonContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<Person> GetByIdAsync(int id)
        {
            return _dbContext.Persons
                .Include(p => p.Skills)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<List<Person>> ListAsync()
        {
            return _dbContext.Persons
                .Include(p => p.Skills)
                .ToListAsync();
        }

        public Task AddAsync(Person session)
        {
            _dbContext.Persons.Add(session);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(Person session)
        {
            _dbContext.Entry(session).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }
    }
}

