using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GoshenJimenez.Eitri.Models;
using System.IO;
using Newtonsoft.Json;

namespace GoshenJimenez.Eitri.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            SaveFileViewModel model = new SaveFileViewModel()
            {
                Files = new List<FileNamePair>()
            };

            var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");
            foreach (string file in Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories))
            {
                var data = JsonConvert.DeserializeObject<FileModel>(System.IO.File.ReadAllText(file));
                model.Files.Add(new FileNamePair(){
                    FileName = data.FileName,
                    SaveName = System.IO.Path.GetFileName(file).Split('.')[0].ToString()
                });
            }
            return View(model);
        }

        [HttpPost, Route("/home/save-file")]
        public IActionResult SaveFile(SaveFileViewModel model)
        {
            if (!string.IsNullOrEmpty(model.FileData))
            {
                var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");

                if (!Directory.Exists(dirPath))
                     Directory.CreateDirectory(dirPath);

                var logFile = System.IO.File.Create(dirPath + "/" + Guid.NewGuid().ToString() + ".txt");
                var logWriter = new System.IO.StreamWriter(logFile);
                var file = new FileModel()
                {
                    FileData = model.FileData,
                    FileName = model.FileName,
                    LastModifiedDate = model.LastModifiedDate,
                    FileSize = model.FileSize,
                    FileType = model.FileType
                };

                logWriter.WriteLine(JsonConvert.SerializeObject(file));
                logWriter.Dispose();
            }
            return RedirectToAction("Index");
        }

        [HttpGet, Route("file/{fileName}")]
        public JsonResult File(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = "";

            var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "assets", fileName + ".txt");
            FileModel file = JsonConvert.DeserializeObject<FileModel>(System.IO.File.ReadAllText(filePath));

            return Json(file);
        }

    }
}
