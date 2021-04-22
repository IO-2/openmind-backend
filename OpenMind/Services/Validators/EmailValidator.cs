using System.Threading.Tasks;
using OpenMind.Services.Validators.Interfaces;

namespace OpenMind.Services.Validators
{
    public class EmailValidator : IEmailValidator
    {
        public async Task<int> ValidateAsync(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                return 455;
            }

            return 200;
        }
    }
}