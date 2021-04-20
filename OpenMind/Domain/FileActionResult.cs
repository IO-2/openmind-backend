namespace OpenMind.Domain
{
    public class FileActionResult : ServiceActionResult
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}