using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Isle.Configuration;
using Isle.Extensions.Logging.Benchmarks.MEL;
using Microsoft.Extensions.Logging;

namespace Isle.Extensions.Logging.Benchmarks;

[MemoryDiagnoser]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net50)]
[SimpleJob(RuntimeMoniker.NetCoreApp31)]
[SimpleJob(RuntimeMoniker.Net48)]
public class MELBenchmarks
{
    private ILoggerFactory _loggerFactory = null!;
    private ILogger<MELBenchmarks> _logger = null!;

    private static readonly Rect Rect = new (0, 0, 3, 4);
    private static readonly int Area = Rect.Width * Rect.Height;
    private static readonly int Perimeter = 2 * (Rect.Width + Rect.Height);

    [ParamsAllValues]
    public bool IsEnabled { get; set; } = true;

    [ParamsAllValues]
    public bool EnableCaching { get; set; }

    [ParamsAllValues]
    public bool RenderMessage { get; set; }

    [GlobalSetup(Target = nameof(Standard))]
    public void GlobalSetupStandard()
    {
        GlobalSetup();
    }

    [GlobalSetup(Targets = new [] { nameof(InterpolatedWithManualDestructuring), nameof(InterpolatedWithNamed), nameof(InterpolatedWithLiteralValue) })]
    public void GlobalSetupWithManualDestructuring()
    {
        GlobalSetup();
        IsleConfiguration.Configure(builder => builder
            .ConfigureExtensionsLogging(cfg => cfg.EnableMessageTemplateCaching = EnableCaching));
    }

    [GlobalSetup(Targets = new[] { nameof(InterpolatedWithExplicitAutomaticDestructuring), nameof(InterpolatedWithImplicitAutomaticDestructuring) })]
    public void GlobalSetupWithAutoDestructuring()
    {
        GlobalSetup();
        IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring()
            .ConfigureExtensionsLogging(cfg => cfg.EnableMessageTemplateCaching = EnableCaching));
    }

    private void GlobalSetup()
    {
        _loggerFactory = LoggerFactory.Create(builder =>
        {
            var minLogLevel = IsEnabled ? LogLevel.Information : LogLevel.Error;
            builder
                .SetMinimumLevel(minLogLevel)
                .AddBenchmark(opt =>
                {
                    opt.MinLogLevel = minLogLevel;
                    opt.RenderMessage = RenderMessage;
                });
        });
        _logger = _loggerFactory.CreateLogger<MELBenchmarks>();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _loggerFactory.Dispose();
    }

    [Benchmark(Baseline = true)]
    public void Standard()
    {
        _logger.LogInformation("The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.", Rect, Area, Perimeter);
    }

    [Benchmark]
    public void InterpolatedWithManualDestructuring()
    {
        _logger.LogInformation($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    [Benchmark]
    public void InterpolatedWithNamed()
    {
        _logger.LogInformation($"The area of rectangle {Rect.Named("@Rect")} is Width * Height = {Area.Named("Area")} and its perimeter is 2 * (Width + Height) = {Perimeter.Named("Perimeter")}.");
    }

    [Benchmark]
    public void InterpolatedWithLiteralValue()
    {
        _logger.LogInformation($"The area of rectangle {new LiteralValue(Rect.ToString())} is Width * Height = {Area.Named("Area")} and its perimeter is 2 * (Width + Height) = {Perimeter.Named("Perimeter")}.");
    }

    [Benchmark]
    public void InterpolatedWithExplicitAutomaticDestructuring()
    {
        _logger.LogInformation($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    [Benchmark]
    public void InterpolatedWithImplicitAutomaticDestructuring()
    {
        _logger.LogInformation($"The area of rectangle {Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }
}