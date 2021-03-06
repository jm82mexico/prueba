using System.Security.Cryptography;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : MainControllerBase
    {
        //http://localhost:5132/api/Cursos
        [HttpGet]
        [
            Authorize(
                AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)
        ]
        public async Task<ActionResult<List<Curso>>> Get()
        {
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        //http://localhost:5132/api/Cursos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> Detalle(int id)
        {
            return await Mediator.Send(new ConsultaId.CursoUnico { Id = id });
        }

        [HttpPost]
        [
            Authorize(
                AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)
        ]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>>
        Editar(int id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(int id)
        {
            return await Mediator.Send(new Eliminar.Ejecuta { CursoId = id });
        }
    }
}
