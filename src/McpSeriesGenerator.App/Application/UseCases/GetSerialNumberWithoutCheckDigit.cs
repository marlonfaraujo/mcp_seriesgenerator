using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.Dtos;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class GetSerialNumberWithoutCheckDigit
    {
        private readonly IVehicleData _vehicleData;

        public GetSerialNumberWithoutCheckDigit(IVehicleData vehicleData)
        {
            this._vehicleData = vehicleData;
        }

        public async Task<IEnumerable<VehicleDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var vehicles = await this._vehicleData.GetWithoutCheckDigitAsync(cancellationToken);
            return vehicles.Select(x => new VehicleDto(x.VehicleSerialNumber.GetValueWithCheckDigit()));
        }
    }
}
