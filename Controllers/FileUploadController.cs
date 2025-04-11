using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CompanyManagementSystem.Web.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadPath;

        public FileUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            
            // Uploads klasörünü oluştur
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public IActionResult Index()
        {
            var files = Directory.GetFiles(_uploadPath)
                .Select(Path.GetFileName)
                .ToList();
            
            return View(files);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Lütfen bir dosya seçin.";
                return RedirectToAction(nameof(Upload));
            }

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            TempData["Success"] = $"{fileName} başarıyla yüklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Download(string fileName)
        {
            var filePath = Path.Combine(_uploadPath, fileName);
            
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            
            return File(memory, GetContentType(filePath), fileName);
        }

        public IActionResult Delete(string fileName)
        {
            var filePath = Path.Combine(_uploadPath, fileName);
            
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                TempData["Success"] = $"{fileName} başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = "Dosya bulunamadı.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".txt" => "text/plain",
                ".pdf" => "application/pdf",
                ".doc" => "application/vnd.ms-word",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }
    }
} 