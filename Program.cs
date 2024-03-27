

namespace ControlFolderProject; 
public class Program {
    private static async Task Main() {
        string folderPath = @"files";

        var observer = new FolderObserver(folderPath);
        observer.FileChanged += (sender, fullPath) => Console.WriteLine($"File {fullPath} changed");
        observer.FileCreated += (sender, fullPath) => Console.WriteLine($"File {fullPath} created");
        observer.FileDeleted += (sender, fullPath) => Console.WriteLine($"File {fullPath} deleted");
        observer.FileRenamed += (sender, names) => Console.WriteLine($"File renamed from {names.OldName} to {names.NewName}");

        var cancellationTokenSource = new CancellationTokenSource();

        // Запуск наблюдения в асинхронном режиме
        await observer.StartObservingAsync(cancellationTokenSource.Token);

        // Некоторый код здесь...

        // Остановка наблюдения
        observer.StopObserving();
    }
}
