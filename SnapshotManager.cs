
namespace ControlFolderProject; 
public class SnapshotManager {
    private DateTime _lastSnapshotTime;

    public SnapshotManager() {
        _lastSnapshotTime = DateTime.MinValue;
    }

    public DateTime LastSnapshotTime => _lastSnapshotTime;

    public void TakeSnapshot() {
        _lastSnapshotTime = DateTime.Now;
    }
}
