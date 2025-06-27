namespace McpSeriesGenerator.App.Shared.Abstractions
{
    public interface IArtifactConfig
    {
        public string BasePath { get; }
        public Dictionary<string,string> Input { get; }
        public Dictionary<string,string> Output{ get; }
    }
}
