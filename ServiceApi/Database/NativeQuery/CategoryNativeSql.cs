using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;

namespace ServiceApi.Database.NativeQuery;

public class CategoryNativeSql : BaseNativeSQL
{
    public CategoryNativeSql() : base() { }
    public ConcurrentDictionary<int, string> InitCategory()
    {
        try
        {
            string query = $"SELECT * FROM [dbo].[Category]";

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    ConcurrentDictionary<int, string> categories = new ConcurrentDictionary<int, string>();
                    while (reader.Read())
                    {
                        int Id = reader.GetInt32(0);
                        string Title = reader.GetString(1);
                        categories.TryAdd(Id, Title);
                    }
                    conn.Close();
                    return categories;
                }
            }
        }
        catch (Exception)
        {
            return new ConcurrentDictionary<int, string>();
        }
    }
}
