using System.Xml;
using Moments.Model;
using Flurl.Http;

namespace Moments;

public class RssResult
{
    public List<Article> Data { get; set; } = new();
    public string? Message { get; set; }
}

public static class Rss
{
    public static async Task<RssResult> GetRss(string? url, Rule rule)
    {
        if (url is null)
        {
            return new RssResult();
        }

        var xmlDoc = new XmlDocument();
        try
        {
            var rss = await url.GetStringAsync();
            xmlDoc.LoadXml(rss);
        }
        catch (Exception e)
        {
            return new RssResult
            {
                Message = e.ToString()
            };
        }

        var xmlNamespaceManager =
            new XmlNamespaceManager(xmlDoc.NameTable);
        List<Article> ret = new List<Article>();
        var siteName = xmlDoc.GetElementsByTagName("title")[0]?.InnerText;
        if (rule is Rule.Rss)
        {
            xmlNamespaceManager.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
            var items = xmlDoc.GetElementsByTagName("item");
            for (int i = 0; i < items.Count; i++)
            {
                ret.Add(
                    new Article
                    {
                        Title = items[i]?.SelectSingleNode("title")?.InnerText,
                        Link = items[i]?.SelectSingleNode("link")?.InnerText,
                        PubDate = DateTime.Parse(items[i]?.SelectSingleNode("pubDate")?.InnerText!),
                        Description = items[i]?.SelectSingleNode("description")?.InnerText,
                        Content = items[i]?.SelectSingleNode("content:encoded", xmlNamespaceManager)?.InnerText,
                        From = siteName
                    });
            }
        }

        if (rule is Rule.Atom)
        {
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

                ret.Add(
                    new Article
                    {
                        Title = titles[^i]?.InnerText,
                        Link = links[^i]!.Attributes!.GetNamedItem("href")!.Value,
                        PubDate = DateTime.Parse(published[^i]!.InnerText),
                        Description = summary[^i]?.InnerText,
                        Content = temp,
                        From = siteName
                    });
            }
        }

        return new RssResult
        {
            Data = ret
        };
    }
}