using System.Collections.Concurrent;

namespace ServiceApi.Wrappers;

public static class HubWrapper
{
    public static ConcurrentDictionary<int, string> connectionMap;
    static HubWrapper()
    {
        connectionMap = new();
    }
}
