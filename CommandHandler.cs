
namespace ControlFolderProject; 
public class CommandHandler {
    private readonly FolderObserver _folderObserver;

    public CommandHandler(FolderObserver folderObserver) {
        _folderObserver = folderObserver;
    }

    public void HandleCommand(string command) {
        string[] parts = command.Split(' ');
        string action = parts[0];

        switch (action) {
            case "commit":
                _folderObserver.Commit();
                Console.WriteLine("Commit successful");
                break;
            case "info":
                if (parts.Length > 1) {
                    string fileName = parts[1];
                    string fileInfo = _folderObserver.GetFileInfo(fileName);
                    Console.WriteLine(fileInfo);
                } else {
                    Console.WriteLine("Filename is missing");
                }
                break;
            case "status":
                string status = _folderObserver.GetStatus();
                Console.WriteLine(status);
                break;
            default:
                Console.WriteLine("Unknown command");
                break;
        }
    }
}
