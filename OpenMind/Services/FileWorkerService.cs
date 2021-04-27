using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using OpenMind.Domain;

namespace OpenMind.Services
{
    public abstract class FileWorkerService : Service
    {
        protected abstract IList<string> AllowedFiles { get; set; }
        protected abstract string ContentsFolderName { get; set; }
        
        private readonly string FolderSalt = "942f46443886666f932307475b0d0815";
        
        // TODO: Remove this shit
        protected string GetRootUrl(HttpContext httpContext)
        {
            var afterDomain = httpContext.Request.Path;
            var thisControllerPath = httpContext.Request.GetDisplayUrl();
            var root = thisControllerPath.Remove(thisControllerPath.IndexOf(afterDomain)) + "/";
            
            return root;
        }
        
        private string Md5Hash(string input)
        {
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                sBuilder.Append(result[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        protected async Task<SaveFileResponse> SaveFile(string webRootPath, string folder, IFormFile file)
        {
	    if (string.IsNullOrWhiteSpace(webRootPath))
	    {
   	        webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
	    }	

            var mediaPath = Path.Combine(webRootPath, folder + FolderSalt);
            if (!Directory.Exists(mediaPath))
            {
                Directory.CreateDirectory(mediaPath);
            }
            
            // TODO: Change this string working stuff to normal paths
            var onlyExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);
            var onlyName = file.FileName.Remove(file.FileName.LastIndexOf("."));

            var newFilename = Md5Hash(onlyName + new Random().Next(0, 100000) + DateTime.UtcNow) + "." + onlyExtension;

            using (FileStream fileStream = File.Create(Path.Combine(mediaPath, newFilename)))
            {
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }

            return new SaveFileResponse
            {
                SavedFilePath = Path.Combine(ContentsFolderName + FolderSalt, newFilename)
            };
        }

        protected void DeleteFile(string fullPath)
        {
            // TODO: Make proper deletion
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
