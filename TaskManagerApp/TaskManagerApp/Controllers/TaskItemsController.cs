using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp.Controllers;

[Authorize]
public sealed class TaskItemsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppText _text;

    public TaskItemsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, AppText text)
    {
        _context = context;
        _userManager = userManager;
        _text = text;
    }

    public async Task<IActionResult> Index(string filter = "all", string search = "", string sort = "newest")
    {
        var userId = CurrentUserId();
        var all = _context.TaskItems.AsNoTracking().Where(task => task.UserId == userId);
        var today = DateTime.UtcNow.Date;
        var total = await all.CountAsync();
        var completed = await all.CountAsync(task => task.IsCompleted);
        var overdue = await all.CountAsync(task => !task.IsCompleted && task.DueDate != null && task.DueDate < today);

        IQueryable<TaskItem> query = all;
        query = filter.ToLowerInvariant() switch
        {
            "open" => query.Where(task => !task.IsCompleted),
            "completed" => query.Where(task => task.IsCompleted),
            "overdue" => query.Where(task => !task.IsCompleted && task.DueDate != null && task.DueDate < today),
            _ => query
        };
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(task => task.Title.Contains(term) || (task.Description != null && task.Description.Contains(term)));
        }
        query = sort.ToLowerInvariant() switch
        {
            "due" => query.OrderBy(task => task.DueDate == null).ThenBy(task => task.DueDate).ThenByDescending(task => task.Priority),
            "priority" => query.OrderByDescending(task => task.Priority).ThenBy(task => task.DueDate),
            _ => query.OrderBy(task => task.IsCompleted).ThenByDescending(task => task.CreatedAt)
        };

        return View(new TaskDashboardViewModel
        {
            Tasks = await query.ToListAsync(), Total = total, Open = total - completed, Completed = completed,
            Overdue = overdue, Filter = filter, Search = search, Sort = sort
        });
    }

    public async Task<IActionResult> Details(int id)
    {
        var task = await OwnedTask(id, true);
        return task is null ? NotFound() : View(task);
    }

    public IActionResult Create() => View(new TaskItem { Priority = TaskPriority.Normal });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,Priority,DueDate")] TaskItem task)
    {
        ModelState.Remove(nameof(TaskItem.UserId));
        if (string.IsNullOrWhiteSpace(task.Title)) ModelState.AddModelError(nameof(TaskItem.Title), _text["RequiredTitle"]);
        if (!ModelState.IsValid) return View(task);
        task.Title = task.Title.Trim();
        task.Description = task.Description?.Trim();
        task.UserId = CurrentUserId();
        task.CreatedAt = DateTime.UtcNow;
        _context.Add(task);
        await _context.SaveChangesAsync();
        Notify("success", _text["TaskCreated"]);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var task = await OwnedTask(id, true);
        return task is null ? NotFound() : View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,Priority,DueDate")] TaskItem input)
    {
        if (id != input.Id) return NotFound();
        var task = await OwnedTask(id);
        if (task is null) return NotFound();
        ModelState.Remove(nameof(TaskItem.UserId));
        if (string.IsNullOrWhiteSpace(input.Title)) ModelState.AddModelError(nameof(TaskItem.Title), _text["RequiredTitle"]);
        if (!ModelState.IsValid) return View(input);
        task.Title = input.Title.Trim();
        task.Description = input.Description?.Trim();
        task.IsCompleted = input.IsCompleted;
        task.Priority = input.Priority;
        task.DueDate = input.DueDate;
        await _context.SaveChangesAsync();
        Notify("success", _text["TaskUpdated"]);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int id, string? returnUrl)
    {
        var task = await OwnedTask(id);
        if (task is null) return NotFound();
        task.IsCompleted = !task.IsCompleted;
        await _context.SaveChangesAsync();
        Notify("success", _text[task.IsCompleted ? "TaskCompleted" : "TaskReopened"]);
        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : Url.Action(nameof(Index))!);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var task = await OwnedTask(id, true);
        return task is null ? NotFound() : View(task);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await OwnedTask(id);
        if (task is null) return NotFound();
        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();
        Notify("success", _text["TaskDeleted"]);
        return RedirectToAction(nameof(Index));
    }

    private string CurrentUserId() => _userManager.GetUserId(User) ?? throw new InvalidOperationException("Authenticated user has no identifier.");
    private Task<TaskItem?> OwnedTask(int id, bool noTracking = false)
    {
        IQueryable<TaskItem> query = _context.TaskItems;
        if (noTracking) query = query.AsNoTracking();
        var userId = CurrentUserId();
        return query.FirstOrDefaultAsync(task => task.Id == id && task.UserId == userId);
    }
    private void Notify(string kind, string message) => TempData["NotificationMessage"] = $"{kind}:{message}";
}
