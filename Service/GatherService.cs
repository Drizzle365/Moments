using System.Text.RegularExpressions;
using System.Xml;
using Flurl.Http;
using Moments.Model;

#pragma warning disable CS4014

namespace Moments.Service;

/// <summary>
/// 采集服务
/// </summary>
public class GatherService
{
    private readonly IFreeSql _db;
    private readonly ILogger<GatherService> _logger;

    public GatherService(IFreeSql db, ILoggerFactory logger)
    {
        _db = db;
        _logger = logger.CreateLogger<GatherService>();
    }

    /// <summary>
    /// 单个朋友站点采集
    /// </summary>
    /// <param name="target">目标朋友</param>
    /// <returns>采集日志</returns>
    public async void GatherRssItem(Friend target)
    {
        var res = await GetFeed(target.Feed, target.Rule, target.FriendId);
        if (res.Count == 0)
        {
            return;
        }

        foreach (var article in res)
        {
            try
            {
                await _db.Insert<Article>().AppendData(article).ExecuteAffrowsAsync();
            }
            catch
            {
                break;
            }
        }
    }

    /// <summary>
    /// 全部站点采集
    /// </summary>
    public void GatherRssAll()
    {
        var friends = _db.Select<Friend>().ToList();
        foreach (var item in friends)
        {
            GatherRssItem(item);
        }
    }


    private async Task<List<Article>> GetFeed(string? url, Rule rule, int fid)
    {
        if (url is null)
        {
            return new();
        }

        if (rule == Rule.Rss)
        {
            return await GetRss(url, fid);
        }

        if (rule == Rule.Atom)
        {
            return await GetAtom(url, fid);
        }

        return new();
    }

    private async Task<List<Article>> GetRss(string url, int fid)
    {
        var xmlDoc = new XmlDocument();
        try
        {
            var html = await url.GetStringAsync();
            xmlDoc.LoadXml(html);
        }
        catch (Exception)
        {
            _logger.LogWarning("站点采集失败：" + url);
        }

        var xmlNamespaceManager =
            new XmlNamespaceManager(xmlDoc.NameTable);
        List<Article> ret = new List<Article>();
        xmlNamespaceManager.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
        var items = xmlDoc.GetElementsByTagName("item");
        for (int i = 0; i < items.Count; i++)
        {
            var innerText = items[i]?.SelectSingleNode("pubDate")?.InnerText;
            if (innerText != null)
                ret.Add(
                    new Article
                    {
                        Title = items[i]?.SelectSingleNode("title")?.InnerText,
                        Link = items[i]?.SelectSingleNode("link")?.InnerText,
                        PubDate = DateTime.Parse(innerText),
                        Description = ReplaceHtmlTag(items[i]?.SelectSingleNode("description")?.InnerText),
                        Content = items[i]?.SelectSingleNode("content:encoded", xmlNamespaceManager)?.InnerText,
                        FriendId = fid
                    });
            else
            {
                _logger.LogWarning("日期不存在：" + items[i]?.SelectSingleNode("link")?.InnerText);
            }
        }

        return ret;
    }

    private async Task<List<Article>> GetAtom(string url, int fid)
    {
        var xmlDoc = new XmlDocument();
        try
        {
            var html = await url.GetStringAsync();
            xmlDoc.LoadXml(html);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
        }

        var xmlNamespaceManager =
            new XmlNamespaceManager(xmlDoc.NameTable);
        List<Article> ret = new List<Article>();
        xmlNamespaceManager.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
        var titles = xmlDoc.GetElementsByTagName("title");
        var links = xmlDoc.GetElementsByTagName("link");
        var summary = xmlDoc.GetElementsByTagName("summary");
        var content = xmlDoc.GetElementsByTagName("content");
        var published = xmlDoc.GetElementsByTagName("published");
        for (int i = 1; i <= published.Count; i++)
        {
            var temp = summary[^i]?.InnerText;
            if (content.Count == summary.Count)
            {
                temp = content[^i]?.InnerText;
            }

            var innerText = published[^i]?.InnerText;
            if (innerText != null)
                ret.Add(
                    new Article
                    {
                        Title = titles[^i]?.InnerText,
                        Link = links[^i]!.Attributes!.GetNamedItem("href")!.Value,
                        PubDate = DateTime.Parse(innerText),
                        Description = ReplaceHtmlTag(summary[^i]?.InnerText),
                        Content = temp,
                        FriendId = fid
                    });
            else
            {
                _logger.LogWarning("日期不存在：" + url);
            }
        }

        return ret;
    }

    private static string ReplaceHtmlTag(string? html)
    {
        if (html is null)
        {
            return "";
        }

        if (string.IsNullOrEmpty(html)) return html;
        //删除脚本
        html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "");
        //删除标签
        var r = new Regex(@"<[^>]+>|<.*", RegexOptions.IgnoreCase);
        Match m;
        for (m = r.Match(html); m.Success; m = m.NextMatch())
        {
            html = html.Replace(m.Groups[0].ToString(), "");
        }

        return html.Trim();
    }

    public async Task<Dictionary<string, string?>> GetRssSiteInfo(string? url)
    {
        if (url is null)
        {
            return new();
        }

        var xmlDoc = new XmlDocument();
        try
        {
            var html = await url.GetStringAsync();
            xmlDoc.LoadXml(html);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
        }

        var title = xmlDoc.GetElementsByTagName("title");
        var link = xmlDoc.GetElementsByTagName("link");
        var id = xmlDoc.GetElementsByTagName("id");
        var description = xmlDoc.GetElementsByTagName("description");
        var subtitle = xmlDoc.GetElementsByTagName("subtitle");
        var dic = new Dictionary<string, string?>
        {
            { "title", title[0]?.InnerText },
            { "link", link[0]?.InnerText },
            { "id", id[0]?.InnerText },
            { "description", description[0]?.InnerText },
            { "subtitle", subtitle[0]?.InnerText }
        };
        return dic;
    }
}