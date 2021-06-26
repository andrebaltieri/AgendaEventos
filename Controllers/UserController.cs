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
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUserAsync([FromBody] User model)
        {
            try
            {
                var roles = new List<Role>();
                foreach (var role in model.Roles)
                {
                    var roleSearch = await _context.Roles.SingleOrDefaultAsync(w => w.Id == role.Id);
                    if (roleSearch is null)
                        return NotFound(new { message = "Tipo de usuário informado é inválido." });

                    roles.Add(roleSearch);
                }
                model.Roles = roles;

                await _context.Users.AddAsync(model);
                await _context.SaveChangesAsync();

                return CreatedAtRoute(new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context
                            .Users
                            .Include(r => r.Roles)
                            .AsNoTracking()
                            .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.Include(r => r.Roles).SingleAsync(w => w.Id == id);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUserAsync(int id, [FromBody] User model)
        {
            var user = await _context.Users.Include(r => r.Roles).SingleOrDefaultAsync(w => w.Id == id);
            if (user is null)
                return NotFound();

            _context.Entry(user).CurrentValues.SetValues(model);

            user.Roles.Clear();
            foreach (var role in model.Roles)
            {
                var roleSearch = await _context.Roles.FindAsync(role.Id);
                user.Roles.Add(roleSearch);
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUserAsync(int id)
        {
            var user = await _context.Users.Include(r => r.Roles).SingleOrDefaultAsync(s => s.Id == id);
            if (user is null)
                return NotFound();

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}