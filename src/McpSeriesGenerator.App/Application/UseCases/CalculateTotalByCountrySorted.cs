using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.Dtos;
using McpSeriesGenerator.App.Domain.Entities;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class CalculateTotalByCountrySorted
    {
        private readonly IVehicleData _vehicleData;

        public CalculateTotalByCountrySorted(IVehicleData vehicleData)
        {
            this._vehicleData = vehicleData;
        }

        public async Task ExecuteAsync(IEnumerable<VehicleDto> vehicleItems, IEnumerable<CountryDto> countryItems, CancellationToken cancellationToken = default)
        {
            var vehicles = new List<Vehicle>();
            foreach (var vehicle in vehicleItems)
            {
                var newVehicle = Vehicle.Create(vehicle.SerialNumber);
                vehicles.Add(newVehicle);
            }
            await this._vehicleData.SaveTotalAsync(vehicles, 
                countryItems.Select(country => Country.Create(country.Acronym, country.Name)), 
                cancellationToken);
        }
    }
}
