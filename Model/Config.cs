using FreeSql.DataAnnotations;

namespace Moments.Model;

public class Config
{
    [Column(IsPrimary = true)] public string Key { get; set; }
    public string? Value { get; set; }
}