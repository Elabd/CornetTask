using System.Collections.Generic;
using TodoList.Core.Models;

namespace TodoList.Web.Models.TodoViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<TodoItem> RecentlyAddedTodos { get; set; }
        public IEnumerable<TodoItem> CloseDueToTodos { get; set; }
    }
}
