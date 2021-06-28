using Agenda.Data;
using Agenda.Models;
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

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> Delete(int id)
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (events == null)
            {
                return NotFound(new { message = "Evento não encontrado" });
            }

            try
            {
                _context.Remove(events);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível remover o evento" });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Event>> Get()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .AsNoTracking()
                .ToListAsync();
            return events;
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Event> GetById(int id)
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return events;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> Post([FromBody] Event model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            try
            {
                model.StartDate = model.StartDate.ToUniversalTime();
                model.EnrollmentDeadlineDate = model.EnrollmentDeadlineDate.ToUniversalTime();
                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o evento" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Event>> Update(int id, [FromBody] Event model)
        {
            if (id != model.Id)
                return NotFound(new { message = "Evento não encontrado" });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                model.StartDate = model.StartDate.ToUniversalTime();
                model.EnrollmentDeadlineDate = model.EnrollmentDeadlineDate.ToUniversalTime();
                _context.Entry<Event>(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Não foi possível atualizar o evento" });
            }
        }
    }
}