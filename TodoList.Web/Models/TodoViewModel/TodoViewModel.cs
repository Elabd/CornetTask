using System.Collections.Generic;
using TodoList.Core.Models;

namespace TodoList.Web.Models.TodoViewModel
{
    public class TodoViewModel
    {
        public IEnumerable<TodoItem> Todos { get; set; }
        public IEnumerable<TodoItem> Dones { get; set; }
    }
}
