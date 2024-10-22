using APIDemo161024.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIDemo161024.DatabaseConnect
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().
                SetBasePath(AppContext.BaseDirectory).
                AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DatabaseContext"));
        }
        public DbSet<Book> Book { get; set; }

     

    }
}
