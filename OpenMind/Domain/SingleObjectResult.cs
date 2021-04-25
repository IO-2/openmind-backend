namespace OpenMind.Domain
{
    public class SingleObjectResult<T> : ServiceActionResult
    {
        public T Data { get; set; }
    }
}