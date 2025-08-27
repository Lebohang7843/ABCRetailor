using System.IO;
using System.Threading.Tasks;
using ABCRetailers.Models;
using ABCRetailers.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailers.Controllers
{
    public class UploadController : Controller
    {
        private readonly IAzureStorageService _storage;
        public UploadController(IAzureStorageService storage) { _storage = storage; }

        public IActionResult Index() => View(new FileUploadModel());

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile proofFile)
        {
            if (proofFile == null || proofFile.Length == 0)
            {
                TempData["Error"] = "Please select a file to upload.";
                return RedirectToAction("Index");
            }

            // ✅ Only two arguments (IFormFile + optional file name)
            var (url, blobName) = await _storage.UploadProofAsync(proofFile, proofFile.FileName);

            TempData["Success"] = $"Proof uploaded successfully! URL: {url}";
            return RedirectToAction("Index");
        }

    }
}
