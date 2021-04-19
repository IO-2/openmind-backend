using OpenMind.Contracts.Responses;

namespace OpenMind.Domain
{
    public class ChecklistActionResult : ServiceActionResult
    {
        public ChecklistResponseContract Checklist { get; set; }
    }
}