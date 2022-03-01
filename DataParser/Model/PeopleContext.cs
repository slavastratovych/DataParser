using Microsoft.EntityFrameworkCore;

namespace DataParser.Model
{
    internal class PeopleContext : DbContext
    {
        public PeopleContext(DbContextOptions<PeopleContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}
