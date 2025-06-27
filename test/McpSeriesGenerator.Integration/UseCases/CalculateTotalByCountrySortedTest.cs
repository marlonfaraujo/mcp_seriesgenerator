using McpSeriesGenerator.App.Application.UseCases;
using McpSeriesGenerator.App.Infra.Data;

namespace McpSeriesGenerator.Integration.UseCases
{
    public class CalculateTotalByCountrySortedTest : IClassFixture<ArtifactsFixture>
    {
        private readonly ArtifactsFixture _fixture;

        public CalculateTotalByCountrySortedTest(ArtifactsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Given vehicles and countries When executing use case Then it should create Total by Country.txt file")]
        public async Task Given_VehiclesAndCountries_When_ExecutingUseCase_Then_ItShouldCreateListaTotaisFile()
        {
            var countryData = new CountryDataFile(_fixture._artifactConfig);
            var countries = await new GetCountryItems(countryData).ExecuteAsync(CancellationToken.None);
            var vehicleData = new VehicleDataFile(_fixture._artifactConfig);
            var vehicles = await new GetSerialNumberForReport(vehicleData).ExecuteAsync(CancellationToken.None);
            var useCase = new CalculateTotalByCountrySorted(vehicleData);
            await useCase.ExecuteAsync(vehicles, countries, CancellationToken.None);
            var outputPath = Path.Combine(_fixture.ArtifactsPath, "Total by Country.txt");
            Assert.True(File.Exists(outputPath));
            var lines = await File.ReadAllLinesAsync(outputPath);
            Assert.All(lines, line => Assert.True(countries.Any(x => line.Contains(x.Name)))); 
        }
    }
}
