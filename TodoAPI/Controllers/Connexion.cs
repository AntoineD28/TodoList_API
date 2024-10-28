using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoAPI.Models;
using TodoAPI.Models.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Connexion : ControllerBase
    {
        private readonly TodoListContext _context;
        // Utilisation du password hashing de Identity
        private PasswordHasher<User> _passwordHasher;
        private readonly JwtOptions _JwtOptions;

        public Connexion(JwtOptions JwtOptions, TodoListContext context)
        {
            _JwtOptions = JwtOptions;
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // POST: api/<Connexion>
        //Task<ActionResult<User>>
        [HttpPost]
        public IActionResult PostConnexion(UserLoginDto u)
        {
            try
            {
                User user = _context.Users.First(i => i.Username == u.Username);

                // On vérifie le hash stocké en BDD et le hash du password fourni par l'utilisateur
                PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, u.Password);
                if (passwordVerificationResult == 0)
                {
                    return Unauthorized("Invalid credentials");
                }
                var token = GenerateAccessToken(user.Username);
                return Ok(new { user.Id, user.Username, user.Email, AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Génération d'un token basé sur les informations fournis par l'utilisateur 
        private JwtSecurityToken GenerateAccessToken(string userName)
        {
            // Create user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                // Add additional claims as needed (e.g., roles, etc.)
            };

            // Create a JWT
            var token = new JwtSecurityToken(
                issuer: _JwtOptions.Issuer,
                audience: _JwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1), // Token expiration time
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtOptions.SecretKey)),
                    SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}
