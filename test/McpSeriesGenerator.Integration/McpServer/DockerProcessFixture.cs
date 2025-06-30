using System.Diagnostics;

namespace McpSeriesGenerator.Integration.McpServer
{
    public class DockerProcessFixture : IDisposable
    {
        public Process _process { get; private set; }

        public DockerProcessFixture()
        {
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = "exec -i mcp_seriesgenerator_app bash -c \"dotnet McpSeriesGenerator.App.dll\"",
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
