using System;
using System.Collections.Generic;
using System.Threading;

namespace Leetcode1242WebCrawler
{
    public class FakeHtmlParser : IHtmlParser
    {
        private readonly Random delayGenerator;
        private readonly TestCase testCase;
        private readonly Dictionary<string, List<string>> adj;

        public FakeHtmlParser(int seed, TestCase testCase)
        {
            this.delayGenerator = new Random(seed);
            this.testCase = testCase;
        }

        public List<string> GetUrls(string url)
        {
            var delay = this.delayGenerator.Next(0, 16);
            if (delay > 0)
            {
                Thread.Sleep(delay);
            }

            return this.testCase.GetUrlsReachableFrom(url);
        }
    }
}
