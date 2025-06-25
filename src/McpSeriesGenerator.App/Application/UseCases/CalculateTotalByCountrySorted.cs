using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.Dtos;
using McpSeriesGenerator.App.Domain.Entities;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class CalculateTotalByCountrySorted
    {
        private readonly IVehicleData _vehicleData;
        private readonly GetCountryItems _getCountryItems;

        public CalculateTotalByCountrySorted(IVehicleData vehicleData, GetCountryItems getCountryItems)
        {
            this._vehicleData = vehicleData;
            this._getCountryItems = getCountryItems;
        }

        public async Task ExecuteAsync(IEnumerable<VehicleDto> items, CancellationToken cancellationToken = default)
        {
            var countries = await this._getCountryItems.ExecuteAsync(cancellationToken);
            var vehicles = new List<Vehicle>();
            foreach (var vehicle in items)
            {
                var newVehicle = Vehicle.Create(vehicle.SerialNumber);
                vehicles.Add(newVehicle);
            }
            await this._vehicleData.SaveTotalAsync(vehicles, countries, cancellationToken);
        }
    }
}
