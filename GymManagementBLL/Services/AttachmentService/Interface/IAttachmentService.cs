using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.AttachmentService.Interface
{
    public interface IAttachmentService
    {
        string? UploadImage(string FolderName, IFormFile File);
        bool DeleteImage(string FileName, string FolderName);

    }
}
