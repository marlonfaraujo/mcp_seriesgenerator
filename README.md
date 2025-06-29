This is a repository for testing with Model Context Protocol

### Instructions for executing the project

#### Running with docker:

**1.** Access the root directory folder in the terminal, example:
```bash
cd mcp_seriesgenerator
```

**2.** Container execution:
```bash
docker-compose up -d
```

**3.** Then run the below powershell script command, the docker.ps1 file is located in the root directory (The script will copy the files from the docker container to your local machine)
```bash
powershell -ExecutionPolicy ByPass -File docker.ps1
```

#### Running with Claude desktop app:

**1.** Open Claude desktop app for configuration

**2.** Menu - File - Settings - Developer - Edit Config

**3.** Open config file in location: \user\AppData\Roaming\Claude\claude_desktop_config.json (Save the file, and restart Claude for Desktop)
```json
{
    "mcpServers": {
    "serialNumberGenerator": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "C:\\ABSOLUTE\\PATH\\TO\\PROJECT",
        "--no-build"
      ]
    }
  }
}
```

**4.** Look for the "Search & Tools" icon, type something like Upload Vehicle and insert the file with the serial numbers. Example:
```text
series.txt
1313MEXXXA7989-1
0708BRAXXC4014-3
1414ARGXXA5834-9
1213ASMXXC8348-2
0202ARGXXC2614-E
0606BRAXXA6466-8
0606MEXXXA3820-4
```

**5.** More information in: https://modelcontextprotocol.io/quickstart/server

#### Tests with Mcp Server without client applications:

**1.** The Model Context Protocol (MCP) allows servers to expose tools that can be invoked externally in a standardized way.

**1.1.** First, it is necessary to configure the mcp server and its tools that will be exposed in the application. Here we configure with StdioServerTransport:
```csharp
//class src/McpSeriesGenerator.App/Program.cs
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<VehicleTool>();
builder.Services.AddScoped<VehicleTool>();
```

**1.2.** In the project, there are resources created for the mcp Server tool. Among some of them, we have, for example, ReturnsIfTheSerialNumberIsValid.
```csharp
//class src/McpSeriesGenerator.App/McpServer/VehicleTool.cs
[McpServerTool(Name = "ReturnsIfTheSerialNumberIsValid"), Description("Validate serial number with check digit.")]
public string ReturnsIfTheSerialNumberIsValid(
    [Description("Enter a serial number with check digit")] string SerialNumber)
{
    if (string.IsNullOrWhiteSpace(SerialNumber))
    {
        return "Serial number cannot be empty.";
    }
    var vehicle = Vehicle.Create(SerialNumber);
    if (vehicle.VehicleSerialNumber.ValidateCheckDigit())
    {
        return "This serial number is valid";
    }
    return "This serial number is invalid";
}
```

**2.** In the integration testing project, there is communication with the Mcp Server.

**2.1.** In the following code, in the constructor method, a process instance is created to run the mcp server project externally from the integration test project.
```csharp
//class test/McpSeriesGenerator.Integration/McpServer/ProcessFixture.cs
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
```

**2.2.** Example test method with the ReturnsIfTheSerialNumberIsValid tool:
```csharp
//class test/McpSeriesGenerator.Integration/McpServer/VehicleToolTest.cs
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
```

### Technologies and tools used:
* **General**:
   1. [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) - .NET SDK 9;
* **Testing**:
   1. [XUnit](https://xunit.net/) - Unit tests to ensure code stability;
* **Container Technology**:
   1. [Docker](https://www.docker.com/) - Containerizing the application to facilitate testing;
* **Tools**:
   1. [MCP](https://github.com/modelcontextprotocol/csharp-sdk) - Model context protocol (Version: 0.3.0-preview.1);
   2. [Claude](https://claude.ai/download) - Claude desktop app for client MCP (Windows Version: 0.10.38).