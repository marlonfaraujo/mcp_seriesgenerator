using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Domain.Entities;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class GetCountryItems
    {

        private readonly ICountryData _contryData;

        public GetCountryItems(ICountryData contryData)
        {
            this._contryData = contryData;
        }

        public async Task<IEnumerable<Country>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var countries = await this._contryData.GetAsync(cancellationToken);
            return countries;
        }
    }
}
