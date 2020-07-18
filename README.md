# Leetcode 1242 (Web crawler)
https://leetcode.jp/problemdetail.php?id=1242

I can't run it on Leetcode, because I don't have Leetcode premium.
Be aware the problem specification provides only a synchronous GetUrls method.

## Run it
Using dotnet 3.1.201:

```
dotnet run
```

## How it works:
- Work item queue shared by all threads
- Single thread reads from the queue and spawns workers using Task.Run
  - This thread also ensures URLs are visited exactly once via the visited set
  - Waits for work items using ManualResetEvent, instead of spin-waiting, to avoid wasting CPU cycles
- Worker threads call IHtmlParser.GetUrls and enqueue the results
  - Errors in worker threads are written to Console.Error and otherwise ignored

## Thoughts
There is probably a more idiomatic way to implement this using the TPL.