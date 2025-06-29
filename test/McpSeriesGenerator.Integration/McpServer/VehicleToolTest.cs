using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace McpSeriesGenerator.Integration.McpServer
{
    public class VehicleToolTest : IClassFixture<ProcessFixture>
    {
        private readonly ProcessFixture _fixture;

        public VehicleToolTest(ProcessFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task MCPServer_ShouldResponseToGetVehiclesWithCheckDigitCreated()
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
                    name = "GetVehiclesWithCheckDigitCreated",
                    arguments = new { request = new { vehicleType = "C" } }
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

        [Fact]
        public async Task MCPServer_ShouldResponseToGetVehiclesWithValidatedSerialNumberAndCheckDigit()
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
                    name = "GetVehiclesWithValidatedSerialNumberAndCheckDigit",
                    arguments = new { }
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

        [Fact]
        public async Task MCPServer_ShouldResponseToTotalVehiclesByCountry()
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
                    name = "TotalVehiclesByCountry",
                    arguments = new { }
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

        [Fact]
        public async Task MCPServer_ShouldResponseToReturnsIfTheSerialNumberIsValid()
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
                    name = "ReturnsIfTheSerialNumberIsValid",
                    arguments = new { SerialNumber = "0202ARGXXC2614-E" }
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
