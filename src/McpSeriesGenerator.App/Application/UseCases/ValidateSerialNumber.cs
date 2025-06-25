using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.Dtos;
using McpSeriesGenerator.App.Domain.Entities;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class ValidateSerialNumber
    {
        private readonly IVehicleData _vehicleData;

        public ValidateSerialNumber(IVehicleData vehicleData)
        {
            this._vehicleData = vehicleData;
        }

        public async Task ExecuteAsync(IEnumerable<VehicleDto> items, CancellationToken cancellationToken = default)
        {
            var vehicles = new List<Vehicle>();
            foreach (var vehicle in items)
            {
                vehicles.Add(Vehicle.Create(vehicle.SerialNumber));
            }
            await this._vehicleData.SaveValidatedAsync(vehicles, cancellationToken);
        }
    }
}
