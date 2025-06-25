namespace McpSeriesGenerator.App.Domain.Exceptions
{
    public class CountryInvalidException : Exception
    {
        public CountryInvalidException()
        {
        }
        public CountryInvalidException(string? message) : base(message)
        {
        }   
    }
}
