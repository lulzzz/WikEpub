using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WikEpub.Models;

namespace WikEpub.Services
{
    public class FileManagerService : BackgroundService
    {

        public IDictionary<string, DateTime> epubFileLocationTimeStamps { get; set; } = new Dictionary<string, DateTime>();
        private IWebHostEnvironment _webHostEnv;
        public FileManagerService(IWebHostEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv; 
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DoWork(stoppingToken, epubFileLocationTimeStamps);
        }
        
        public async Task DoWork(CancellationToken cancellationToken, IDictionary<string, DateTime> epubFilesTimeStamps)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RemoveOldDownloads(epubFilesTimeStamps);
            }
        }

        private async Task RemoveOldDownloads(IDictionary<string, DateTime> epubFilesTimeStamps)
        {
            var currentDateTime = DateTime.Now;
            foreach (var (directory, dateTimeAdded) in epubFilesTimeStamps)
            {
                if (ElapsedTimeSinceDownloadRequest(dateTimeAdded, currentDateTime) >= 1)
                {
                   //System.Diagnostics.Debug.WriteLine($"current dt: {currentDateTime} \n" +
                   //     $"directory dt: {dateTimeAdded} \n" +
                   //     $"directory: {directory}");
                    var deleteFileTask = Task.Run(() =>
                    { if (File.Exists(directory)) File.Delete(directory); });
                    epubFileLocationTimeStamps.Remove(directory);
                    await deleteFileTask;
                }
            }
            await Task.Delay(1000 * 60); // 1 min
        }

        private double ElapsedTimeSinceDownloadRequest(DateTime timeAdded, DateTime currentTime)
            => (currentTime - timeAdded).TotalMinutes;
        
    }
}

