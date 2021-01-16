using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context) {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);

                if (curso == null) {
                    throw new ManejadorExcepcion(HttpStatusCode.InternalServerError, new { curso = "No se encontro el curso"});
                }

                _context.Remove(curso);

                var resultado = await _context.SaveChangesAsync();

                if (resultado <= 0) {
                    throw new Exception("No se puedieron guardar los cambios");
                }
                
                return Unit.Value;
            }
        }
    }
}