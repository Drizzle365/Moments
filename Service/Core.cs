using Moments.Model;

namespace Moments.Service;

public class Core
{
    private readonly IFreeSql _db;

    public Core(IFreeSql db)
    {
        _db = db;
    }


    public async Task<GatherLog> GatherRssItem(Friend target)
    {
        var cnt = 0;
        var res = await Rss.GetRss(target.Rss, target.Rule);
        foreach (var article in res.Data)
        {
            try
            {
                await _db.Insert<Article>().AppendData(article).ExecuteAffrowsAsync();
                cnt++;
            }
            catch
            {
                break;
            }
        }

        var temp = new GatherLog
        {
            Name = target.Name,
            Url = target.Rss,
            Num = cnt,
            DateTime = DateTime.Now,
            Error = res.Message
        };
        await _db.Insert<GatherLog>().AppendData(temp).ExecuteAffrowsAsync();
        return temp;
    }

    protected void GatherRssAll()
    {
        var friends = _db.Select<Friend>().ToList();
        foreach (var item in friends)
        {
            GatherRssItem(item);
        }
    }
}