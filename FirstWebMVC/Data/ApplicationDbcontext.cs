using Microsoft.EntityFrameworkCore;
using FirstWebMVC.Models;

namespace FirstWebMVC.Data{
    public class ApplicationDbContext: DbContext{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {}
        public DbSet<Person> Person {get; set;}
        public DbSet<FirstWebMVC.Models.Employee> Employee { get; set; } = default!;
        public DbSet<FirstWebMVC.Models.DaiLy> DaiLy { get; set; } = default!;

           protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Employee>().ToTable("Employee");
    modelBuilder.Entity<Person>().ToTable("Persons");
}
        
    }

 

}