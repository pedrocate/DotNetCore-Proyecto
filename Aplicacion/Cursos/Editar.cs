using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest {
            public int CursoId { get; set; }
            public string Titulo { get; set ;}
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta> {
            public EjecutaValidacion() {
                RuleFor(obj => obj.Titulo).NotEmpty();
                RuleFor(obj => obj.Descripcion).NotEmpty();
                RuleFor(obj => obj.FechaPublicacion).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            CursosOnlineContext _context;
            public Handler(CursosOnlineContext context) {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.CursoId);

                if (curso == null) {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new { curso = "No se encontro el curso"});
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                var resultado = await _context.SaveChangesAsync();

                if (resultado > 0) {
                    return Unit.Value;
                }

                throw new Exception("No se guardaron los cambios en el curso");
            }
        }
    }
}