
using ControlFolderProject.Files;

namespace ControlFolderProject;
public class FolderObserver {
    private readonly string _folderPath;
    private FileSystemWatcher? _watcher;
    private readonly Dictionary<string, IFile> _files = new Dictionary<string, IFile>();

    public event EventHandler<string>? FileChanged;
    public event EventHandler<string>? FileCreated;
    public event EventHandler<string>? FileDeleted;
    public event EventHandler<(string OldName, string NewName)>? FileRenamed;

    public FolderObserver(string folderPath) {
        _folderPath = folderPath;
    }

    public async Task StartObservingAsync(CancellationToken cancellationToken = default) {
        _watcher = new FileSystemWatcher();
        _watcher.Path = _folderPath;
        _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

        _watcher.Changed += (sender, e) => OnFileChanged(e.FullPath);
        _watcher.Created += (sender, e) => OnFileCreated(e.FullPath);
        _watcher.Deleted += (sender, e) => OnFileDeleted(e.FullPath);
        _watcher.Renamed += (sender, e) => OnFileRenamed(e.OldFullPath, e.FullPath);

        _watcher.EnableRaisingEvents = true;

        // Обработка существующих файлов в папке
        ProcessExistingFiles();

        await Task.Delay(-1, cancellationToken);
    }

    public void StopObserving() {
        _watcher!.EnableRaisingEvents = false;
        _watcher.Dispose();
    }

    private void ProcessExistingFiles() {
        var files = Directory.GetFiles(_folderPath);
        foreach (var file in files) {
            AddFile(file);
        }
    }

    private void AddFile(string fullPath) {
        IFile file = CreateFile(fullPath);
        if (file != null) {
            _files[fullPath] = file;
        }
    }

    private IFile CreateFile(string fullPath) {
        if (IsTextFile(fullPath)) {
            return new TextFile(fullPath);
        } else if (IsImageFile(fullPath)) {
            return new ImageFile(fullPath);
        } else if (IsPythonFile(fullPath)) {
            return new PythonFile(fullPath);
        } else if (IsJavaFile(fullPath)) {
            return new JavaFile(fullPath);
        } else {
            // Если неизвестный тип файла, возвращаем null
            return null;
        }
    }

    private bool IsTextFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".txt" || extension == ".log"; // Пример расширений текстовых файлов
    }

    private bool IsImageFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".jpg" || extension == ".png" || extension == ".bmp"; // Пример расширений изображений
    }

    private bool IsPythonFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".py"; // Расширение для файлов Python
    }

    private bool IsJavaFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".java"; // Расширение для файлов Java
    }

    private void OnFileChanged(string fullPath, WatcherChangeTypes changeType) {
        if (changeType != WatcherChangeTypes.Renamed) {
            FileChanged?.Invoke(this, fullPath);
        }
    }

    private void OnFileCreated(string fullPath) {
        AddFile(fullPath);
        FileCreated?.Invoke(this, fullPath);
    }

    private void OnFileDeleted(string fullPath) {
        _files.Remove(fullPath);
        FileDeleted?.Invoke(this, fullPath);
    }

    private void OnFileRenamed(string oldFullPath, string newFullPath) {
        if (_files.ContainsKey(oldFullPath)) {
            _files.Remove(oldFullPath);
            AddFile(newFullPath);
        }
        FileRenamed?.Invoke(this, (oldFullPath, newFullPath));
    }
}

