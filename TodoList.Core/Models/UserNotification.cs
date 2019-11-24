using System;

namespace TodoList.Core.Models
{
    public class UserNotification
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }

        public Notification Notification { get; set; }
        public bool IsRead { get; private set; }

        public UserNotification()
        {

        }
        public UserNotification(ApplicationUser user, Notification notification)
        {
            User = user ?? throw new ArgumentNullException("user");
            Notification = notification ?? throw new ArgumentNullException("notification");
        }

        public void Read()
        {
            IsRead = true;
        }
    }
}

