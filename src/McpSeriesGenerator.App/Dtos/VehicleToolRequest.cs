namespace McpSeriesGenerator.App.Dtos
{
    public class VehicleToolRequest
    {
        public string? SerialNumber { get; set; } = string.Empty;
        public string? YearOfManufacture { get; set; } = string.Empty;
        public string? ModelYear { get; set; } = string.Empty;
        public string? VehicleType { get; set; } = string.Empty;
        public string? AcronymCountryOfManufacture { get; set; } = string.Empty;
    }
}
