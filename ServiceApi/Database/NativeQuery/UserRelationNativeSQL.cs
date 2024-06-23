using Microsoft.Data.SqlClient;

namespace ServiceApi.Database.NativeQuery;

public class UserRelationNativeSQL:BaseNativeSQL
{
    public async Task<List<int>> SelectListUserFriend(int userId)
    {
        List<int> result = new();
        try
        {
            string query = $"SELECT [ToUserId] FROM [dbo].[UserRelation] WHERE FromUserId = @fromUserId";

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@fromUserId", userId);

                    await conn.OpenAsync();

                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        result.Add(reader.GetInt32(0));
                    }
                    conn.Close();

                    return result;
                }
            }
        }
        catch (Exception)
        {
            return result;
        }
    }
    public async Task<int> InsertListUserFriend(int fromUserId, int toUserId)
    {
        try
        {
            string query = $"INSERT INTO [dbo].[UserRelation]([FromUserId],[ToUserId]) VALUES (@FromUserId,@ToUserId)";

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FromUserId", fromUserId);
                    cmd.Parameters.AddWithValue("@ToUserId", toUserId);

                    await conn.OpenAsync();

                    var output = await cmd.ExecuteScalarAsync();
                    conn.Close();

                    return output == null ? 0 : Convert.ToInt32(output);
                }
            }
        }
        catch (Exception)
        {
            return 0;
        }
    }
}
