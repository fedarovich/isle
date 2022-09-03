using System.Reflection;
using BenchmarkDotNet.Attributes;
using Isle.Configuration;
using Serilog;
using Serilog.Events;

namespace Isle.Serilog.Benchmarks;

[MemoryDiagnoser]
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

    [GlobalSetup(Target = nameof(Standard))]
    public void GlobalSetupStandard()
    {
        GlobalSetup();
    }

    [GlobalSetup(Targets = new[] { nameof(InterpolatedWithManualDestructuring), nameof(Interpolated2WithManualDestructuring) /*, nameof(InterpolatedWithLiteralValue) */})]
    public void GlobalSetupWithManualDestructuring()
    {
        GlobalSetup();
        IsleConfiguration.Configure(builder => builder
            .ConfigureSerilog(cfg => cfg.EnableMessageTemplateCaching = EnableCaching));
    }

    [GlobalSetup(Targets = new[] { nameof(InterpolatedWithExplicitAutomaticDestructuring), nameof(Interpolated2WithExplicitAutomaticDestructuring), nameof(InterpolatedWithImplicitAutomaticDestructuring), nameof(Interpolated2WithImplicitAutomaticDestructuring) })]
    public void GlobalSetupWithAutoDestructuring()
    {
        GlobalSetup();
        IsleConfiguration.Configure(builder => builder.WithAutomaticDestructuring()
            .ConfigureSerilog(cfg => cfg.EnableMessageTemplateCaching = EnableCaching));
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
        IsleConfiguration.Reset();
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

    //[Benchmark]
    //public void InterpolatedWithLiteralValue()
    //{
    //    _logger.InformationInterpolated2($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {(LiteralValue) Perimeter.ToString()}.");
    //}

    [Benchmark]
    public void Interpolated2WithManualDestructuring() {
        _logger.InformationInterpolated2($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    [Benchmark]
    public void Interpolated2WithExplicitAutomaticDestructuring() {
        _logger.InformationInterpolated2($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    [Benchmark]
    public void Interpolated2WithImplicitAutomaticDestructuring() {
        _logger.InformationInterpolated2($"The area of rectangle {Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {Perimeter}.");
    }

    //[Benchmark]
    //public void Interpolate2dWithLiteralValue() {
    //    _logger.InformationInterpolated2($"The area of rectangle {@Rect} is Width * Height = {Area} and its perimeter is 2 * (Width + Height) = {(LiteralValue)Perimeter.ToString()}.");
    //}

}