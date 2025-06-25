namespace McpSeriesGenerator.App.Domain.Exceptions
{
    public class SerialNumberInvalidException : Exception
    {
        public SerialNumberInvalidException()
        {
        }

        public SerialNumberInvalidException(string? message) : base(message)
        {
        }
    }
}
