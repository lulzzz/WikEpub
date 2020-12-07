using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WikEpub.Models;

namespace WikEpub.Services
{
    public class FileManagerService : BackgroundService
    {

        IDictionary<EpubFile, DateTime> epubFileTimeStamps { get; set; } = new Dictionary<EpubFile, DateTime>();
        public FileManagerService()
        {
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // call await worker.DoWork(token)
            await DoWork(stoppingToken, epubFileTimeStamps);
        }
        
        public async Task DoWork(CancellationToken cancellationToken, IDictionary<EpubFile, DateTime> epubFilesTimeStamps)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //do somethings
                await Task.Delay(1000 * 60); // 1 min
            }
        }

    }
}

