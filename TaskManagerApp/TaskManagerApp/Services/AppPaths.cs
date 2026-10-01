namespace TaskManagerApp.Services;

public static class AppPaths
{
    public static string DataDirectory()
    {
        var configured = Environment.GetEnvironmentVariable("VIOLET_PULSAR_DATA_DIR");
        if (!string.IsNullOrWhiteSpace(configured)) return configured;
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (OperatingSystem.IsWindows()) return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Violet Pulsar");
        if (OperatingSystem.IsMacOS()) return Path.Combine(home, "Library", "Application Support", "Violet Pulsar");
        var xdg = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
        return Path.Combine(string.IsNullOrWhiteSpace(xdg) ? Path.Combine(home, ".local", "share") : xdg, "violet-pulsar");
    }
}
