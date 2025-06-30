using McpSeriesGenerator.App.Domain.Entities;
using McpSeriesGenerator.App.Infra.Data;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.Text.Json;

namespace McpSeriesGenerator.App.McpServer
{
    public class UploadVehicleTool : McpServerTool
    {
        public override Tool ProtocolTool => new Tool
        {
            Name = "UploadVehicleWithSerialNumber",
            Description = "Upload vehicles with serial number.",
            InputSchema = JsonSerializer.Deserialize<JsonElement>("""
                                {
                                    "type": "object",
                                    "properties": {
                                      "file": {
                                        "type": "string",
                                        "format": "binary",
                                        "description": "File content"
                                      },
                                      "fileName": {
                                        "type": "string",
                                        "description": "Name of the file to be processed"
                                      }
                                    },
                                    "required": ["file","fileName"]
                                }
                                """)
        };

        public override async ValueTask<CallToolResult> InvokeAsync(RequestContext<CallToolRequestParams> request, CancellationToken cancellationToken = default)
        {
            try
            {
                var @params = request.Params;
                var arguments = @params?.Arguments;

                if (arguments == null)
                {
                    var message = JsonSerializer.Serialize(new { error = "Arguments are null.", message = @params });
                    return new CallToolResult
                    {
                        IsError = true,
                        Content = [new TextContentBlock { Text = message }]
                    };
                }
                var fileName = arguments.GetValueOrDefault("fileName").ToString();
                var file = arguments.GetValueOrDefault("file").ToString();
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    var message = JsonSerializer.Serialize(new { error = "Not found file name.", message = @params });
                    return new CallToolResult
                    {
                        IsError = true,
                        Content = [new TextContentBlock { Text = message }]
                    };
                }
                if (string.IsNullOrWhiteSpace(file))
                {
                    var message = JsonSerializer.Serialize(new { error = "File parameter not found in request.", message = @params });
                    return new CallToolResult
                    {
                        IsError = true,
                        Content = [new TextContentBlock { Text = message }]
                    };
                }
                var vehicles = await UploadVehiclesWithSerialNumber(file, fileName);
                return GetResponseSuccessfully();

                CallToolResult GetResponseSuccessfully()
                {
                    var content = string.Join("\n--\n", vehicles.Select(vehicle =>
                    {
                        return $"""
                            SerialNumber: {vehicle.VehicleSerialNumber.GetValueWithCheckDigit()} 
                            YearOfManufacture: {vehicle.YearOfManufacture} 
                            ModelYear: {vehicle.ModelYear} 
                            VehicleType: {vehicle.VehicleType} 
                            AcronymCountryOfManufacture: {vehicle.AcronymCountryOfManufacture}
                            IsValid: {vehicle.VehicleSerialNumber.ValidateCheckDigit()}
                        """;
                    }));
                    var message = JsonSerializer.Serialize(new { success = "File processed successfully.", content = content });
                    return new CallToolResult()
                    {
                        IsError = false,
                        Content = [new TextContentBlock { Text = message }]
                    };
                }

            }
            catch (Exception ex)
            {
                var message = JsonSerializer.Serialize(new { error = "Error file process.", message = ex.Message, parameters = request.Params });
                return new CallToolResult
                {
                    IsError = true,
                    Content = [new TextContentBlock { Text = message }]
                };
            }
        }

        private async Task<IEnumerable<Vehicle>> UploadVehiclesWithSerialNumber(string fileContent, string fileName, CancellationToken cancellationToken = default)
        {
            var vehicles = new List<Vehicle>();
            string filePath = Path.Combine(AppContext.BaseDirectory, "artifacts", fileName);
            if (!FileService.FileValidate(fileContent, fileName, ".txt"))
            {
                throw new InvalidOperationException("Invalid file format.");
            }
            var lines = fileContent.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                vehicles.Add(Vehicle.Create(line));
            }
            await FileService.WriteAllLinesAsync(filePath, lines, cancellationToken);
            return vehicles;
        }
    }
}
