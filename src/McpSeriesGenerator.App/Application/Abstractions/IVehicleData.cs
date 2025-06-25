using McpSeriesGenerator.App.Domain.Entities;

namespace McpSeriesGenerator.App.Application.Abstractions
{
    public interface IVehicleData
    {
        Task<IEnumerable<Vehicle>> GetWithoutCheckDigitAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Vehicle>> GetToValidateAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Vehicle>> GetForReportAsync(CancellationToken cancellationToken = default);
        Task SaveWithCheckDigitAsync(IEnumerable<Vehicle> vehicles, CancellationToken cancellationToken = default);
        Task SaveValidatedAsync(IEnumerable<Vehicle> vehicles, CancellationToken cancellationToken = default);
        Task SaveTotalAsync(IEnumerable<Vehicle> vehicles, IEnumerable<Country> countries, CancellationToken cancellationToken = default);
    }
}
