using System;

namespace TodoList.Web.Dtos
{
    public class NotificationDto
    {
        public DateTime DateTime { get; set; }
        public DateTime? OriginalDateTime { get; set; }
        public string Title { get; set; }

    }
}
