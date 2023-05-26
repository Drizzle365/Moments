using Moments.Model;

#pragma warning disable CS4014

namespace Moments.Service;

/// <summary>
/// 采集服务
/// </summary>
public class GatherService
{
    private readonly IFreeSql _db;

    public GatherService(IFreeSql db)
    {
        _db = db;
    }

    /// <summary>
    /// 单个朋友站点采集
    /// </summary>
    /// <param name="target">目标朋友</param>
    /// <returns>采集日志</returns>
    public async Task<GatherLog?> GatherRssItem(Friend target)
    {
        var cnt = 0;
        var res = await Rss.GetRss(target.Rss, target.Rule, target.FriendId);
        if (res.Message is null && res.Data.Count == 0)
        {
            return null;
        }

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

    /// <summary>
    /// 全部站点采集
    /// </summary>
    public void GatherRssAll()
    {
        var friends = _db.Select<Friend>().Where(x => x.Visible == true).ToList();
        foreach (var item in friends)
        {
            GatherRssItem(item);
        }
    }
}