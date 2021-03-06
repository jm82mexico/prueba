using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.Cursos;
using Dominio;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Persitencia;
using Seguridad;
using WebAPI.Middleware;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Configura la conexión a la base de datos
            services
                .AddDbContext<CursosOnlineContext>(opt =>
                {
                    opt
                        .UseSqlServer(Configuration
                            .GetConnectionString("DefaultConnection"));
                });

            //Configurar la inyección de dependencias
            services.AddMediatR(typeof (Consulta.Manejador).Assembly);
            services
                .AddControllers(opt =>
                {
                    var policy =
                        new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                    opt.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddFluentValidation(cfg =>
                    cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            //CONFIGURACIÓN PARA UTILIZAR IDENTITY
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder =
                new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();

            //IMPLEMENTAR LA SEGURIDAD CON JWT
            var key =
                new SymmetricSecurityKey(Encoding
                        .UTF8
                        .GetBytes("PeluchaCachoriux"));
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters =
                        new TokenValidationParameters {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateAudience = false, //Incluir las ips validas
                            ValidateIssuer = false
                        };
                });

            //IMPLENTAR LA INTERFAZ PARA LA GENERAR LOS TOKENS
            services.AddScoped<IJwtGenerador, JwtGenerador>();

            //RECUPERAR LA INFORMACIÓN DEL USUARIO
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
