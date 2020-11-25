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
            _downloadRoot =  $@"{_webHostEnvironment.ContentRootPath}\Downloads\";
        }

        public async Task<IActionResult> Index(Guid guid)
        {
            await ClearGuidFile(guid);
            return View();
        }

        public IActionResult DownloadPage(Guid guid, string urls, string bookTitle)
        {
            return View(new DownloadModel { BookTitle = bookTitle, WikiPages = urls.Split(' '), guid = guid});
        }


        public IActionResult BadUrls()
        {
            return View();
        }

        public async Task<IActionResult> DownloadAction(string urls, string bookTitle)
        {
            // validation of urls here, and checks for exceptions thrown by converter too (try/catch)
            if (urls is null | bookTitle is null) return RedirectToAction("BadUrls");
            Guid guid = Guid.NewGuid();
            if (await GetEpub(guid, urls, bookTitle))
                return RedirectToAction("DownloadPage", new {guid = guid, urls = urls, bookTitle = bookTitle});
            return RedirectToAction("BadUrls");
        }

        public async Task ClearGuidFile(Guid guid) =>
            await Task.Run(() =>
            {
                var filePath = $@"{_downloadRoot}{guid}.epub";
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            });

        public async Task<bool> GetEpub(Guid guid, string urls, string bookTitle)
        {
            bookTitle = bookTitle is null ? "WikiBook" : bookTitle;
            await _getEpub.FromAsync(urls.Split(' '), _downloadRoot, bookTitle, guid);
            return true;
        }
        
        public async Task<FileResult> DownloadFile(Guid guid, string bookTitle)
        {
            var fullDownloadPath = $"{_downloadRoot}{guid}.epub";
            byte[] epubFile = await System.IO.File.ReadAllBytesAsync(fullDownloadPath);
            await ClearGuidFile(guid);
            return File(epubFile, "application/epub+zip", $"{bookTitle}.epub");
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
