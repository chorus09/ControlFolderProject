

namespace ControlFolderProject; 
public class Program {
    private static async Task Main() {
        string folderPath = @"files";
        string logFilePath = @"log.txt";

        var observer = new FolderObserver(folderPath, logFilePath);
        var commandHandler = new CommandHandler(observer);

        // Запускаем наблюдение за папкой в отдельном потоке
        Task observerTask = Task.Run(() => observer.StartObservingAsync());

        while (true) {
            Console.WriteLine("Available commands: commit, info <filename>, status");
            Console.Write("Enter command: ");
            string input = Console.ReadLine();
            commandHandler.HandleCommand(input);
        }
    }
}
