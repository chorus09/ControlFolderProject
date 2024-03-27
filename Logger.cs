
namespace ControlFolderProject; 
public class Logger {
    private readonly string _logFilePath;

    public Logger(string logFilePath) {
        _logFilePath = logFilePath;
    }

    public void Log(string message) {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";
        File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
    }
}
