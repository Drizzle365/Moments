using Microsoft.AspNetCore.Mvc;
using Moments.Model;
using Moments.Service;

namespace Moments.Controllers;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IFreeSql _db;
    private readonly FriendService _friendService;

    public ApiController(IFreeSql db, FriendService friendService)
    {
        _db = db;
        _friendService = friendService;
    }

    /// <summary>
    /// 获取朋友信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("friends")]
    public async Task<ActionResult<List<Friend>>> Friends()
    {
        var temp = await _db.Select<Friend>().Where(x => x.Visible == true)
            .ToListAsync();
        var random = new Random();
        var friends = new List<Friend>();
        foreach (var item in temp)
        {
            friends.Insert(random.Next(friends.Count), item);
        }

        return friends;
    }

    /// <summary>
    /// 获取文章信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("articles")]
    public async Task<ActionResult<List<Article>>> Articles()
    {
        return await _db.Select<Article>()
            .ToListAsync();
    }

    /// <summary>
    /// 申请友链
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public ActionResult<bool> Apply(Friend self)
    {
        _friendService.Add(self);
        return true;
    }
}