using System.Collections.Generic;
using System.Linq;

namespace Leetcode1242WebCrawler
{
    public class TestCase
    {
        private readonly Dictionary<string, List<string>> adj;

        public TestCase(string[] urls, int[][] edges, string startUrl, string[] output)
        {
            this.StartUrl = startUrl;
            this.StartUrl = startUrl;
            this.Output = output.OrderBy(x => x).ToList();

            this.adj = new Dictionary<string, List<string>>();

            for (int i = 0; i < urls.Length; i++)
            {
                this.adj.Add(urls[i], new List<string>());
            }

            foreach (var edge in edges)
            {
                this.adj[urls[edge[0]]].Add(urls[edge[1]]);
            }
        }

        public string StartUrl { get; }
        public List<string> Output { get; }

        public List<string> GetUrlsReachableFrom(string url)
        {
            return this.adj[url].ToList();
        }
    }
}
