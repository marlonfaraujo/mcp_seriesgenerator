using Microsoft.Extensions.Configuration;
using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Domain.Entities;
using McpSeriesGenerator.App.Dtos;

namespace McpSeriesGenerator.App.Infra.Data
{
    public class VehicleDataFile : IVehicleData
    {
        private const string artifactKey = "Artifact";
        private readonly IConfiguration _configuration;
        private readonly ArtifactConfig _artifactConfig;
        private readonly string _basePath;

        public VehicleDataFile(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._artifactConfig = this._configuration.GetSection(artifactKey).Get<ArtifactConfig>();
            this._basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, this._artifactConfig.BasePath));
        }

        public async Task<IEnumerable<Vehicle>> GetWithoutCheckDigitAsync(CancellationToken cancellationToken = default)
        {
            const string key = "seriesWithoutCheckDigit";
            var vehicles = new List<Vehicle>();
            await foreach (var item in File.ReadLinesAsync(GetFilePath(this._artifactConfig.Input[key])))
            {
                vehicles.Add(Vehicle.Create(item));
            }
            return vehicles;
        }

        public async Task<IEnumerable<Vehicle>> GetToValidateAsync(CancellationToken cancellationToken = default)
        {
            const string key = "seriesForValidate";
            var vehicles = new List<Vehicle>();
            await foreach (var item in File.ReadLinesAsync(GetFilePath(this._artifactConfig.Input[key])))
            {
                vehicles.Add(Vehicle.Create(item));
            }
            return vehicles;
        }

        public async Task SaveWithCheckDigitAsync(IEnumerable<Vehicle> vehicles, CancellationToken cancellationToken = default)
        {
            const string key = "seriesWithCheckDigit";
            string filePath = Path.Combine(this._basePath, this._artifactConfig.Output[key]);
            await File.WriteAllLinesAsync(filePath, 
                vehicles.Select(x => x.VehicleSerialNumber.GetValueWithCheckDigit()),
                cancellationToken);
        }

        public async Task SaveValidatedAsync(IEnumerable<Vehicle> vehicles, CancellationToken cancellationToken = default)
        {
            const string key = "seriesValidated";
            string filePath = Path.Combine(this._basePath, this._artifactConfig.Output[key]);
            var dict = vehicles.ToDictionary(item => item.VehicleSerialNumber.GetValueWithCheckDigit(), 
                item => item.VehicleSerialNumber.ValidateCheckDigit() ? "true" : "false");
            await File.WriteAllLinesAsync(filePath, 
                dict.Select(x => $"{x.Key} {x.Value}"), cancellationToken);
        }

        public async Task<IEnumerable<Vehicle>> GetForReportAsync(CancellationToken cancellationToken = default)
        {
            const string key = "seriesforReporting";
            var vehicles = new List<Vehicle>();
            await foreach (var item in File.ReadLinesAsync(GetFilePath(this._artifactConfig.Input[key]), cancellationToken))
            {
                vehicles.Add(Vehicle.Create(item));
            }
            return vehicles;
        }

        public async Task SaveTotalAsync(IEnumerable<Vehicle> vehicles, IEnumerable<Country> countries, CancellationToken cancellationToken = default)
        {
            const string key = "totalByCountry";
            string filePath = Path.Combine(this._basePath, this._artifactConfig.Output[key]);
            var totals = vehicles
                .GroupBy(g => g.AcronymCountryOfManufacture)           
                .Select(vehicle => new CountryTotalDto(countries.FirstOrDefault(country => 
                    country.Acronym.Equals(vehicle.First().AcronymCountryOfManufacture)).Name, 
                    vehicle.Count()))
                .ToList();
            var dict = totals
                .OrderBy(o => o.Name)
                .ToDictionary(item => item.Name, item => item.Total);
            await File.WriteAllLinesAsync(filePath, 
                dict.Select(x => $"{x.Key}-{x.Value}"), cancellationToken);
        }

        private string GetFilePath(string fileName)
        {
            string filePath = Path.Combine(this._basePath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
            return filePath;
        }
    }
}
