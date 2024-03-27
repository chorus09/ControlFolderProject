
namespace ControlFolderProject.Files;
public class JavaFile : IJavaFile {
    private readonly string _filePath;

    public JavaFile(string filePath) {
        _filePath = filePath;
    }

    public string Name => Path.GetFileName(_filePath);
    public DateTime CreatedTime => File.GetCreationTime(_filePath);
    public DateTime ChangedTime => File.GetLastWriteTime(_filePath);
    public int NumberOfLines => File.ReadAllLines(_filePath).Length;

    public int NumberOfClasses => CountClasses();
    public int NumberOfMethods => CountMethods();
    public int NumberOfCodeLines => CountCodeLines();

    public string GetLanguage() {
        return "Java";
    }

    public string GetData() {
        return $"File: {Name}\nCreated: {CreatedTime}\nChanged: {ChangedTime}\nNumber of Lines: {NumberOfLines}\nNumber of Classes: {NumberOfClasses}\nNumber of Methods: {NumberOfMethods}\nNumber of Code Lines: {NumberOfCodeLines}";
    }

    private int CountClasses() {
        // Пример: считаем ключевое слово class
        int count = 0;
        string[] lines = File.ReadAllLines(_filePath);
        foreach (var line in lines) {
            if (line.Trim().StartsWith("class")) {
                count++;
            }
        }
        return count;
    }

    private int CountMethods() {
        // Пример: считаем ключевое слово void
        int count = 0;
        string[] lines = File.ReadAllLines(_filePath);
        foreach (var line in lines) {
            if (line.Contains("void")) {
                count++;
            }
        }
        return count;
    }

    private int CountCodeLines() {
        // Считаем все строки, кроме комментариев и пустых строк
        int count = 0;
        string[] lines = File.ReadAllLines(_filePath);
        foreach (var line in lines) {
            if (!string.IsNullOrWhiteSpace(line) && !line.Trim().StartsWith("//")) {
                count++;
            }
        }
        return count;
    }
}
