using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Dominio;
using Aplicacion.Cursos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Get() {
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetBydId(int id) {
            return await Mediator.Send(new ConsultaId.CursoUnico{ Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Nuevo.Ejecuta data) {
            return await Mediator.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(int id, Editar.Ejecuta data) {
            data.CursoId = id;
            return await Mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(int id) {
            return await Mediator.Send(new Eliminar.Ejecuta{ Id = id });
        }
    }
}