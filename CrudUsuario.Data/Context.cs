using CrudUsuario.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrudUsuario.Data
{
    public class Context : IdentityDbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) { }
    }
}