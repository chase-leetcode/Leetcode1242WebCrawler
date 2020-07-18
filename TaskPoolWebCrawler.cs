using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Leetcode1242WebCrawler
{
    public class TaskPoolWebCrawler
    {
        private readonly IHtmlParser parser;

        public TaskPoolWebCrawler(IHtmlParser parser)
        {
            this.parser = parser;
        }

        public List<string> Run(string rootUrl)
        {
            var rootHostname = GetHostname(rootUrl);
            var visited = new HashSet<string>(); // single reader, single writer
            var waitHandle = new ManualResetEvent(false); // single reader, multiple writers
            var workItems = new ConcurrentBag<string>() { rootUrl }; // single reader, multiple writers
            int inProgress = 0; // not going to roll over in a Leetcode test

            while (inProgress > 0 || workItems.Count > 0)
            {
                if (inProgress > 0 && workItems.Count == 0)
                {
                    waitHandle.WaitOne();
                    waitHandle.Reset();
                }

                while (workItems.TryTake(out var url))
                {
                    if (visited.Contains(url))
                    {
                        continue;
                    }

                    Interlocked.Increment(ref inProgress);
                    visited.Add(url);
                    Task.Run(() =>
                    {
                        ProcessUrl(url, rootHostname, workItems);
                        Interlocked.Decrement(ref inProgress);
                        waitHandle.Set();
                    });
                }
            }

            return visited.ToList();
        }

        private void ProcessUrl(string url, string rootHostname, ConcurrentBag<string> workItems)
        {
            WriteLine($"worker: ProcessUrl {url}");
            List<string> children;
            try
            {
                // GetUrls runs synchronously and takes up to 15 ms to complete
                children = this.parser.GetUrls(url);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"worker: Failed to get child URLs for URL {url ?? "null"} {e.ToString()}");
                return;
            }

            foreach (var child in children)
            {
                try
                {
                    if (GetHostname(child) != rootHostname)
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"worker: Failed to get hostname for URL {child ?? "null"} {e.ToString()}");
                    continue;
                }

                WriteLine($"worker: add work item {child}");
                workItems.Add(child);
            }
        }

        private string GetHostname(string url)
        {
            return string.Join("", url.Skip("http://".Length).TakeWhile(x => x != '/'));
        }

        private static void WriteLine(string message)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - {message}");
        }
    }
}