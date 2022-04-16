using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MainControllerBase
    {
        // http://localhost:5132/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>>
        Login(Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        // http://localhost:5132/api/Usuario/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>>
        Registrar(Registrar.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }

        [HttpGet]
        [
            Authorize(
                AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)
        ]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            return await Mediator.Send(new UsuarioActual.Ejecutar());
        }
    }
}
