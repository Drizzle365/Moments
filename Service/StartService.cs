using Moments.Model;

namespace Moments.Service;

public class StartService
{
    private readonly IFreeSql _db;
    private readonly string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "moments.db");
    private readonly TimedTasksService _tasksService;
    private readonly ILogger<StartService> _logger;

    public StartService(IFreeSql db, TimedTasksService tasksService, ILoggerFactory logger)
    {
        _db = db;
        _tasksService = tasksService;
        _logger = logger.CreateLogger<StartService>();
        if (GetDbSize() == 0)
        {
            Install();
        }

        Start();
    }

    private long GetDbSize()
    {
        FileInfo fileInfo = new FileInfo(_dbPath);
        return fileInfo.Length;
    }

    private void Install()
    {
        _logger.LogInformation("开始迁移数据库");
        _db.CodeFirst.SyncStructure(typeof(Friend));
        _db.CodeFirst.SyncStructure(typeof(Article));
        _db.CodeFirst.SyncStructure(typeof(Config));
        _logger.LogInformation("开始初始化配置数据");
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Title", Value = "Moments" })
            .ExecuteAffrows();
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Origin", Value = "dearain.cn" })
            .ExecuteAffrows();
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Interval", Value = "3600000" })
            .ExecuteAffrows();
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Token", Value = "lantin" })
            .ExecuteAffrows();
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Name", Value = "时雨" })
            .ExecuteAffrows();
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Avatar", Value = "https://q1.qlogo.cn/g?b=qq&nk=396823203&s=100" })
            .ExecuteAffrows();
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Sentence", Value = "天行健，君子以自强不息。" })
            .ExecuteAffrows();
        _db.Insert<Config>()
            .AppendData(new Config { Key = "Banner", Value = "header.jpg" })
            .ExecuteAffrows();
        _logger.LogInformation("初始化配置数据完成");
        _logger.LogInformation("数据库结构迁移完成");
    }

    private void Start()
    {
        _logger.LogInformation("启动朋友圈服务");
        var interval = _db.Select<Config>().Where(x => x.Key == "Interval").First();
        if (interval.Value != null) _tasksService.Start(double.Parse(interval.Value));
    }
}