using System.Threading.Tasks;

namespace OpenMind.Services.Validators.Interfaces
{
    public interface IEmailValidator : IValidator
    {
        Task<int> ValidateAsync(string email);
    }
}