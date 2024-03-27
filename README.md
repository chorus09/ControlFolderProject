ControlFolderProject
Description
The ControlFolderProject is a program designed to monitor and detect changes in documents within a specified folder.

How It Works
The program consists of the following components:

1. FolderObserver: A class responsible for monitoring the folder and handling events for file changes, creations, deletions, and renames. It uses instances of classes implementing interfaces to represent different file types.
2. Logger: A class for logging events such as file creations, renames, and deletions.
3. SnapshotManager: A class for managing snapshot timestamps of the folder's state.

Usage
Ensure you have the .NET Core SDK installed.
Clone the repository to your local machine.
Open a console in the project's root directory.
Run the application and enter commands in the console to manage the folder monitoring:
commit: Takes a snapshot of the current folder state.
info <filename>: Displays information about a file.
status: Displays the status of all files in the folder.

Example usage:
dotnet run
Available commands: commit, info <filename>, status
Enter command: commit
Commit successful
Enter command: info test.txt
Filename: test.txt
Extension: .txt
Created: 2023-10-11 09:01:28
Updated: 2023-10-11 09:01:28
Enter command: status
files\test.txt: not changed
files\image.png: changed
files\python_script.py: not changed
File Types
The program supports various file types, including text files, images, Python files, and Java files. Each file type is handled appropriately to display information about it.

File Interfaces
IFile: Parent interface for all file types.
IProgramFile: Interface for program files (Java and Python).
IJavaFile: Interface for Java files.
IPythonFile: Interface for Python files.
IImageFile: Interface for images.
ITextFile: Interface for text files.

Interface Implementations
TextFile: Implementation of the ITextFile interface for text files. Counts the number of lines, words, and characters in the file.
ImageFile: Implementation of the IImageFile interface for images. Displays the image size.
PythonFile: Implementation of the IPythonFile interface for Python files. Counts the number of lines of code.
JavaFile: Implementation of the IJavaFile interface for Java files. Counts the number of lines of code.
