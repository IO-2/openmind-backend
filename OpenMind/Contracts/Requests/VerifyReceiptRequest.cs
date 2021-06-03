using OpenMind.Models;

namespace OpenMind.Contracts.Requests
{
    public class VerifyReceiptRequest : Localizable
    {
        public string Receipt { get; set; }
    }
}