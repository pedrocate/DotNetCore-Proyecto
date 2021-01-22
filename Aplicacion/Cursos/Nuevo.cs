using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest {
            public string Titulo { get; set ;}
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
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
            private readonly CursosOnlineContext _context;
            public Handler(CursosOnlineContext context) {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Guid _cursoId = Guid.NewGuid();
                var curso = new Curso {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                _context.Curso.Add(curso);

                if (request.ListaInstructor != null) {
                    foreach (var id in request.ListaInstructor) {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = _cursoId,
                            InstructorId = id
                        };

                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                var valor = await _context.SaveChangesAsync(); //Devuelve el número de operaciones que se realian sobre la base de datos

                if (valor > 0) {
                    return Unit.Value; 
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}