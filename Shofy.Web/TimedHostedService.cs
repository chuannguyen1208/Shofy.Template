using MediatR;
using Shofy.UseCases.Documents.Commands;

namespace Shofy.Web;

public class TimedHostedService : BackgroundService
{
    private readonly ILogger<TimedHostedService> _logger;
    private int _executionCount;

    public TimedHostedService(IServiceProvider services, ILogger<TimedHostedService> logger)
    {
        Services = services;
        _logger = logger;
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        await DoWork();

        using PeriodicTimer timer = new(TimeSpan.FromMinutes(1));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
        }
    }

    private async Task DoWork()
    {
        using (var scope = Services.CreateScope())
        {
            var mediator =
                scope.ServiceProvider
                    .GetRequiredService<IMediator>();

            await mediator.Send(new ProcessUploadedDocumentsCommand());
        }
        await Task.CompletedTask;
    }
}
