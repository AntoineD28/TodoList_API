using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Models.Dto;

namespace TodoAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoListContext _context;

        public TodoItemsController(TodoListContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // GET: api/TodoItems/User/1
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemByUser(int userId)
        {
            var todoItems = await _context.TodoItems.Where(p => p.UserId == userId).ToListAsync();

            if (todoItems == null)
            {
                return NotFound();
            }

            return todoItems;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, TodoItemUpdateDto todoItemDto)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Title = todoItemDto.Title;
            todoItem.Description = todoItemDto.Description;
            todoItem.IsCompleted = todoItemDto.IsCompleted;
            todoItem.UpdateTime = DateTime.UtcNow; // Mise à jour de la date

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(todoItem);
        }


        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemPostDto TI)
        {
            try
            {
                // On récupère l'utilisateur avec l'ID donné
                User u = _context.Users.ElementAt(TI.UserId);

                // Si l'utilsateur n'est pas trouvé aucun tâche ne sera créée 
                if (u == null)
                {
                    return NotFound();
                }

                // On créer le TodoItem a partir du dto
                TodoItem todoItem = new()
                {
                    Title = TI.Title,
                    Description = TI.Description,
                    UserId = TI.UserId,
                    IsCompleted = false,
                };

                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            }
            catch(Exception e)
            {
                return NotFound("L'utilisateur n'a pas été retrouvé en base de données, la tâche n'est pas créée.");
            }

        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
