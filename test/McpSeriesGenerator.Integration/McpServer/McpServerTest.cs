using System.Text.Json;
using System.Text.Json.Nodes;

namespace McpSeriesGenerator.Integration.McpServer
{
    public class McpServerTest : IClassFixture<DockerProcessFixture>
    {
        private readonly DockerProcessFixture _fixture;

        public McpServerTest(DockerProcessFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task MCPServer_ShouldResponseToListOfTools()
        {
            Assert.NotNull(_fixture._process);

            _ = Task.Run(async () =>
            {
                string? err;
                while ((err = await _fixture._process.StandardError.ReadLineAsync()) != null)
                    Console.WriteLine("[STDERR] " + err);
            });
            var jsonString = JsonSerializer.Serialize(new
            {
                jsonrpc = "2.0",
                id = 1,
                method = "tools/list",
                @params = new { }
            });
            await _fixture._process.StandardInput.WriteLineAsync(jsonString);
            await _fixture._process.StandardInput.FlushAsync();
            if (_fixture._process.StandardOutput.Peek() > 0)
            {
                var responseOut = await _fixture._process.StandardOutput.ReadLineAsync();
                Assert.True(!string.IsNullOrWhiteSpace(responseOut));
                Assert.Contains("jsonrpc", responseOut);
                JsonNode? node = JsonNode.Parse(responseOut);
                Assert.NotNull(node);
                Assert.NotNull(node?["result"]);
                Assert.NotNull(node?["result"]?["tools"]);
            }
        }
    }
}
