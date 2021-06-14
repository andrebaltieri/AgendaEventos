using Agenda.Data;
using Agenda.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DataContext _context;

        public RolesController(DataContext context)
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

                return CreatedAtAction(nameof(CreateAsync), new { id = model.Id }, model);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRolesAsync() => Ok(await _context.Roles.AsNoTracking().ToListAsync());

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
    }
}