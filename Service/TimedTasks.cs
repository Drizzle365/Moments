using System.Timers;

namespace Moments.Service;

public class TimedTasks : Core
{
    private IFreeSql _db;

    public TimedTasks(IFreeSql db) : base(db)
    {
        _db = db;
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
        GatherRssAll();
    }
}