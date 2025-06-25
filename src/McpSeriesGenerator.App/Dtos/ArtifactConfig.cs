namespace McpSeriesGenerator.App.Dtos
{
    public record ArtifactConfig(string BasePath, Dictionary<string, string> Input, Dictionary<string, string> Output)
    {
    }
}
