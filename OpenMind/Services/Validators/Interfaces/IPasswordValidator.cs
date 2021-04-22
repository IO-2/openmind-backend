using System.Threading.Tasks;

namespace OpenMind.Services.Validators.Interfaces
{
    public interface IPasswordValidator : IValidator
    {
        Task<int> ValidateAsync(string password);
    }
}