using System.Diagnostics;

namespace McpSeriesGenerator.Integration.McpServer
{
    public class ProcessFixture : IDisposable
    {
        public Process _process { get; private set; }

        public ProcessFixture()
        {
            string appDll = Path.Combine(AppContext.BaseDirectory, "McpSeriesGenerator.App.dll");
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = $"\"{appDll}\"",
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
