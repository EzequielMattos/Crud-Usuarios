using CrudUsuario.Application.Services;
using CrudUsuario.Data;
using CrudUsuario.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrudUsuario.Ioc
{
    public static class InjectorConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<Context>()
            .AddDefaultTokenProviders();

            services.AddScoped<IUsuarioService, UsuarioService>();
        }
    }
}
