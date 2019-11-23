using System;

namespace TodoList.Web.Models.TodoViewModel
{
    public class EditViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
    }
}