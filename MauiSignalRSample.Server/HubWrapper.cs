using System.Collections.Concurrent;

namespace ServiceApi.Wrappers;

public static class HubWrapper
{
    public static ConcurrentDictionary<int, string> connectionMap;
    public static ConcurrentDictionary<string, string> tokenMap;
    static HubWrapper()
    {
        connectionMap = new();
        tokenMap = new ();
    }
}
