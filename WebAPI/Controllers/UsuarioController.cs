using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class UsuarioController : MainControllerBase
    {
        // http://localhost:5132/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>>
        Login(Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }
    }
}
