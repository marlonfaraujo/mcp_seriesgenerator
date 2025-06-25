namespace McpSeriesGenerator.App.Domain.Exceptions
{
    public class VehicleInvalidException : Exception
    {
        public VehicleInvalidException()
        {
        }

        public VehicleInvalidException(string? message) : base(message)
        {
        }
    }
}
