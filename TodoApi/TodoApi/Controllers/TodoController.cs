using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if(_context.ToDoItems.Count() == 0)
            {
                //Create a new TodoItem if none exists
                _context.ToDoItems.Add(new TodoItem { Name = "Item1" });
                _context.ToDoItems.Add(new TodoItem { Name = "Item2" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.ToDoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(int Id)
        {
           var todoItem =  _context.ToDoItems.Where(t => t.Id == Id).FirstOrDefault();

            if(todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _context.ToDoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { Id = item.Id }, item);
        }
    }
}
