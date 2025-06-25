using McpSeriesGenerator.App.Application.Abstractions;
using McpSeriesGenerator.App.Application.UseCases;
using McpSeriesGenerator.App.Infra.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<IVehicleData, VehicleDataFile>();
        services.AddScoped<ICountryData, CountryDataFile>();
        services.AddScoped<GetSerialNumberWithoutCheckDigit>();
        services.AddScoped<GenerateCheckDigit>();
        services.AddScoped<GetSerialNumberToValidate>();
        services.AddScoped<ValidateSerialNumber>();
        services.AddScoped<GetSerialNumberForReport>();
        services.AddScoped<CalculateTotalByCountrySorted>();
        services.AddScoped<GetCountryItems>();

    });

try
{
    var app = host.Build();
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
        var getSerialNumberForReport = app.Services.GetRequiredService<GetSerialNumberForReport>();
        var vehicles = await getSerialNumberForReport.ExecuteAsync(CancellationToken.None);
        var calculateTotalByCountrySorted = app.Services.GetRequiredService<CalculateTotalByCountrySorted>();
        await calculateTotalByCountrySorted.ExecuteAsync(vehicles, CancellationToken.None);
    }

    await app.RunAsync();

} 
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during host creation: {ex.Message}");
    return;
}