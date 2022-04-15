using MediatR;
using Persitencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public int CursoId { get; set; }

            public string Titulo { get; set; }

            public string Descripcion { get; set; }

            public DateTime? FechaPublicacion { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext context;

            public Manejador(CursosOnlineContext _context)
            {
                context = _context;
            }

            public async Task<Unit>
            Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await context.Curso.FindAsync(request.CursoId);

                if (curso == null)
                {
                    throw new Exception("El curso no existe");
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion =
                    request.FechaPublicacion ?? curso.FechaPublicacion;

                var resultado = await context.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se guardaron los cambios en el curso");
            }
        }
    }
}
