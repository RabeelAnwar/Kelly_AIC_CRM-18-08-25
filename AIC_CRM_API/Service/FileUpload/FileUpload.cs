using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Service.FileUpload
{
    public class FileUpload : IFileUpload
    {
        public void fileUpload(IFormFile file, string tenantId, string folderName, string fileName, IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                string webRootPath = hostingEnvironment.WebRootPath;

                var upload = Path.Combine(webRootPath, "Tenants/" + tenantId + "/" + folderName);

                if (!Directory.Exists(upload))
                {
                    Directory.CreateDirectory(upload);
                }

                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        //var extension = Path.GetExtension(file.FileName);
                        //var FileName = fileName + extension;
                        var FileName = fileName;

                        using (var filesStream = new FileStream(Path.Combine(upload, FileName), FileMode.Create))
                        {
                            file.CopyTo(filesStream);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
