using System.Timers;

namespace Moments.Service;

public class TimedTasksService
{
    private readonly GatherService _gatherService;

    public TimedTasksService(GatherService gatherService)
    {
        _gatherService = gatherService;
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
        Console.WriteLine(DateTime.Now + " : 开始采集任务");
        _gatherService.GatherRssAll();
    }
}