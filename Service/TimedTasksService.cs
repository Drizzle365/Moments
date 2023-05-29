using System.Timers;

namespace Moments.Service;

public class TimedTasksService
{
    private readonly GatherService _gatherService;
    private readonly ILogger<TimedTasksService> _logger;

    public TimedTasksService(GatherService gatherService, ILoggerFactory logger)
    {
        _gatherService = gatherService;
        _logger = logger.CreateLogger<TimedTasksService>();
    }

    public void Start(double interval)
    {
        var timer = new System.Timers.Timer();
        timer.Enabled = true;
        timer.Interval = interval;
        timer.Start();
        timer.Elapsed += Gather!;
    }

    private void Gather(object source, ElapsedEventArgs e)
    {
        _logger.LogInformation(DateTime.Now + " : 开始采集任务");
        _gatherService.GatherRssAll();
    }
}