using McpSeriesGenerator.App.Application.UseCases;
using McpSeriesGenerator.App.Infra.Data;

namespace McpSeriesGenerator.Integration.UseCases
{
    public class ValidateSerialNumberTest : IClassFixture<ArtifactsFixture>
    {
        private readonly ArtifactsFixture _fixture;

        public ValidateSerialNumberTest(ArtifactsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Given vehicles to validate When executing use case Then it should create Series Validated.txt file")]
        public async Task Given_VehiclesToValidate_When_ExecutingUseCase_Then_ItShouldCreateSerieVerificadaFile()
        {
            var vehicleData = new VehicleDataFile(_fixture._artifactConfig);
            var useCase = new ValidateSerialNumber(vehicleData);
            var vehicles = await new GetSerialNumberToValidate(vehicleData).ExecuteAsync(CancellationToken.None);
            await useCase.ExecuteAsync(vehicles, CancellationToken.None);
            var outputPath = Path.Combine(_fixture.ArtifactsPath, "Series Validated.txt");
            Assert.True(File.Exists(outputPath));
            var lines = await File.ReadAllLinesAsync(outputPath);
            Assert.All(lines, line => Assert.Matches(@".+ (true|false)", line));
        }
    }
}
