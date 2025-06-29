using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace McpSeriesGenerator.Integration.McpServer
{
    public class UploadVehicleToolTest : IClassFixture<ProcessFixture>
    {
        private readonly ProcessFixture _fixture;

        public UploadVehicleToolTest(ProcessFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async Task MCPServer_ShouldResponseToUploadVehicleWithSerialNumber()
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
                method = "tools/call",
                @params = new
                {
                    name = "UploadVehicleWithSerialNumber",
                    arguments = new
                    {
                        fileName = "upload.txt",
                        file = "1313MEXXXA7989-1\n0708BRAXXC4014-3\n1414ARGXXA5834-9\n1213ASMXXC8348-2\n0202ARGXXC2614-E\n0606BRAXXA6466-8\n0606MEXXXA3820-4"
                    }
                }
            });
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            var message = $"Content-Length: {jsonBytes.Length}\r\n\r\n{jsonString}";
            await _fixture._process.StandardInput.WriteLineAsync(message);
            await _fixture._process.StandardInput.FlushAsync();
            if (_fixture._process.StandardOutput.Peek() > 0)
            {
                var responseOut = await _fixture._process.StandardOutput.ReadLineAsync();
                Assert.True(!string.IsNullOrWhiteSpace(responseOut));
                Assert.Contains("jsonrpc", responseOut);
                JsonNode? node = JsonNode.Parse(responseOut);
                Assert.NotNull(node);
                Assert.NotNull(node?["result"]);
                Assert.NotNull(node?["result"]?["isError"]);
                Assert.True(bool.TryParse(node?["result"]?["isError"]?.ToString(), out bool isError));
                Assert.False(isError);
            }
        }
    }
}
