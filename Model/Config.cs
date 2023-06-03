using FreeSql.DataAnnotations;

namespace Moments.Model;

public class Config
{
#pragma warning disable CS8618
    [Column(IsPrimary = true)] public string Key { get; init; }
#pragma warning restore CS8618
    public string? Value { get; init; }
}