using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ContainsOptimization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var context = new MyContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var types = new[]{PersonType.Happy, PersonType.Sad};
                
                var result = context.People.Where(p => types.Contains(p.Children.Any() ? PersonType.Happy : PersonType.Sad)).ToList();
                Console.WriteLine($"It worked: {result.Count}");
            }
        }
    }

    public class MyContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost,14330;Database=aika;User=sa;Password=SecretPassword1234;TrustServerCertificate=True"); //.LogTo(Console.WriteLine).EnableSensitiveDataLogging();
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PersonType Type {get; set;}
        public virtual List<Child> Children { get; private set; }
    }

    public class Child{
        public int Id {get;set;}
        public int PersonId {get;set;}
        public Person Person {get;set;}

        public string Name { get; set; }
    }

    public enum PersonType{
        Happy,
        Sad
    }

}