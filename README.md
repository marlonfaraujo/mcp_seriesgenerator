This is a repository for testing with Model Context Protocol

### Instructions for executing the project

#### Running with docker:

1. Access the root directory folder in the terminal, example:
```bash
cd mcp_seriesgenerator
```

2. Container execution:
```bash
docker-compose up -d
```

3. Then run the below powershell script command, the docker.ps1 file is located in the root directory (The script will copy the files from the docker container to your local machine)
```bash
powershell -ExecutionPolicy ByPass -File docker.ps1
```

#### Claude desktop app configuration:

1. Open Claude desktop

2. Menu - File - Settings - Developer - Edit Config

3. Open config file in location: \user\AppData\Roaming\Claude\claude_desktop_config.json (Save the file, and restart Claude for Desktop)
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

4. Look for the "Search & Tools" icon, type something like Upload Vehicle and insert the file with the serial numbers. Example:
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

5. More information in: https://modelcontextprotocol.io/quickstart/server

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
