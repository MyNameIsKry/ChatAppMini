namespace Utils;
public class Logger
{
    public static void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
    }

    public static void LogError(string message, Exception ex)
    {
        Console.WriteLine($"[{DateTime.Now}] ERROR: {message} - Exception: {ex.Message}");
    }
}