using System.Data;
using System.Net;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persitencia;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;

            private readonly SignInManager<Usuario> _signInManager;

            private readonly CursosOnlineContext _context;

            public Manejador(
                UserManager<Usuario> userManager,
                SignInManager<Usuario> signInManager,
                CursosOnlineContext context
            )
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _context = context;
            }

            public async Task<UsuarioData>
            Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario =
                    await _userManager.FindByEmailAsync(request.Email);

                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var resultado =
                    await _signInManager
                        .CheckPasswordSignInAsync(usuario,
                        request.Password,
                        false);
                if (resultado.Succeeded)
                {
                    return new UsuarioData {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = "Aqui ira la data del token",
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Imagen = null
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}
