using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TodoList.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UserNotification> UserNotifications { get; set; }

    }
}
