using FreeSql.DataAnnotations;

namespace Moments.Model;

public class GatherLog
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int GatherId { get; set; }

    public string? Name { get; set; }
    public string? Url { get; set; }
    public int Num { get; set; }
    public string? Error { get; set; }
    public DateTime DateTime { get; set; }
}