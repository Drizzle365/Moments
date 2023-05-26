using Flurl.Http;
using Moments.Model;

namespace Moments.Service;

/// <summary>
/// 朋友服务
/// </summary>
public class FriendService
{
    private readonly IFreeSql _db;
    private readonly ConfigService _configService;

    public FriendService(IFreeSql db, ConfigService configService)
    {
        _db = db;
        _configService = configService;
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

    public bool Del(Friend item)
    {
        var rows = _db.Delete<Friend>()
            .Where(x => x.FriendId == item.FriendId)
            .ExecuteAffrows();
        return rows > 0;
    }
    

    public async void VerifyItem(Friend target)
    {
        if (target.VerifyUrl is not null)
        {
            try
            {
                await target.VerifyUrl.GetStringAsync();
            }
            catch
            {
                await _db.Update<Friend>().Where(x => x.FriendId == target.FriendId)
                    .Set(x => x.Verify, FriendState.无法访问).ExecuteAffrowsAsync();
                return;
            }

            var temp = await target.VerifyUrl.GetStringAsync();
            if (temp.IndexOf(_configService.Get("Origin"), StringComparison.Ordinal) != -1)
            {
                await _db.Update<Friend>().Where(x => x.FriendId == target.FriendId).Set(x => x.Verify, FriendState.正常)
                    .ExecuteAffrowsAsync();
            }
            else
            {
                await _db.Update<Friend>().Where(x => x.FriendId == target.FriendId).Set(x => x.Verify, FriendState.失链)
                    .ExecuteAffrowsAsync();
            }
        }
    }
}