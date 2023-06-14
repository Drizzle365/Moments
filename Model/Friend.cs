using FreeSql.DataAnnotations;

namespace Moments.Model;

public enum Rule
{
    Rss,
    Atom,
    Other
}

public enum FriendType
{
    隐藏,
    友邻,
    关注
}

public class Friend
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int FriendId { get; set; }

    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public string? Email { get; set; }
    public string? Link { get; set; }
    public string? Feed { get; set; }
    public Rule Rule { get; set; } = Rule.Rss;
    public FriendType FriendType { get; set; } = FriendType.友邻;
}