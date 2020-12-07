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

namespace WikEpub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public IHtmlsToEpub _getEpub;
        private readonly string _downloadRoot;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnv, IHtmlsToEpub getEpub)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnv;
            _getEpub = getEpub;
            _downloadRoot = $@"{_webHostEnvironment.ContentRootPath}\Downloads\";
        }
        
        public IActionResult Index() => RedirectToAction("CreateEpub");
        
        [HttpGet, Route("")]
        public IActionResult CreateEpub() => View();
        
        
        [HttpPost, Route("")]
        public async Task<IActionResult> CreateEpub(EpubFile epubFile)
        {
            epubFile.guid = Guid.NewGuid();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("BadUrls");
            }
            if (await GetEpub(epubFile))
                return RedirectToAction("DownloadPage", epubFile);
            return Redirect("/ConversionFail");
        }
         
        [Route("Download")]
        public IActionResult DownloadPage(EpubFile epubFile) => View(epubFile);

        [Route("ConversionFail")]
        public IActionResult BadUrls() => View();

        [Route("FileNotFound")]
        public IActionResult FileNotFound() => View();

        public async Task<IActionResult> GoBackAndDelete(EpubFile epubFile)
        {
            await ClearEpubFile(epubFile.guid);
            return RedirectToAction("CreateEpub");
        }
        
        public async Task ClearEpubFile(Guid guid) =>
            await Task.Run(() =>
            {
                var filePath = $@"{_downloadRoot}{guid}.epub";
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            });

        public async Task<bool> GetEpub(EpubFile EpubFile)
        {
            await _getEpub.FromAsync(EpubFile.WikiPages, _downloadRoot, EpubFile.BookTitle, EpubFile.guid);
            return true;
        }

        public async Task<IActionResult> DownloadFile(EpubFile epubFile)
        {
            var fullDownloadPath = $"{_downloadRoot}{epubFile.guid}.epub";
            if (!System.IO.File.Exists(fullDownloadPath))
                return Redirect("/FileNotFound");
            byte[] byteFile = await System.IO.File.ReadAllBytesAsync(fullDownloadPath);
            await ClearEpubFile(epubFile.guid);
            return File(byteFile, "application/epub+zip", $"{epubFile.BookTitle}.epub");
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
             View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
