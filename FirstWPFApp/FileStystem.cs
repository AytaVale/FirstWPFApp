using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstWPFApp
{
    public class FileSystem
    {
        private readonly FileSystemWatcher _watcher;
        private readonly List<IFileLoader> _loaders;
        public event Action<string> OnNewFileProcessed;

        public FileSystem(List<IFileLoader> loaders)
        {
            _loaders = loaders;

            string inputDirectory = ConfigurationManager.AppSettings["InputDirectory"];
            int monitoringFrequency = int.Parse(ConfigurationManager.AppSettings["MonitoringFrequency"]);

            _watcher = new FileSystemWatcher(inputDirectory)
            {
                Filter = "*.*",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                InternalBufferSize = monitoringFrequency
            };

            _watcher.Created += OnNewFile;
        }

        public async void OnNewFile(object sender, FileSystemEventArgs e)
        {
            await Task.Run(() => ProcessFile(e.FullPath));
            OnNewFileProcessed?.Invoke(e.FullPath);
        }

        public void ProcessFile(string filePath)
        {
            foreach (var loader in _loaders)
            {
                try
                {
                    var data = loader.Load(filePath);
                   
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        public void Start() => _watcher.EnableRaisingEvents = true;

        public void Stop() => _watcher.EnableRaisingEvents = false;
    }
}
