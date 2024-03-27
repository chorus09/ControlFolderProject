using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlFolderProject.Files; 
public class ImageFile : IImageFile {
    private readonly string _filePath;

    public ImageFile(string filePath) {
        _filePath = filePath;
    }

    public string Name => Path.GetFileName(_filePath);
    public DateTime CreatedTime => File.GetCreationTime(_filePath);
    public DateTime ChangedTime => File.GetLastWriteTime(_filePath);
    public int NumberOfLines => 0; // Не применимо к изображениям

    public string Resolution {
        get {
            try {
                using (var img = System.Drawing.Image.FromFile(_filePath)) {
                    return $"{img.Width}x{img.Height}";
                }
            } catch (Exception ex) {
                return "Resolution not available: " + ex.Message;
            }
        }
    }

    public string GetData() {
        return $"File: {Name}\nCreated: {CreatedTime}\nChanged: {ChangedTime}\nResolution: {Resolution}";
    }
}
