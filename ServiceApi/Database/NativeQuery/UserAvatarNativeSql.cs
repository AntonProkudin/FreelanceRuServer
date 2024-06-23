using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;

namespace ServiceApi.Database.NativeQuery;

public class UserAvatarNativeSql : BaseNativeSQL
{
    public UserAvatarNativeSql() : base() { }
    public async Task<string> GetUrlAvatar(int Id)
    {
        try
        {
            string query = $"SELECT TOP(1) [Url] FROM [dbo].[UserAvatar] WHERE UserId = {Id}";
            string url = "";
            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        url = reader.GetString(0);
                    }
                    conn.Close();
                    return url;
                }
            }
        }
        catch (Exception)
        {
            return "image/default.png";
        }
    }
    public async Task<bool> SetUrlAvatar(int id, string url)
    {
        try
        {
            string query = $"UPDATE UserAvatar SET [Url]=N'{url}' WHERE [UserId] = {id} \r\nIF @@ROWCOUNT = 0 \r\nINSERT INTO UserAvatar ([UserId],[Url]) VALUES ( {id} , N'{url}')";
            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                    return true;
                }
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
}
