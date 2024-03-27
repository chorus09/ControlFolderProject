
namespace ControlFolderProject.Files; 
public class TextFile : ITextFile {
    private readonly string _filePath;

    public TextFile(string filePath) {
        _filePath = filePath;
    }

    public string Name => Path.GetFileName(_filePath);
    public DateTime CreatedTime => File.GetCreationTime(_filePath);
    public DateTime ChangedTime => File.GetLastWriteTime(_filePath);
    public int NumberOfLines => File.ReadAllLines(_filePath).Length;

    public int NumberOfWords {
        get {
            string[] words = File.ReadAllText(_filePath).Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }

    public int NumberOfCharacters => File.ReadAllText(_filePath).Length;

    public string GetData() {
        return $"File: {Name}\nCreated: {CreatedTime}\nChanged: {ChangedTime}\nNumber of Lines: {NumberOfLines}\nNumber of Words: {NumberOfWords}\nNumber of Characters: {NumberOfCharacters}";
    }
}
