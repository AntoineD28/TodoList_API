using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TodoAPI.Models;
using TodoAPI.Models.Dto;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TodoListContext _context;
        // Utilisation du password hashing de Identity
        private PasswordHasher<User> _passwordHasher;

        public UsersController(TodoListContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<object> GetUsers()
        {
            // on ajoute à chaque utlisateur sa liste de tâches
            return await _context.Users.Select(u => new
            {
                u.Id,
                u.Username,
                u.Password,
                u.Email,
                u.CreateTime,
                TodoItems = u.
                    TodoItems
                    .Select(t => new { t.Title, t.Description, t.IsCompleted })
                    .ToList()
            }).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
           
            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserRegisterDto user)
        {
            User _user = new(user.Username, user.Email, user.Password);

            // On entre le hash du mdp dans la bdd
            _user.Password = _passwordHasher.HashPassword(_user, user.Password);
            _context.Users.Add(_user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = _user.Id }, _user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
