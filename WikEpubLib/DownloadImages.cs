using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikEpubLib
{
    public class DownloadImages : IDownloadImages
    {
        public async Task From(WikiPageRecord pageRecord, string oepbsDirectory, HttpClient httpClient)=>
            await pageRecord.SrcMap.ToList().ForEachAsync(async src =>
            {
                string htmlImageRepository = src.Value.Split('\\')[0];
                string filePath = @$"{oepbsDirectory}\{src.Value}";

                HttpResponseMessage responseResult = await httpClient.GetAsync(@$"https://{src.Key}");
                using var memoryStream = await responseResult.Content.ReadAsStreamAsync();
                Directory.CreateDirectory(@$"{oepbsDirectory}\{htmlImageRepository}\");
                await using var fileStream = File.Create(filePath);
                await memoryStream.CopyToAsync(fileStream);
            });
         
    }
}
