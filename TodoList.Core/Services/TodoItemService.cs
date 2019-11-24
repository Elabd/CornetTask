using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Core.Contexts;
using TodoList.Core.Interfaces;
using TodoList.Core.Models;

namespace TodoList.Core.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;
        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddItemAsync(TodoItem todo, ApplicationUser user)
        {
            todo.Id = Guid.NewGuid();
            todo.Done = false;
            todo.UserId = user.Id;
            todo.File = new FileInfo
            {
                TodoId = todo.Id,
                Path = "",
                Size = 0
            }; _context.ToDos.Add(todo);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? todo.Id.ToString() : "";
        }

        public async Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(ApplicationUser user)
        {
            return await _context.ToDos
                .Where(t => !t.Done && t.UserId == user.Id)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetCompleteItemsAsync(ApplicationUser user)
        {
            return await _context.ToDos
                .Where(t => t.Done && t.UserId == user.Id)
                .ToArrayAsync();
        }

        public bool Exists(Guid id)
        {
            return _context.ToDos
                .Any(t => t.Id == id);
        }

        public async Task<bool> UpdateDoneAsync(Guid id, ApplicationUser user)
        {
            var todo = await _context.ToDos
                .Where(t => t.Id == id && t.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (todo == null) return false;

            todo.Done = !todo.Done;

            var saved = await _context.SaveChangesAsync();
            return saved == 1;
        }

        public async Task<bool> UpdateTodoAsync(TodoItem editedTodo, ApplicationUser user)
        {
            var todo = await _context.ToDos
                .Where(t => t.Id == editedTodo.Id && t.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (todo == null) return false;

            todo.Title = editedTodo.Title;
            todo.Content = editedTodo.Content;

            var saved = await _context.SaveChangesAsync();
            return saved == 1;
        }

        public async Task<TodoItem> GetItemAsync(Guid id)
        {
            return await _context.ToDos
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> DeleteTodoAsync(Guid id, ApplicationUser currentUser)
        {
            var todo = await _context.ToDos
                .Where(t => t.Id == id && t.UserId == currentUser.Id)
                .SingleOrDefaultAsync();

            _context.ToDos.Remove(todo);

            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<IEnumerable<TodoItem>> GetRecentlyAddedItemsAsync(ApplicationUser currentUser)
        {
            return await _context.ToDos
                .Where(t => t.UserId == currentUser.Id && !t.Done
                && DateTime.Compare(DateTime.Now.AddDays(-1), t.AddedDateTime) <= 0)
                .ToArrayAsync();
        }
        public async Task<IEnumerable<TodoItem>> GetDueTo2DaysItems(ApplicationUser user)
        {
            return await _context.ToDos
                .Where(t => t.UserId == user.Id && !t.Done
                && DateTime.Compare(DateTime.Now.AddDays(1), t.DueToDateTime) >= 0)
                .ToArrayAsync();
        }

        public async Task<bool> SaveFileAsync(Guid todoId, ApplicationUser currentUser, string path, long size)
        {
            var todo = await _context.ToDos.Include(t => t.File)
                .Where(t => t.Id == todoId && t.UserId == currentUser.Id)
                .SingleOrDefaultAsync();

            if (todo == null) return false;

            todo.File.Path = path;
            todo.File.Size = size;
            todo.File.TodoId = todo.Id;

            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> MarkAsReadAsync(ApplicationUser currentUser)
        {
            var userId = currentUser.Id;
            var notifications = _context.UserNotifications
                .Where(un => un.User.Id == userId && !un.IsRead).ToList();


            notifications.ForEach(n => n.Read());

            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<IEnumerable<UserNotification>> GetNewNotificationsAsync(ApplicationUser currentUser)
        {

            var userId = currentUser.Id;
            return await _context.UserNotifications.Include(un => un.Notification)
                .Where(un => un.User.Id == userId && !un.IsRead).ToArrayAsync();
        }

        public async Task<bool> AddNotification(ApplicationUser currentUser, TodoItem todoItem)
        {
            var flag = _context.UserNotifications.Any(un => un.Notification.TodoItem.Id == todoItem.Id);
            if (!flag)
            {
                await _context.UserNotifications.AddAsync(new UserNotification
                {
                    Notification = new Notification { TodoItem = todoItem },
                    User = currentUser
                });
            }
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
    }
}
