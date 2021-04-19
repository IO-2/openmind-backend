using OpenMind.Domain;

namespace OpenMind.Services
{
    public class Service
    {
        protected ServiceActionResult OkServiceActionResult()
        {
            return new ServiceActionResult{ Success = true };
        }
        
        protected ServiceActionResult BadServiceActionResult(params string[] errors)
        {
            return new ServiceActionResult
            {
                Success = false,
                Errors = errors
            };
        }
    }
}