using McpSeriesGenerator.App.Shared.Abstractions;

namespace McpSeriesGenerator.App.Shared.Dtos
{
    public class ArtifactConfig : IArtifactConfig
    {
        public string BasePath { get; set; }
        public Dictionary<string, string> Input { get; set; }
        public Dictionary<string, string> Output { get; set; }
    }
}
