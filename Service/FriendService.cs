using System.Linq.Expressions;
using Moments.Model;

namespace Moments.Service;

/// <summary>
/// 朋友服务
/// </summary>
public class FriendService
{
    private readonly IFreeSql _db;

    public FriendService(IFreeSql db)
    {
        _db = db;
    }

    /// <summary>
    /// 获取朋友总数
    /// </summary>
    /// <param name="friendType">朋友类型</param>
    /// <returns></returns>
    public long Count(FriendType? friendType = null)
    {
        return friendType is not null
            ? _db.Select<Friend>().Where(x => x.FriendType == friendType).Count()
            : _db.Select<Friend>().Count();
    }

    /// <summary>
    /// 异步获取朋友总数
    /// </summary>
    /// <param name="friendType">朋友类型</param>
    /// <returns></returns>
    public async Task<long> CountAsync(FriendType? friendType = null)
    {
        return friendType is not null
            ? await _db.Select<Friend>().Where(x => x.FriendType == friendType).CountAsync()
            : await _db.Select<Friend>().CountAsync();
    }


    /// <summary>
    /// 获取一个朋友
    /// </summary>
    /// <param name="exp">过滤表达式</param>
    /// <returns></returns>
    public Friend? First(Expression<Func<Friend, bool>> exp)
    {
        return _db.Select<Friend>().Where(exp).First();
    }

    /// <summary>
    /// 获取所有朋友
    /// </summary>
    /// <param name="friendType">朋友类型</param>
    /// <returns></returns>
    public List<Friend> List(FriendType? friendType = null)
    {
        return friendType is not null
            ? _db.Select<Friend>().Where(x => x.FriendType == friendType).ToList()
            : _db.Select<Friend>().ToList();
    }

    /// <summary>
    /// 异步获取所有朋友
    /// </summary>
    /// <param name="friendType">朋友类型</param>
    /// <returns></returns>
    public async Task<List<Friend>> ListAsync(FriendType? friendType = null)
    {
        return friendType is not null
            ? await _db.Select<Friend>().Where(x => x.FriendType == friendType).ToListAsync()
            : await _db.Select<Friend>().ToListAsync();
    }


    /// <summary>
    /// 增加朋友记录
    /// </summary>
    /// <param name="item">朋友</param>
    /// <returns>是否添加成功</returns>
    public bool Add(Friend item)
    {
        var rows = _db.Insert<Friend>()
            .AppendData(item)
            .ExecuteAffrows();
        return rows > 0;
    }

    /// <summary>
    /// 编辑朋友
    /// </summary>
    /// <param name="item">朋友</param>
    /// <returns></returns>
    public bool Edit(Friend item)
    {
        var rows = _db.Update<Friend>()
            .SetSource(item)
            .ExecuteAffrows();
        return rows > 0;
    }

    /// <summary>
    /// 删除朋友
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Del(Friend item)
    {
        var rows = _db.Delete<Friend>()
            .Where(x => x.FriendId == item.FriendId)
            .ExecuteAffrows();
        return rows > 0;
    }

    /// <summary>
    /// 清空数据表
    /// </summary>
    /// <returns></returns>
    public int DelAll()
    {
        var rows = _db.Delete<Friend>()
            .Where("1=1")
            .ExecuteAffrows();
        return rows;
    }


    

    /// <summary>
    /// 导出数据
    /// </summary>
    /// <returns></returns>
    public async Task<string> Export()
    {
        var friends = await ListAsync();
        var path = $"wwwroot/export";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var exportFilePath = $"wwwroot/export/friends_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv"}";
        await using StreamWriter writer = new StreamWriter(exportFilePath);
        foreach (Friend item in friends)
        {
            await writer.WriteLineAsync(
                $"{item.Name},{item.Avatar},{item.Description}," +
                $"{item.Email},{item.Link},{item.Feed}," +
                $"{item.Rule},{item.FriendType}"
            );
        }

        return exportFilePath.Replace("wwwroot", "");
    }

    /// <summary>
    /// 导入数据
    /// </summary>
    /// <param name="file">文件</param>
    /// <returns></returns>
    public async Task<bool> Import(Stream file)
    {
        List<Friend> friends = new List<Friend>();
        using StreamReader reader = new StreamReader(file);
        while (await reader.ReadLineAsync() is { } line)
        {
            string[] data = line.Split(',');
            string name = data[0];
            string? avatar = data[1].Length == 0 ? null : data[1];
            string description = data[2];
            string email = data[3];
            string link = data[4];
            string feed = data[5];
            Rule rule = (Rule)Enum.Parse(typeof(Rule), data[6]);
            FriendType friendType = (FriendType)Enum.Parse(typeof(FriendType), data[7]);
            Friend friend = new Friend
            {
                Name = name,
                Avatar = avatar,
                Description = description,
                Email = email,
                Link = link,
                Feed = feed,
                Rule = rule,
                FriendType = friendType
            };
            friends.Add(friend);
        }

        foreach (var item in friends)
        {
            Add(item);
        }

        return true;
    }
}