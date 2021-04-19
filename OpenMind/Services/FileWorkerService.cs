using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace OpenMind.Services
{
    public class FileWorkerService : Service
    {
        public string GetRootUrl(HttpContext httpContext)
        {
            var afterDomain = httpContext.Request.Path;
            var thisControllerPath = httpContext.Request.GetDisplayUrl();
            var root = thisControllerPath.Remove(thisControllerPath.IndexOf(afterDomain)) + "/";
            
            return root;
        }
    }
}