using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Agenda.Models;
using AgendaEventos.Models;

namespace AgendaEventos.Services
{
    public static class TokenService
    {

        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name.ToString()),
                    new Claim(ClaimTypes.Role, user.Roles.FirstOrDefault().Title.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }        
    }
}

// [HttpPost("login/{user}/{password}")]
//         public ActionResult<dynamic> LoAuthenticategin(string user, string password)
//         {
//             const string _user = "joao.silva";
//             const string _password = "123123";
//             if (!user.Equals(_user) || !password.Equals(_password))
//                 return NotFound(new {message = "Senha e/ou Usuário inválido(s)."});

//             var userToken = new User()
//             {
//                 Id = 1,
//                 Email = "joao.silva@gmail.com",
//                 Name = "Joao Silva",
//                 Roles = new List<Role>()
//                 {
//                     new Role(){Id = 1, Description = "Admin", Title = "Administrador do Sistema.F"},
//                 }
//             };

//             var token = TokenService.GenerateToken(userToken);
//             return new {
//                 user = user,
//                 token = token
//             };
//         }
