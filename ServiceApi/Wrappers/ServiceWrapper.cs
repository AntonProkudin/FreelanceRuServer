using Microsoft.Data.SqlClient;
using ServiceApi.Database.NativeQuery;
using System.Collections.Concurrent;

namespace ServiceApi.Wrappers;

public static class ServiceWrapper
{
    private static CategoryNativeSql db;
    public static ConcurrentDictionary<int,string> CategoryList;
    public static ConcurrentDictionary<(int,int), int> Relation;
    static ServiceWrapper()
    {
        Relation = new();
        db = new();
        CategoryList = db.InitCategory();
    }
}
