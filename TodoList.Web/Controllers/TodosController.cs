using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using TodoList.Core.Interfaces;
using TodoList.Core.Models;
using TodoList.Web.Models.TodoViewModel;

namespace TodoList.Web.Controllers
{
    public class ToDosController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDosController(ITodoItemService todoItemService,
            UserManager<ApplicationUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Home()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var recentlyAddedTodos = await _todoItemService.GetRecentlyAddedItemsAsync(currentUser);
            var dueTo2daysTodos = await _todoItemService.GetDueTo2DaysItems(currentUser);

            var homeViewModel = new HomeViewModel()
            {
                RecentlyAddedTodos = recentlyAddedTodos,
                CloseDueToTodos = dueTo2daysTodos
            };

            return View(homeViewModel);
        }


        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();
            var todos = await _todoItemService.GetIncompleteItemsAsync(currentUser);
            var dones = await _todoItemService.GetCompleteItemsAsync(currentUser);

            var model = new TodoViewModel()
            {
                Todos = todos,
                Dones = dones
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var successful = await _todoItemService
                .UpdateDoneAsync(id, currentUser);

            if (!successful)
            {
                return BadRequest("Could not update todo.");
            }

            return RedirectToAction("Index");
        }

        public ViewResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,DuetoDateTime,Tags")]TodoItemCreateViewModel todo)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var todoItem = new TodoItem
            {
                Title = todo.Title,
                Content = todo.Content,
                DueToDateTime = todo.DuetoDateTime,
                ImagePath = todo.ImagePath
            };
            var successful = await _todoItemService
                .AddItemAsync(todoItem, currentUser);

            if (!successful)
            {
                return BadRequest(new { error = "Could not add item." });
            }


            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var todo = await _todoItemService
                .GetItemAsync(id);
            if (todo == null) return NotFound();

            var editViewModel = new EditViewModel()
            {
                Id = todo.Id,
                Title = todo.Title,
                Content = todo.Content,
                ImagePath = todo.ImagePath

            };
            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditViewModel todo)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var successful = await _todoItemService.UpdateTodoAsync(
                new TodoItem()
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Content = todo.Content,
                    ImagePath = todo.ImagePath
                }, currentUser);

            if (!successful)
            {
                return BadRequest("Could not update todo.");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var todo = await _todoItemService
                .GetItemAsync(id);
            if (todo == null) return NotFound();

            ViewData["FileName"] = todo.ImagePath;
            return View(todo);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty) return NotFound();

            var todo = await _todoItemService.GetItemAsync(id);

            if (todo == null) return NotFound();

            var deleteViewModel = new DeleteViewModel()
            {
                Id = todo.Id,
                Title = todo.Title,
                FilePath = todo.ImagePath
            };
            ViewData["FileName"] = Path.GetFileName(todo.ImagePath);
            return View(deleteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, string filePath = "")
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            bool successful;
            successful = await _todoItemService
                    .DeleteTodoAsync(id, currentUser);

            if (!successful)
                return BadRequest(new { error = "Couldn't delete item!" });

            return RedirectToAction("Index");

        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
