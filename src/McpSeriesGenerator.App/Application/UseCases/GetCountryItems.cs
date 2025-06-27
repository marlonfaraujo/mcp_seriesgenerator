using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.Dtos;

namespace McpSeriesGenerator.App.Application.UseCases
{
    public class GetCountryItems
    {

        private readonly ICountryData _contryData;

        public GetCountryItems(ICountryData contryData)
        {
            this._contryData = contryData;
        }

        public async Task<IEnumerable<CountryDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var countries = await this._contryData.GetAsync(cancellationToken);
            return countries.Select(country => new CountryDto(country.Acronym, country.Name));
        }
    }
}
