using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Controllers
{
    [Authorize]
    public class TaskItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TaskItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /TaskItems
        public async Task<IActionResult> Index()
        {

            var userId = GetCurrentUserId();
            var tasks = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            return View(tasks);
        }

        // GET: /TaskItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetCurrentUserId();
            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // GET: /TaskItems/Create
        public IActionResult Create() => View();

        // POST: /TaskItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] TaskItem taskItem)
        {
            Console.WriteLine(">>> Create POST invoked");
            // The user does not provide the UserId; we get it from the current session.
            // Therefore, we remove it from the ModelState before validation.
            ModelState.Remove(nameof(taskItem.UserId));

            var userId = GetCurrentUserId();
            if (userId == null)
            {
                // This handles the CS8601 warning and makes the code more robust
                return Unauthorized("Impossible de déterminer l'utilisateur.");
            }



            if (ModelState.IsValid)
                {
                    taskItem.UserId = userId;
                    taskItem.CreatedAt = DateTime.UtcNow;
                    taskItem.IsCompleted = false;

                    _context.Add(taskItem);
                    await _context.SaveChangesAsync();
                    TempData["NotificationMessage"] = "success:Tâche créée avec succès.";
                    return RedirectToAction(nameof(Index));
                }
            return View(taskItem);
        }

        // GET: /TaskItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetCurrentUserId();
            var taskItem = await _context.TaskItems
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // POST: /TaskItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted")] TaskItem taskItemFromForm)
        {
            if (id != taskItemFromForm.Id) return NotFound();

            var userId = GetCurrentUserId();

            if (userId == null) return Challenge();

            // To prevent overwriting properties like CreatedAt, fetch the original task
            // from the database first. This is more efficient and safer
            var taskToUpdate = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (taskToUpdate == null) 
            {
                // The task doesn't exist or doesn't belong to the current user.
                return NotFound();
            }

            // We are not binding UserId or CreatedAt from the form, so remove them from validation
            // to prevent ModelState errors on these required properties.
            ModelState.Remove(nameof(TaskItem.UserId));
            ModelState.Remove(nameof(TaskItem.CreatedAt));

            if (ModelState.IsValid)
                {
                    // Update the properties of the tracked entity with the values from the form.
                    // This ensures properties not in the form (like CreatedAt) are not overwritten.
                    taskToUpdate.Title = taskItemFromForm.Title;
                    taskToUpdate.Description = taskItemFromForm.Description;
                    taskToUpdate.IsCompleted = taskItemFromForm.IsCompleted;

                    await _context.SaveChangesAsync();
                    TempData["NotificationMessage"] = "success:Tâche mise à jour avec succès.";
                    return RedirectToAction(nameof(Index));
                }
            return View(taskItemFromForm);
        }

        // GET: /TaskItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = GetCurrentUserId();
            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // POST: /TaskItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();
            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
                TempData["NotificationMessage"] = "success:Tâche supprimée avec succès.";
            }
            return RedirectToAction(nameof(Index));
        }

        private string? GetCurrentUserId() => _userManager.GetUserId(User);
    }

}