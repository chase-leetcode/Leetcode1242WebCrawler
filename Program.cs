using System;
using System.Linq;

namespace Leetcode1242WebCrawler
{
    /// <summary>
    /// https://leetcode.jp/problemdetail.php?id=1242
    /// </summary>
    class Program
    {
        private const int delayGeneratorSeed = 0;

        static void Main(string[] args)
        {
            var testCases = new[] { TestCase1(), TestCase2(), TestCase3() };
            for (int i = 0; i < testCases.Length; i++)
            {
                var testCase = testCases[i];
                Console.WriteLine($"Test case {i}");
                Console.WriteLine("==========================");

                var parser = new FakeHtmlParser(delayGeneratorSeed, testCase);
                var urls = new TaskPoolWebCrawler(parser).Run(testCase.StartUrl).OrderBy(x => x);

                if (string.Join("\n", urls) != string.Join("\n", testCase.Output))
                {
                    throw new Exception("Incorrect result");
                }

                Console.WriteLine(string.Join(Environment.NewLine, urls));
            }
        }

        static TestCase TestCase1()
        {
            return new TestCase(new[]
            {
                "http://news.yahoo.com",
                "http://news.yahoo.com/news",
                "http://news.yahoo.com/news/topics/",
                "http://news.google.com",
                "http://news.yahoo.com/us"
            },
            new[] { new[] { 2, 0 }, new[] { 2, 1 }, new[] { 3, 2 }, new[] { 3, 1 }, new[] { 0, 4 } },
            "http://news.yahoo.com/news/topics/",
            new[]
            {
                "http://news.yahoo.com",
                "http://news.yahoo.com/news",
                "http://news.yahoo.com/news/topics/",
                "http://news.yahoo.com/us"
            }
            );
        }

        static TestCase TestCase2()
        {
            // Explanation: The startUrl links to all other pages that do not share the same hostname.
            return new TestCase(new[]
            {
                "http://news.yahoo.com",
                "http://news.yahoo.com/news",
                "http://news.yahoo.com/news/topics/",
                "http://news.google.com"
            },
            new[] { new[] { 0, 2 }, new[] { 2, 1 }, new[] { 3, 2 }, new[] { 3, 1 }, new[] { 3, 0 } },
            "http://news.google.com",
            new[] { "http://news.google.com" }
            );
        }

        static TestCase TestCase3()
        {
            // Explanation: The startUrl links to all other pages that do not share the same hostname.
            return new TestCase(new[]
            {
                "http://news.google.com"
            },
            new int[0][],
            "http://news.google.com",
            new[] { "http://news.google.com" }
            );
        }
    }
}
