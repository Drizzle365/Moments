using FreeSql.DataAnnotations;

namespace Moments.Model;

public enum Rule
{
    Rss,
    Atom,
    Other
}

public enum FriendState
{
    未知,
    正常,
    失链,
    无法访问
}

public class Friend
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int FriendId { get; set; }

    public string? Name { get; set; }
    public string? Avatar { get; set; }

    public string? Info { get; set; }
    public string? Email { get; set; }
    public string? Link { get; set; }
    public string? Rss { get; set; }
    public Rule Rule { get; set; } = Rule.Rss;
    public string? VerifyUrl { get; set; }

    public FriendState Verify { get; set; } = FriendState.未知;
    public bool Visible { get; set; } = true;
}