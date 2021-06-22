using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agenda.Data;
using AgendaEventos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    //Criar usuario
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<User>> Post(
        [FromBody] User user,
        [FromServices] DataContext context)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var roles = await context.Roles.AsNoTracking().ToListAsync();
            if (roles.Count == 0)
                return BadRequest(new {message = "Nao foi possível incluir um novo usuário."});
            
            if (user.RoleId != 0)
            {
                var role = roles.FirstOrDefault(r => r.Id == user.RoleId);
                if (role == null)
                    return BadRequest(new {message = "Nao foi possível incluir um novo usuário."});
            }
            
            context.Users.Add(user);
            await context.SaveChangesAsync();

            /* Insert in table 'UserRoles' 
                UserId = user.Id;
                RoleId = roles.FirstOrDefault().Id;
            */
            
            return Ok(user);
        }
        catch
        {
            return BadRequest(new {message = "Nao foi possivel incluir um novo usuario."});
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
        int id,
        [FromServices] DataContext context)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync();

        return Ok(user);
    }

    //Alterar usuario
    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<User>> Put(int id,
        [FromBody] User user,
        [FromServices] DataContext context)
    {
        if (user.Id != id) return NotFound(new {message = "Usuario nao encontrado"});

        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            context.Entry<User>(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return user;
        }
        catch
        {
            return BadRequest(new {message = "Nao foi possivel alterar os dados do usuario."});
        }
    }

    //Excluir usuario
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<User>> Delete(
        int id,
        [FromServices] DataContext context)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return NotFound(new {message = "Usuario nao encontrado."});

        try
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return Ok(user);
        }
        catch
        {
            return BadRequest(new {message = "Nao foi possivel excluir o usuario."});
        }
    }
}