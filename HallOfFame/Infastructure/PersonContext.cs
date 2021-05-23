using Microsoft.EntityFrameworkCore;
using HallOfFame.Core.Models;

namespace HallOfFame.Infastructure
{
    public class PersonContext : DbContext
    {
        /// <summary>
        /// Таблица персонажей
        /// </summary>
        public DbSet<Person> Persons { get; set; }
        /// <summary>
        /// Таблица навыков
        /// </summary>
        public DbSet<Skill> Skills { get; set; }

        /// <summary>
        /// Конструктор создания контекста данных
        /// </summary>
        /// <param name="options">Опции подключения к БД</param>
        public PersonContext(DbContextOptions<PersonContext> options) : base(options) { }

        /// <summary>
        /// Метод для задания конфикурации связей моделей
        /// </summary>
        /// <param name="modelBuilder">Построитель, который определяет модель для создаваемого контекста.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasOne(p => p.Person)
                .WithMany(b => b.Skills);
        }

    }
}
