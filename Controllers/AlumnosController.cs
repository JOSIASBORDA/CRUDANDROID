using CRUDALUMNOS.MODELS;
using CRUDALUMNOS.SERVICIO;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CRUDALUMNOS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly AlumnoService _alumnoService;

        public AlumnosController(AlumnoService alumnoService)
        {
            _alumnoService = alumnoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Alumno>>> Get()
        {
            var alumnos = await _alumnoService.GetAsync();
            return Ok(alumnos);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Alumno>> Get(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format");
            }

            var alumno = await _alumnoService.GetAsync(id);

            if (alumno is null)
            {
                return NotFound();
            }

            return Ok(alumno);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Alumno nuevoAlumno)
        {
            // No necesitas asignar el Id en la solicitud
            await _alumnoService.CreateAsync(nuevoAlumno);
            // Devuelve la respuesta con el Id generado
            return CreatedAtAction(nameof(Get), new { id = nuevoAlumno.Id.ToString() }, nuevoAlumno);
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Alumno alumnoActualizado)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format");
            }

            var alumno = await _alumnoService.GetAsync(id);

            if (alumno is null)
            {
                return NotFound();
            }

            alumnoActualizado.Id = alumno.Id;
            await _alumnoService.UpdateAsync(id, alumnoActualizado);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return BadRequest("Invalid ID format");
            }

            var alumno = await _alumnoService.GetAsync(id);

            if (alumno is null)
            {
                return NotFound();
            }

            await _alumnoService.RemoveAsync(id);
            return NoContent();
        }
    }
}
