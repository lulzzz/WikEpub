using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using WikEpub.Models;
using Microsoft.AspNetCore.Http;
using WikEpubLib.Interfaces;
using WikEpubLib;
using Microsoft.AspNetCore.Authorization;
using WikEpub.Services;

namespace WikEpub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IHtmlsToEpub _getEpub;
        private readonly string _downloadRoot;
        private FileManagerService _fileManagerService;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnv, IHtmlsToEpub getEpub, FileManagerService fileManagerService)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnv;
            _getEpub = getEpub;
            _downloadRoot = $@"{_webHostEnvironment.ContentRootPath}\Downloads\";
            _fileManagerService = fileManagerService;
        }
        
        
        [HttpGet, Route("")]
        public IActionResult CreateEpub() => View();
        
        
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateEpub(EpubFile epubFile)
        {
            epubFile.guid = Guid.NewGuid();
            epubFile.FilePath = $"{_downloadRoot}{epubFile.guid}.epub";
            if (!ModelState.IsValid)
            {
                return RedirectToAction("BadUrls");
            }
            if (await GetEpub(epubFile))
            {
                _fileManagerService.epubFileLocationTimeStamps.Add(epubFile.FilePath, DateTime.Now);
                return RedirectToAction("DownloadPage", epubFile);
            }
            return Redirect("/ConversionFail");
        }
        public async Task<bool> GetEpub(EpubFile EpubFile)
        {
            await _getEpub.FromAsync(EpubFile.WikiPages, _downloadRoot, EpubFile.BookTitle, EpubFile.guid);
            return true;
        }

        public async Task<IActionResult> DownloadFile(EpubFile epubFile)
        {
            if (!System.IO.File.Exists(epubFile.FilePath))
                return Redirect("/FileNotFound");
            byte[] byteFile = await System.IO.File.ReadAllBytesAsync(epubFile.FilePath);
            await ClearEpubFile(epubFile.FilePath);
            return File(byteFile, "application/epub+zip", $"{epubFile.BookTitle}.epub");
        }

        [Route("Download")]
        public IActionResult DownloadPage(EpubFile epubFile) => View(epubFile);

        [Route("ConversionFail")]
        public IActionResult BadUrls() => View();

        [Route("FileNotFound")]
        public IActionResult FileNotFound() => View();

        public async Task<IActionResult> GoBackAndDelete(EpubFile epubFile)
        {
            await ClearEpubFile(epubFile.FilePath);
            return RedirectToAction("CreateEpub");
        }
        
        public async Task ClearEpubFile(string filePath) =>
            await Task.Run(() =>
            {
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            });

       
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
             View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
