using System.Diagnostics;

namespace McpSeriesGenerator.Integration.McpServer
{
    public class ProcessFixture : IDisposable
    {
        public Process _process { get; private set; }

        public ProcessFixture()
        {
            string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\..\src\McpSeriesGenerator.App"));
            string csproj = Path.Combine(projectDir, "McpSeriesGenerator.App.csproj");
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"run --no-launch-profile --project \"{csproj}\" --no-build",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            _process.Start();
            if (_process == null)
            {
                throw new InvalidOperationException("Failed to start MCP server process");
            }
        }

        public void Dispose()
        {
            if (_process != null && !_process.HasExited)
            {
                try
                {
                    _process.Kill();
                    _process.Dispose();
                }
                catch (Exception ex) 
                { 
                }
            }

            _process?.Dispose();
        }
    }
}
