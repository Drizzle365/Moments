﻿using Moments.Model;

namespace Moments.Service;

public class ArticleService
{
    private readonly IFreeSql _db;

    public ArticleService(IFreeSql db)
    {
        _db = db;
    }

    /// <summary>
    /// 异步获取文章
    /// </summary>
    /// <param name="friendId">朋友ID</param>
    /// <param name="page">页数</param>
    /// <param name="size">单页大小</param>
    /// <returns></returns>
    public async Task<List<Article>> List(int? friendId = null, int page = 1, int size = 10)
    {
        return friendId is not null
            ? await _db.Select<Article>()
                .Where(x => x.FriendId == friendId)
                .Offset((page - 1) * size)
                .Limit(size)
                .ToListAsync()
            : await _db.Select<Article>()
                .Offset((page - 1) * size)
                .Limit(size)
                .ToListAsync();
    }
}