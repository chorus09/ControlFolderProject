
namespace ControlFolderProject.Files; 
public interface IProgramFile : IFile {
    int NumberOfClasses { get; }
    int NumberOfMethods { get; }
    int NumberOfCodeLines { get; }
    string GetLanguage();
}
