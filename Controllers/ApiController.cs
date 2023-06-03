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
    private readonly ArticleService _articleService;


    public ApiController(IFreeSql db, FriendService friendService, ArticleService articleService)
    {
        _db = db;
        _friendService = friendService;
        _articleService = articleService;
    }

    /// <summary>
    /// 获取朋友信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("friends")]
    public async Task<ActionResult<List<Friend>>> Friends()
    {
        var temp = await _friendService.ListAsync(true);
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
    public async Task<ActionResult<object>> Articles(int? friendId = null, int page = 1, int size = 10)
    {
        return await _articleService.ListAsync(friendId, page, size);
    }
}