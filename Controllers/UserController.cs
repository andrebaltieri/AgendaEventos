using Agenda.Data;
using Agenda.Models;
using Agenda.Services;
using AgendaEventos.Repositories;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UserRepository _userRepository;

        public UserController(DataContext context, UserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
        {
            var user = await _userRepository.Authenticate(model.Email, model.Password);

            if (user is null)
                return NotFound(new { message = "Email e/ou senha inválido(s)." });


            var token = TokenService.GenerateToken(user);

            user.Password = string.Empty;
            return new
            {
                user = new 
                { 
                    name = user.Name,
                    email = user.Email 
                },
                token = token
            };
        }


        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> CreateUserAsync([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var roles = new List<Role>();
                foreach (var role in model.Roles)
                {
                    var roleSearch = await _context.Roles.FindAsync(role.Id);
                    if (roleSearch is null)
                    {
                        return BadRequest(new { message = "Tipo de usuário informado é inválido." });
                    }

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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> DeleteUserAsync(int id)
        {
            var user = await _context.Users.Include(r => r.Roles).FirstOrDefaultAsync(s => s.Id == id);
            if (user is null)
            {
                return NotFound();
            }

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

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.Include(r => r.Roles).FirstOrDefaultAsync(w => w.Id == id);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context
                .Users
                .Include(r => r.Roles)
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> UpdateUserAsync(int id, [FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.Include(r => r.Roles).FirstOrDefaultAsync(w => w.Id == id);
            if (user is null)
            {
                return NotFound();
            }

            _context.Entry(user).CurrentValues.SetValues(model);

            user.Roles.Clear();
            foreach (var role in model.Roles)
            {
                var roleSearch = await _context.Roles.FindAsync(role.Id);
                if (roleSearch is null)
                {
                    return BadRequest(new { message = "Tipo de usuário informado é inválido." });
                }

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
    }
}