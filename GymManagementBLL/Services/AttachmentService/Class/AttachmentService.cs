using GymManagementBLL.Services.AttachmentService.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService.Class
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] AllowedExtentions = { ".jpg", ".jpeg", ".png" };
        private readonly IWebHostEnvironment _webHost;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost=webHost;
        }

        public string? UploadImage(string FolderName, IFormFile File)
        {
            try
            {
                if (FolderName is null || File is null || File.Length == 0) return null;

                if (File.Length > MaxFileSize) return null;

                var Extension = Path.GetExtension(File.FileName).ToLower();

                if (!AllowedExtentions.Contains(Extension)) return null;

                //D:\Course .NET\07 ASP .Net MVC\MVC Project\GymManagementSystemSolution\GymManagementPL\wwwroot\images\{FolderName}
                var FolderPath = Path.Combine(_webHost.WebRootPath, "images", FolderName);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                //saadfdssdasfasfas.jpg
                //dfsddxvssdfafaf_Test.jpg
                var FileName = Guid.NewGuid().ToString() + Extension;

                //D:\Course .NET\07 ASP .Net MVC\MVC Project\GymManagementSystemSolution\GymManagementPL\wwwroot\images\{FolderName}\{FileName}
                var FilePath = Path.Combine(FolderPath, FileName);
                using var FileStream = new FileStream(FilePath, FileMode.Create);

                File.CopyTo(FileStream);

                return FileName;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Upload File To Folder => {FolderName} : {ex.Message}");
                return null;
            }

        }
        public bool DeleteImage(string FileName, string FolderName)
        {
            try
            {
                var FilePath = Path.Combine(_webHost.WebRootPath, "images", FolderName, FileName);
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed To Delete File With Name => {FileName} From Folder => {FolderName} : {ex.Message}");
                return false;
            }

        }


    }
}
