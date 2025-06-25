using McpSeriesGenerator.App.Domain.Entities;

namespace McpSeriesGenerator.App.Application.Abstractions
{
    public interface ICountryData
    {
        Task<IEnumerable<Country>> GetAsync(CancellationToken cancellationToken = default);
    }
}
