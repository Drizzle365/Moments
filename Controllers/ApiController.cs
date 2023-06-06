using Microsoft.AspNetCore.Mvc;
using Moments.Model;
using Moments.Service;

namespace Moments.Controllers;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly FriendService _friendService;
    private readonly ArticleService _articleService;

    public ApiController(FriendService friendService, ArticleService articleService)
    {
        _friendService = friendService;
        _articleService = articleService;
    }

    /// <summary>
    /// 获取朋友信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("friends")]
    public async Task<ActionResult<List<Friend>>> Friends(bool? isVis = null)
    {
        return await _friendService.ListAsync(isVis);
    }

    /// <summary>
    /// 获取文章信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("articles")]
    public async Task<ActionResult<object>> Articles(int? friendId = null, int page = 1, int size = 10)
    {
        return await _articleService.FriendWithArticle(friendId, page, size);
    }

    [HttpGet("articles/count")]
    public async Task<ActionResult<long>> ArticleCount()
    {
        return await _articleService.CountAsync();
    }

    [HttpGet("friends/count")]
    public async Task<ActionResult<long>> FriendCount()
    {
        return await _friendService
            .CountAsync(true);
    }
}