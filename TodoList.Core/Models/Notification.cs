using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        [Required]
        public TodoItem TodoItem { get; set; }

        //public Notification()
        //{

        //}
        //private Notification(TodoItem todoItem)
        //{
        //    TodoItem = todoItem ?? throw new ArgumentNullException("todoItem");
        //    DateTime = DateTime.Now;
        //}
    }
}