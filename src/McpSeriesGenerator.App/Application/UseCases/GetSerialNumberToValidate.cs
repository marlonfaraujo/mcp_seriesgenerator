using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.Dtos;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class GetSerialNumberToValidate
    {
        private readonly IVehicleData _vehicleData;

        public GetSerialNumberToValidate(IVehicleData vehicleData)
        {
            this._vehicleData = vehicleData;
        }

        public async Task<IEnumerable<VehicleDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var vehicles = await this._vehicleData.GetToValidateAsync(cancellationToken);
            return vehicles.Select(x => new VehicleDto(x.VehicleSerialNumber.GetValueWithCheckDigit()));
        }
    }
}
