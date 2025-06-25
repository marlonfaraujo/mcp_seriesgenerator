using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.Dtos;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class GetSerialNumberForReport
    {
        private readonly IVehicleData _vehicleData;

        public GetSerialNumberForReport(IVehicleData vehicleData)
        {
            this._vehicleData = vehicleData;
        }

        public async Task<IEnumerable<VehicleDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var vehicles = await this._vehicleData.GetForReportAsync(cancellationToken);
            return vehicles.Select(x => new VehicleDto(x.VehicleSerialNumber.GetValueWithCheckDigit()));
        }
    }
}
