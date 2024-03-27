
namespace ControlFolderProject.Files; 
public interface IFile {
    string Name { get; }
    DateTime CreatedTime { get; }
    DateTime ChangedTime { get; }
    int NumberOfLines { get; }
    string GetData();
}
