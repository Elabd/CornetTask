﻿using System;

namespace TodoList.Web.Models.TodoViewModel
{
    public class DeleteViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
    }
}
