using Moments.Model;

namespace Moments.Service;

public class ArticleService
{
    private readonly IFreeSql _db;

    public ArticleService(IFreeSql db)
    {
        _db = db;
    }

    /// <summary>
    /// 异步获取文章总数
    /// </summary>
    /// <returns></returns>
    public async Task<long> CountAsync()
    {
        return await _db.Select<Article>().CountAsync();
    }

    /// <summary>
    /// 同步获取文章总数
    /// </summary>
    /// <returns></returns>
    public long Count()
    {
        return _db.Select<Article>().Count();
    }

    /// <summary>
    /// 异步获取文章
    /// </summary>
    /// <param name="friendId">朋友ID</param>
    /// <param name="page">页数</param>
    /// <param name="size">单页大小</param>
    /// <returns></returns>
    public async Task<List<Article>> ListAsync(int? friendId = null, int page = 1, int size = 10)
    {
        return friendId is not null
            ? await _db.Select<Article>()
                .Where((a) => a.FriendId == friendId)
                .OrderBy("PubDate DESC")
                .Offset((page - 1) * size)
                .Limit(size)
                .ToListAsync()
            : await _db.Select<Article>()
                .OrderBy("PubDate DESC")
                .Offset((page - 1) * size)
                .Limit(size)
                .ToListAsync();
    }
    /// <summary>
    /// 异步获取文章和朋友连接后的数据
    /// </summary>
    /// <param name="friendId"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public async Task<List<Dictionary<string, string?>>> FriendWithArticle(int? friendId = null, int page = 1,
        int size = 10)
    {
        var dbData = friendId is not null
            ? await _db.Select<Article, Friend>()
                .Where((a, f) => a.FriendId == f.FriendId && f.FriendId == friendId)
                .OrderBy("PubDate DESC")
                .Offset((page - 1) * size)
                .Limit(size)
                .ToListAsync((a, f) => new { a, f })
            : await _db.Select<Article, Friend>()
                .Where((a, f) => a.FriendId == f.FriendId)
                .OrderBy("PubDate DESC")
                .Offset((page - 1) * size)
                .Limit(size)
                .ToListAsync((a, f) => new { a, f });

        var ret = new List<Dictionary<string, string?>>();
        foreach (var item in dbData)
        {
            var temp = Utils.ObjectToDictionary(item.a);
            temp.Add("SiteName", item.f.Name);
            temp.Add("SiteLink", item.f.Link);
            ret.Add(temp);
        }

        return ret;
    }
}