using System.Collections.Generic;

namespace Leetcode1242WebCrawler
{
    public interface IHtmlParser
    {
        List<string> GetUrls(string url);
    }
}