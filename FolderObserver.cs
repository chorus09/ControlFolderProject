
using ControlFolderProject.Files;
using System.Text;

namespace ControlFolderProject;
public class FolderObserver {
    private readonly string _folderPath;
    private FileSystemWatcher? _watcher;
    private readonly Dictionary<string, IFile> _files = new Dictionary<string, IFile>();
    private readonly Logger _logger;
    private readonly SnapshotManager _snapshotManager;

    public event EventHandler<string>? FileChanged;
    public event EventHandler<string>? FileCreated;
    public event EventHandler<string>? FileDeleted;
    public event EventHandler<(string OldName, string NewName)>? FileRenamed;

    public FolderObserver(string folderPath, string logFilePath) {
        _folderPath = folderPath;
        _logger = new Logger(logFilePath);
        _snapshotManager = new SnapshotManager();
    }

    public async Task StartObservingAsync(CancellationToken cancellationToken = default) {
        _watcher = new FileSystemWatcher();
        _watcher.Path = _folderPath;
        _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

        _watcher.Changed += (sender, e) => OnFileChanged(e.FullPath, e.ChangeType);
        _watcher.Created += (sender, e) => OnFileCreated(e.FullPath);
        _watcher.Deleted += (sender, e) => OnFileDeleted(e.FullPath);
        _watcher.Renamed += (sender, e) => OnFileRenamed(e.OldFullPath, e.FullPath);

        _watcher.EnableRaisingEvents = true;
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
            _logger.Log($"File created: {fullPath}");
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
            return null;
        }
    }

    private bool IsTextFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".txt" || extension == ".log";
    }

    private bool IsImageFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".jpg" || extension == ".png" || extension == ".bmp";
    }

    private bool IsPythonFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".py";
    }

    private bool IsJavaFile(string filePath) {
        string extension = Path.GetExtension(filePath);
        return extension == ".java";
    }

    private void OnFileChanged(string fullPath, WatcherChangeTypes changeType) {
        if (changeType != WatcherChangeTypes.Renamed) {
            FileChanged?.Invoke(this, fullPath);
            _logger.Log($"File changed: {fullPath}");
        }
    }

    private void OnFileCreated(string fullPath) {
        AddFile(fullPath);
        FileCreated?.Invoke(this, fullPath);
    }

    private void OnFileDeleted(string fullPath) {
        _files.Remove(fullPath);
        FileDeleted?.Invoke(this, fullPath);
        _logger.Log($"File deleted: {fullPath}");
    }

    private void OnFileRenamed(string oldFullPath, string newFullPath) {
        if (_files.ContainsKey(oldFullPath)) {
            _files.Remove(oldFullPath);
            AddFile(newFullPath);
            FileRenamed?.Invoke(this, (oldFullPath, newFullPath));
            _logger.Log($"File renamed: {oldFullPath} -> {newFullPath}");
        }
    }
    public void Commit() {
        _snapshotManager.TakeSnapshot();
        _logger.Log("Snapshot taken");
    }


    public string GetFileInfo(string fileName) {
        string fullPath = Path.Combine(_folderPath, fileName);
        if (File.Exists(fullPath)) {
            IFile file;
            if (IsTextFile(fullPath)) {
                file = new TextFile(fullPath);
            } else if (IsImageFile(fullPath)) {
                file = new ImageFile(fullPath);
            } else if (IsPythonFile(fullPath)) {
                file = new PythonFile(fullPath);
            } else if (IsJavaFile(fullPath)) {
                file = new JavaFile(fullPath);
            } else {
                return "Unknown file type";
            }

            return file.GetData();
        } else {
            return "File not found";
        }
    }

    public string GetStatus() {
        DateTime lastSnapshotTime = _snapshotManager.LastSnapshotTime;
        StringBuilder statusBuilder = new StringBuilder();
        foreach (var kvp in _files) {
            DateTime lastModified = File.GetLastWriteTime(kvp.Key);
            string status = lastModified > lastSnapshotTime ? "changed" : "not changed";
            statusBuilder.AppendLine($"{kvp.Key}: {status}");
        }
        return statusBuilder.ToString();
    }
}

