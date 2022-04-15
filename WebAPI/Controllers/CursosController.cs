using System.Security.Cryptography;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly IMediator mediator;

        public CursosController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        //http://localhost:5132/api/Cursos
        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Get()
        {
            return await mediator.Send(new Consulta.ListaCursos());
        }

        //http://localhost:5132/api/Cursos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> Detalle(int id)
        {
            return await mediator.Send(new ConsultaId.CursoUnico { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>>
        Editar(int id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            return await mediator.Send(data);
        }
    }
}
