using Agenda.Data;
using Agenda.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/v1/users")]
public class UserController : ControllerBase
{
    //Criar usuario
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<User>> Post(
        [FromBody] User user, [FromServices] DataContext context)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!user.Roles.Any())
        {
            return BadRequest("Informe um perfil de usuário");
        }

        try
        {
            var roles = new List<Role>();
            foreach (var role in user.Roles)
            {
                var hydratedRole = await context.Roles.FindAsync(role.Id);
                if (hydratedRole is null)
                {
                    return BadRequest("Perfil de usuário não existe");
                }

                roles.Add(hydratedRole);
            }

            user.Roles = roles;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return CreatedAtRoute(new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    //Consultar lista de usuarios
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<User>>> Get(
        [FromServices] DataContext context)
    {
        var users = await context.Users.AsNoTracking().ToListAsync();
        return Ok(users);
    }

    //Consultar usuario especifico por id
    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<User>> GetById(
        int id, [FromServices] DataContext context)
    {
        var user = await context.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        return Ok(user);
    }

    //Alterar usuario
    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<User>> Put(int id, [FromBody] User user, [FromServices] DataContext context)
    {
        if (user.Id != id) return NotFound(new { message = "Usuario nao encontrado" });

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            context.Entry<User>(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return user;
        }
        catch
        {
            return BadRequest(new { message = "Nao foi possivel alterar os dados do usuario." });
        }
    }

    //Excluir usuario
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<User>> Delete(
        int id, [FromServices] DataContext context)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return NotFound(new { message = "Usuario nao encontrado." });

        try
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return Ok(user);
        }
        catch
        {
            return BadRequest(new { message = "Nao foi possivel excluir o usuario." });
        }
    }
}