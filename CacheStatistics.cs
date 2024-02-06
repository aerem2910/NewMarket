using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Reflection;

namespace StoreMarket
{
    public static class CacheStatistics
    {
        public static string GetCacheStatistics(IMemoryCache memoryCache)
        {
            if (memoryCache is MemoryCache cache)
            {
                var field = typeof(MemoryCache).GetField("_stats", BindingFlags.NonPublic | BindingFlags.Instance);
                var stats = (MemoryCacheStats)field.GetValue(cache);

                var statsText = $"Cache Hits: {stats.HitCount}\nCache Misses: {stats.MissCount}\nCache Sets: {stats.SetCount}";

                var filePath = Path.Combine(Path.GetTempPath(), "cache_statistics.txt");
                File.WriteAllText(filePath, statsText);

                return filePath;
            }
            else
            {
                return "IMemoryCache does not implement MemoryCache interface.";
            }
        }

        private class MemoryCacheStats
        {
            public object HitCount { get; internal set; }
            public object MissCount { get; internal set; }
            public object SetCount { get; internal set; }
        }
    }
}

