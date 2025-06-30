using McpSeriesGenerator.App.Domain.Entities;
using McpSeriesGenerator.App.Dtos;
using McpSeriesGenerator.App.Infra.Data;
using McpSeriesGenerator.App.Shared.Abstractions;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace McpSeriesGenerator.App.McpServer
{
    [McpServerToolType]
    public class VehicleTool
    {
        private readonly IArtifactConfig _artifactConfig;
        private readonly string _basePath;

        public VehicleTool(IArtifactConfig artifactConfig)
        {
            this._artifactConfig = artifactConfig;
            this._basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, this._artifactConfig.BasePath));
        }

        [McpServerTool(Name = "GetVehiclesWithCheckDigitCreated"), Description("Get vehicles with serial number and check digit.")]
        public async Task<string> GetVehiclesWithCheckDigitCreated(
            [Description("Input params: SerialNumber, YearOfManufacture, ModelYear, VehicleType, AcronymCountryOfManufacture")] VehicleToolRequest request, 
            CancellationToken cancellationToken = default)
        {
            var outputPath = FileService.GetFilePath(this._basePath, this._artifactConfig.Output["seriesWithCheckDigit"]);
            var vehicles = await FileService.ReadLinesAsObjectsAsync<Vehicle>(
                originalPath: outputPath,
                parser: line =>
                {
                    return Vehicle.Create(line);
                },cancellationToken: cancellationToken);

            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.SerialNumber))
                {
                    vehicles = vehicles
                        .Where(vehicle => vehicle.VehicleSerialNumber.GetValueWithCheckDigit().Contains(request.SerialNumber, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if(!string.IsNullOrWhiteSpace(request.YearOfManufacture) && int.TryParse(request.YearOfManufacture, out int yearOfManufacture))
                {
                    vehicles = vehicles
                        .Where(vehicle => vehicle.YearOfManufacture.ToString("D4").Equals(yearOfManufacture.ToString("D4")))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(request.ModelYear) && int.TryParse(request.ModelYear, out int modelYear))
                {
                    vehicles = vehicles
                        .Where(vehicle => vehicle.ModelYear.ToString("D4").Equals(modelYear.ToString("D4")))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(request.VehicleType))
                {
                    vehicles = vehicles
                        .Where(vehicle => vehicle.VehicleType.Equals(request.VehicleType))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(request.AcronymCountryOfManufacture))
                {
                    vehicles = vehicles
                        .Where(vehicle => vehicle.AcronymCountryOfManufacture.Equals(request.AcronymCountryOfManufacture))
                        .ToList();
                }
            }

            return string.Join("\n--\n", vehicles.Select(vehicle =>
            {
                return $"""
                    SerialNumber: {vehicle.VehicleSerialNumber.GetValueWithCheckDigit()} 
                    YearOfManufacture: {vehicle.YearOfManufacture} 
                    ModelYear: {vehicle.ModelYear} 
                    VehicleType: {vehicle.VehicleType} 
                    AcronymCountryOfManufacture: {vehicle.AcronymCountryOfManufacture}
                    IsValid: {vehicle.VehicleSerialNumber.ValidateCheckDigit()}
                """;
            }));
        }

        [McpServerTool(Name = "GetVehiclesWithValidatedSerialNumberAndCheckDigit"), Description("Get vehicles with validated serial number check digit.")]
        public async Task<string> GetVehiclesWithValidatedSerialNumberAndCheckDigit(CancellationToken cancellationToken = default)
        {
            var outputPath = FileService.GetFilePath(this._basePath, this._artifactConfig.Output["seriesValidated"]);
            var lines = await FileService.ReadLinesAsync(outputPath, cancellationToken);
            return string.Join("\n--\n", lines.Select(x =>
            {
                return x;
            }));
        }

        [McpServerTool(Name = "TotalVehiclesByCountry"), Description("Get total vehicles by country.")]
        public async Task<string> GetTotalVehiclesByCountry(CancellationToken cancellationToken = default)
        {
            var outputPath = FileService.GetFilePath(this._basePath, this._artifactConfig.Output["totalByCountry"]);
            var lines = await FileService.ReadLinesAsync(outputPath, cancellationToken);
            return string.Join("\n--\n", lines.Select(x =>
            {
                return x;
            }));
        }

        [McpServerTool(Name = "ReturnsIfTheSerialNumberIsValid"), Description("Validate serial number with check digit.")]
        public string ReturnsIfTheSerialNumberIsValid(
            [Description("Enter a serial number with check digit")] string SerialNumber)
        {
            if (string.IsNullOrWhiteSpace(SerialNumber))
            {
                return "Serial number cannot be empty.";
            }
            var vehicle = Vehicle.Create(SerialNumber);
            if (vehicle.VehicleSerialNumber.ValidateCheckDigit())
            {
                return "This serial number is valid";
            }
            return "This serial number is invalid";
        }
    }
}
