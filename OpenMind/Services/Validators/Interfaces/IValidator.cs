using System.Threading.Tasks;

namespace OpenMind.Services.Validators.Interfaces
{
    public interface IValidator
    {
        Task<int> ValidateAsync(string data);
    }
}