using McpSeriesGenerator.App.Shared.Abstractions;
using McpSeriesGenerator.App.Shared.Dtos;
using Microsoft.Extensions.Configuration;

namespace McpSeriesGenerator.Integration
{
    public class ArtifactsFixture
    {
        private readonly IConfiguration _configuration;
        public readonly IArtifactConfig _artifactConfig;
        public string ArtifactsPath { get; }

        public ArtifactsFixture()
        {
            ArtifactsPath = Path.Combine(AppContext.BaseDirectory, "artifacts");

            if (!Directory.Exists(ArtifactsPath))
                throw new DirectoryNotFoundException($"'artifacts' folder not found: {ArtifactsPath}");

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _artifactConfig = _configuration.GetSection("Artifact").Get<ArtifactConfig>();
        }

        public string GetFile(string filename)
        {
            var fullPath = Path.Combine(ArtifactsPath, filename);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found: {fullPath}");
            return fullPath;
        }
    }
}
