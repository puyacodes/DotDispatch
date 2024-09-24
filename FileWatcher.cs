using System.Diagnostics;

namespace DotDispatch
{
    public class FileWatcher: IDisposable
    {
        FileSystemWatcher _watcher;
        string _path;
        string _filename;
        string _html;
        bool _uptodate;
        public FileWatcher(string path, string filename)
        {
            _path = path;
            _filename = filename;
            _watcher = new FileSystemWatcher(path);
            _watcher.NotifyFilter = NotifyFilters.Attributes
                         | NotifyFilters.CreationTime
                         | NotifyFilters.DirectoryName
                         | NotifyFilters.FileName
                         | NotifyFilters.LastAccess
                         | NotifyFilters.LastWrite
                         | NotifyFilters.Security
                         | NotifyFilters.Size;

            _watcher.Changed += Watcher_Changed;
            _watcher.Created += Watcher_Changed;
            _watcher.Deleted += Watcher_Changed;
            _watcher.Renamed += Watcher_Changed;
            _watcher.Error += Watcher_Error;
            _watcher.Filter = filename;
            _watcher.IncludeSubdirectories = false;
            _watcher.EnableRaisingEvents = true;
        }
        public void Init()
        {
            _uptodate = !System.IO.File.Exists(System.IO.Path.Combine(_path, _filename));
        }
        void Watcher_Error(object sender, ErrorEventArgs e)
        {
            Debug.WriteLine($"Watcher: ERROR, {e.GetException()}");
        }
        void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    _uptodate = false;
                    break;
                case WatcherChangeTypes.Created:
                    _uptodate = false;
                    break;
                case WatcherChangeTypes.Renamed:
                    _html = "";
                    break;
                case WatcherChangeTypes.Deleted:
                    _html = "";
                    break;
                default:
                    break;
            }
        }
        public string GetContent()
        {
            if (string.IsNullOrEmpty(_html) || !_uptodate)
            {
                var path = System.IO.Path.Combine(_path, _filename);

                using StreamReader reader = new(path, new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open, Share = FileShare.Read });

                _html = reader.ReadToEnd();
            }

            return _html;
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
