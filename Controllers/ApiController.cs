using Microsoft.AspNetCore.Mvc;
using Moments.Model;

namespace Moments.Controllers;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    private readonly IFreeSql _db;

    public ApiController(IFreeSql db)
    {
        _db = db;
    }

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

    [HttpGet("articles")]
    public async Task<ActionResult<List<Article>>> Articles()
    {
        return await _db.Select<Article>()
            .ToListAsync();
    }
}