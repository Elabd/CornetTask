using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Core.Interfaces;
using TodoList.Core.Models;
using TodoList.Web.Dtos;

namespace TodoList.Web.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationsController(ITodoItemService todoItemService, UserManager<ApplicationUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }
        public async Task<IEnumerable<NotificationDto>> GetNewNotifications()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var allIncompleteItems = await _todoItemService.GetIncompleteItemsAsync(currentUser);
            foreach (var item in allIncompleteItems)
            {
                if (item.DueToDateTime < DateTime.Now)
                {
                    await _todoItemService.AddNotification(currentUser, item);
                }
            }
            var userNotifications = await _todoItemService.GetNewNotificationsAsync(currentUser);
            List<NotificationDto> notificationDtos = new List<NotificationDto>();
            foreach (var item in userNotifications)
            {
                notificationDtos.Add(new NotificationDto
                {
                    DateTime = DateTime.Now,
                    OriginalDateTime = item.Notification.TodoItem.DueToDateTime,
                    Title = item.Notification.TodoItem.Title
                });
            }
            return notificationDtos;
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsReadAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            await _todoItemService.MarkAsReadAsync(currentUser);
            return Ok();
        }


    }
}