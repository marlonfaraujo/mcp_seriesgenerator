namespace McpSeriesGenerator.Integration
{
    public class ArtifactsFixture
    {
        public string ArtifactsPath { get; }

        public ArtifactsFixture()
        {
            ArtifactsPath = Path.Combine(AppContext.BaseDirectory, "artifacts");

            if (!Directory.Exists(ArtifactsPath))
                throw new DirectoryNotFoundException($"'artifacts' folder not found: {ArtifactsPath}");
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
