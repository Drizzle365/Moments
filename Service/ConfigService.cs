using Moments.Model;

namespace Moments.Service;

public class ConfigService
{
    private readonly IFreeSql _db;
    private readonly Dictionary<string, string?> _configData = new Dictionary<string, string?>();

    public ConfigService(IFreeSql db)
    {
        _db = db;
        UpdateConfig();
    }

    private void UpdateConfig()
    {
        var configs = _db.Select<Config>().ToList();
        _configData.Clear();
        foreach (var item in configs)
        {
            _configData.Add(item.Key, item.Value);
        }
    }

    public string Get(string key)
    {
        return _configData[key] ?? "";
    }

    public bool Set(string key, string? value)
    {
        var rows = _db.Update<Config>()
            .Where(x => x.Key == key)
            .Set(x => x.Key, key)
            .Set(x => x.Value, value)
            .ExecuteAffrows();
        UpdateConfig();
        return rows > 0;
    }
}