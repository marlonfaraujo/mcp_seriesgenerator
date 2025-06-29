using McpSeriesGenerator.App.Shared.Abstractions;

namespace McpSeriesGenerator.App.Shared.Dtos
{
    public class ArtifactConfig : IArtifactConfig
    {
        public string BasePath { get; set; } = string.Empty;
        public Dictionary<string, string> Input { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> Output { get; set; } = new Dictionary<string, string>();
    }
}
