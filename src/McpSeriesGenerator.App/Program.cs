using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.UseCases;
using McpSeriesGenerator.App.Infra.Data;
using McpSeriesGenerator.App.McpServer;
using McpSeriesGenerator.App.Shared.Abstractions;
using McpSeriesGenerator.App.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;

var builder = new HostApplicationBuilder(args);
    builder.Configuration.SetBasePath(AppContext.BaseDirectory);
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole(consoleLogOptions =>
    {
        // Configure all logs to go to stderr
        consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
    });

    builder.Services.AddScoped<IVehicleData, VehicleDataFile>();
    builder.Services.AddScoped<IVehicleData, VehicleDataFile>();
    builder.Services.AddScoped<ICountryData, CountryDataFile>();
    builder.Services.AddScoped<GetSerialNumberWithoutCheckDigit>();
    builder.Services.AddScoped<GenerateCheckDigit>();
    builder.Services.AddScoped<GetSerialNumberToValidate>();
    builder.Services.AddScoped<ValidateSerialNumber>();
    builder.Services.AddScoped<GetSerialNumberForReport>();
    builder.Services.AddScoped<CalculateTotalByCountrySorted>();
    builder.Services.AddScoped<GetCountryItems>();
    builder.Services.AddMcpServer()
        .WithStdioServerTransport()
        .WithTools<VehicleTool>();
    //.WithToolsFromAssembly();
    builder.Services.AddScoped<VehicleTool>();
    builder.Services.AddScoped<McpServerTool, UploadVehicleTool>();
    builder.Services.Configure<ArtifactConfig>(
        builder.Configuration.GetSection("Artifact"));
    builder.Services.AddSingleton<IArtifactConfig>(serviceProvider =>
        serviceProvider.GetRequiredService<IOptions<ArtifactConfig>>().Value);

try
{
    var app = builder.Build();
    await generateCheckDigitUseCase();
    await validateSerialNumberUseCase();
    await calculateTotalByCountrySortedUseCase();

    async Task generateCheckDigitUseCase()
    {
        var getSerialNumberWithoutCheckDigit = app.Services.GetRequiredService<GetSerialNumberWithoutCheckDigit>();
        var vehicles = await getSerialNumberWithoutCheckDigit.ExecuteAsync(CancellationToken.None);
        var generateCheckDigit = app.Services.GetRequiredService<GenerateCheckDigit>();
        await generateCheckDigit.ExecuteAsync(vehicles, CancellationToken.None);
    }
    async Task validateSerialNumberUseCase()
    {
        var getSerialNumberToValidate = app.Services.GetRequiredService<GetSerialNumberToValidate>();
        var vehicles = await getSerialNumberToValidate.ExecuteAsync(CancellationToken.None);
        var validateSerialNumber = app.Services.GetRequiredService<ValidateSerialNumber>();
        await validateSerialNumber.ExecuteAsync(vehicles, CancellationToken.None);
    }
    async Task calculateTotalByCountrySortedUseCase()
    {
        var getCountryItems = app.Services.GetRequiredService<GetCountryItems>();
        var countries = await getCountryItems.ExecuteAsync(CancellationToken.None);
        var getSerialNumberForReport = app.Services.GetRequiredService<GetSerialNumberForReport>();
        var vehicles = await getSerialNumberForReport.ExecuteAsync(CancellationToken.None);
        var calculateTotalByCountrySorted = app.Services.GetRequiredService<CalculateTotalByCountrySorted>();
        await calculateTotalByCountrySorted.ExecuteAsync(vehicles, countries, CancellationToken.None);
    }

    await app.RunAsync();

} 
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during host creation: {ex.Message}");
    return;
}