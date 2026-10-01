namespace TaskManagerApp.Models;

public sealed class TaskDashboardViewModel
{
    public required IReadOnlyList<TaskItem> Tasks { get; init; }
    public int Total { get; init; }
    public int Open { get; init; }
    public int Completed { get; init; }
    public int Overdue { get; init; }
    public string Filter { get; init; } = "all";
    public string Search { get; init; } = "";
    public string Sort { get; init; } = "newest";
}
