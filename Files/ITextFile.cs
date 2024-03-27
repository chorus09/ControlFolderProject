
namespace ControlFolderProject.Files;
public interface ITextFile : IFile {
    int NumberOfWords { get; }
    int NumberOfCharacters { get; }
}
