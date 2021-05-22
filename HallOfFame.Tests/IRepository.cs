using HallOfFame.Models;
using System.Collections.Generic;

namespace HallOfFame.Tests
{
    internal interface IRepository
    {
        IEnumerable<Person> GetAll(); // получение всех объектов
        Person GetPerson(int id); // получение одного объекта по id
        void Create(Person item); // создание объекта
        void Update(Person item); // обновление объекта
        void Delete(int id); // удаление объекта по id
        void Save();  // сохранение изменений
    }
}