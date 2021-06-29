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
    [Route("api/v1/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly DataContext _context;

        public RoleController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateAsync(Role model)
        {
            try
            {
                await _context.Roles.AddAsync(model);
                await _context.SaveChangesAsync();

                return CreatedAtRoute(new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                if (role is null)
                {
                    return NotFound();
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role is null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRolesAsync() => Ok(await _context.Roles.AsNoTracking().ToListAsync());

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoleAsync(int id, Role model)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                if (role is null)
                {
                    return NotFound();
                }

                role.Title = model.Title;
                role.Description = model.Description;

                _context.Entry(role).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}