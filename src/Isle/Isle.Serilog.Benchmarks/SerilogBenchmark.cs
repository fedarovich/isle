using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Isle.Configuration;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 1, exportHtml: true)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net48)]
public class SerilogBenchmark
{
    private ILogger _logger = null!;

    private static readonly Rect Rect = new(0, 0, 3, 4);
    private static readonly int Area = Rect.Width * Rect.Height;
    private static readonly int Perimeter = 2 * (Rect.Width + Rect.Height);

    [ParamsAllValues]
    public bool IsEnabled { get; set; } = true;

    [ParamsAllValues]
    public bool EnableCaching { get; set; }

    [ParamsAllValues]
    public bool IsResettable { get; set; }

    [GlobalSetup(Target = nameof(Standard))]
    public void GlobalSetupStandard()
    {
        GlobalSetup();
    }

    [GlobalSetup(Targets = new[] { nameof(InterpolatedWithManualDestructuring), nameof(InterpolatedWithLiteralValue) })]
    public void GlobalSetupWithManualDestructuring()
    {
        GlobalSetup();
        IsleConfiguration.Configure(builder => builder.IsResettable(IsResettable)
            .AddSerilog(cfg => cfg.EnableMessageTemplateCaching = EnableCaching));
    }

    [GlobalSetup(Targets = new[] { nameof(InterpolatedWithExplicitAutomaticDestructuring), nameof(InterpolatedWithImplicitAutomaticDestructuring) })]
    public void GlobalSetupWithAutoDestructuring()
    {
        GlobalSetup();
        IsleConfiguration.Configure(builder => builder.IsResettable(IsResettable).WithAutomaticDestructuring()
            .AddSerilog(cfg => cfg.EnableMessageTemplateCaching = EnableCaching));
    }

    private void GlobalSetup()
    {
        var minLogEventLevel = IsEnabled ? LogEventLevel.Information : LogEventLevel.Error;
        _logger = new LoggerConfiguration()
            .WriteTo.BenchmarkSink()
            .MinimumLevel.Is(minLogEventLevel)
            .CreateLogger();
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        (_logger as IDisposable)?.Dispose();
    }

    [Benchmark(Baseline = true)]
    public void Standard()
    {
        _logger.Information("The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.", Rect, Area, Perimeter);
    }

    [Benchmark]
    public void InterpolatedWithManualDestructuring()
    {
        _logger.InformationInterpolated($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    [Benchmark]
    public void InterpolatedWithExplicitAutomaticDestructuring()
    {
        _logger.InformationInterpolated($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    [Benchmark]
    public void InterpolatedWithImplicitAutomaticDestructuring()
    {
        _logger.InformationInterpolated($"The area of rectangle {Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    [Benchmark]
    public void InterpolatedWithLiteralValue()
    {
        _logger.InformationInterpolated($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {(LiteralValue)Perimeter.ToString()}.");
    }
}