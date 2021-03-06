using Agenda.Data;
using Agenda.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agenda.Controllers
{
    [Route("api/v1/events")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly DataContext _context;

        public EventController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Organizador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> CreateEventAsync([FromBody] Event model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await UserExists(model.SpeakerId))
                {
                    ModelState.AddModelError(nameof(model.Speaker), "Palestrante informado é inválido");
                    return BadRequest(ModelState);
                }

                if (!await UserExists(model.OrganizerId))
                {
                    ModelState.AddModelError(nameof(model.Organizer), "Organizador informado é inválido");
                    return BadRequest(ModelState);
                }

                if (!await CategoryExists(model.CategoryId))
                {
                    ModelState.AddModelError(nameof(model.Category), "Categoria informada é inválido");
                    return BadRequest(ModelState);
                }

                _context.Events.Add(model);
                await _context.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetEventByIdAsync), new { id = model.Id }, model.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador,Organizador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> DeleteEventAsync(int id)
        {
            var events = await _context.Events.FindAsync(id);
            if (events is null)
            {
                return NotFound(new { message = "Evento não encontrado" });
            }

            try
            {
                _context.Remove(events);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id:int}", Name = nameof(GetEventByIdAsync))]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Event>> GetEventByIdAsync(int id)
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (events is null)
            {
                return NotFound(new { message = "Evento não encontrado" });
            }

            return Ok(events);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsAsync()
        {
            return await _context.Events
                .Include(e => e.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrador,Organizador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> UpdateEventAsync(int id, [FromBody] Event model)
        {
            if (id != model.Id)
            {
                return BadRequest(new { message = "Id do evento é inválido." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var events = await _context.Events.FindAsync(id);
                if (events is null)
                {
                    return NotFound(new { message = "Evento não encontrado." });
                }

                if (!await UserExists(model.SpeakerId))
                {
                    ModelState.AddModelError(nameof(model.Speaker), "Palestrante informado é inválido");
                    return BadRequest(ModelState);
                }

                if (!await UserExists(model.OrganizerId))
                {
                    ModelState.AddModelError(nameof(model.Organizer), "Organizador informado é inválido");
                    return BadRequest(ModelState);
                }

                if (!await CategoryExists(model.CategoryId))
                {
                    ModelState.AddModelError(nameof(model.Category), "Categoria informada é inválida");
                    return BadRequest(ModelState);
                }

                _context.Entry(events).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private Task<bool> CategoryExists(int id) => _context.Categories.AnyAsync(c => c.Id == id);

        private Task<bool> UserExists(int id) => _context.Users.AnyAsync(u => u.Id == id);
    }
}