using Microsoft.EntityFrameworkCore;

namespace HallOfFame.Models
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasOne(p => p.Person)
                .WithMany(b => b.Skills);
        }

    }
}
