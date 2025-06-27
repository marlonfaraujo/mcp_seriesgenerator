using McpSeriesGenerator.App.Application.UseCases;
using McpSeriesGenerator.App.Infra.Data;

namespace McpSeriesGenerator.Integration.UseCases
{
    public class GenerateCheckDigitTest : IClassFixture<ArtifactsFixture>
    {
        private readonly ArtifactsFixture _fixture;

        public GenerateCheckDigitTest(ArtifactsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Given vehicles without check digit When executing use case Then it should create Series with Check Digit.txt file")]
        public async Task Given_VehiclesWithoutCheckDigit_When_ExecutingUseCase_Then_ItShouldCreateSerieComDVFile()
        {
            var vehicleData = new VehicleDataFile(_fixture._artifactConfig);
            var useCase = new GenerateCheckDigit(vehicleData);
            var vehicles = await new GetSerialNumberWithoutCheckDigit(vehicleData).ExecuteAsync(CancellationToken.None);
            await useCase.ExecuteAsync(vehicles, CancellationToken.None);

            var outputPath = Path.Combine(this._fixture.ArtifactsPath, "Series with Check Digit.txt");
            Assert.True(File.Exists(outputPath));
            var lines = await File.ReadAllLinesAsync(outputPath);
            Assert.All(lines, line => Assert.Contains("-", line));
        }
    }
}
