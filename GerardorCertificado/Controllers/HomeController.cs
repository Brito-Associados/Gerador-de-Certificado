using GerardorCertificado.Pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;

namespace GerardorCertificado.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(IFormFile backImage, string listName, float positionName, int moreWidthToImage, int moreHeightToImage)
        {
            var names = NamesToList(listName);

            var zipFile = Compress(names, backImage, positionName, moreWidthToImage, moreHeightToImage);

            return File(zipFile, MediaTypeNames.Application.Zip, $"Certificados_{DateTime.Now.ToString("yyyy-MM-dd")}.rar", true);
        }
            
        [HttpPost]
        public ActionResult Test(IFormFile backImage, string nameTest, float positionName, int moreWidthToImage, int moreHeightToImage)
        {
            var image = backImage.OpenReadStream();
            var name = nameTest.Trim().ToUpper();

            using var pdf = Certificate.Generate(name, image, positionName, moreWidthToImage, moreHeightToImage);


            return File(pdf.ToArray(), MediaTypeNames.Application.Pdf, $"Certificado_{name}.pdf", true);
        }

        private static List<string> NamesToList(string names)
        {
            var listNames = new List<string>();

            names.Split("\n").ToList().ForEach(name =>
            {
                name = name.Replace("\r", "");
                name = name.Trim();
                name = name.ToUpper();

                if (!string.IsNullOrEmpty(name))
                {
                    listNames.Add(name);
                }
            });

            return listNames;
        }

        private static byte[] Compress(List<string> listName, IFormFile backImage, float positionName, int moreWidthToImage, int moreHeightToImage)
        {
            using var compressedFileStream = new MemoryStream();
            compressedFileStream.Seek(0, SeekOrigin.Begin);

            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, true))
            {
                foreach (var name in listName)
                {
                    var zipEntry = zipArchive.CreateEntry($"{name}.pdf", CompressionLevel.Optimal);
                    var image = backImage.OpenReadStream();
                    using var pdf = Certificate.Generate(name, image, positionName, moreWidthToImage, moreHeightToImage);
                    using var zipEntryStream = zipEntry.Open();

                    pdf.CopyTo(zipEntryStream);
                }
            }

            return compressedFileStream.ToArray();
        }

    }
}
