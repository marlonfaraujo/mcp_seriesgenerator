using Microsoft.Extensions.Configuration;
using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Domain.Entities;
using McpSeriesGenerator.App.Shared.Dtos;

namespace McpSeriesGenerator.App.Infra.Data
{
    public class CountryDataFile : ICountryData
    {
        private const string artifactKey = "Artifact";
        private readonly IConfiguration _configuration;
        private readonly ArtifactConfig _artifactConfig;
        private readonly string _basePath;

        public CountryDataFile(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._artifactConfig = this._configuration.GetSection(artifactKey).Get<ArtifactConfig>();
            this._basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, this._artifactConfig.BasePath));
        }

        public async Task<IEnumerable<Country>> GetAsync(CancellationToken cancellationToken = default)
        {
            const string key = "countries";
            string filePath = Path.Combine(this._basePath, this._artifactConfig.Input[key]);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
            var countries = new List<Country>();
            await foreach (var item in File.ReadLinesAsync(filePath, cancellationToken))
            {
                if (item.Split(';').First().Length != 3)
                {
                    continue;
                }
                countries.Add(Country.CreateByCsv(item));
            }
            return countries;
        }
    }
}
