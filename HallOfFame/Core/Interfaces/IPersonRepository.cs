using HallOfFame.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HallOfFame.Core.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> GetByIdAsync(int id);
        Task<List<Person>> ListAsync();
        Task AddAsync(Person session);
        Task UpdateAsync(Person session);
    }
}
