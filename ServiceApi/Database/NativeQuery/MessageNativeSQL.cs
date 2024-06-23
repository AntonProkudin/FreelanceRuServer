using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;

namespace ServiceApi.Database.NativeQuery;

public class MessageNativeSQL : BaseNativeSQL
{
    UserRelationNativeSQL db;
    public MessageNativeSQL() : base() 
    {
        db = new();
    }

    public async Task<int> AddMessage(Message msg)
    {
        try
        {
            string query = $"INSERT INTO [dbo].[Messages]([FromUserId],[ToUserId],[Text],[IsImage],[TS]) OUTPUT INSERTED.ID VALUES (@FromUserId,@ToUserId,@Text,0,@TS)";

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FromUserId", msg.FromUserId);
                    cmd.Parameters.AddWithValue("@ToUserId", msg.ToUserId);
                    cmd.Parameters.AddWithValue("@Text", msg.Text);
                    cmd.Parameters.AddWithValue("@TS", msg.SendTime);

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
    public async Task<List<Message>> SelectMessage(int userId, int toUserId, DateTime ts)
    {
        List<Message> messages = new List<Message>();
        try
        {
            string query = $"SELECT [Id],[FromUserId], [ToUserId], [Text], [TS] FROM [dbo].[Messages] Where (FromUserId = @UserId AND ToUserId = @toUserId) OR (FromUserId = @toUserId AND ToUserId = @UserId) And TS < @Date";

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@toUserId", toUserId);
                    cmd.Parameters.AddWithValue("@Date", ts);

                    await conn.OpenAsync();

                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        messages.Add(new Message
                        {
                            Id = reader.GetInt32(0),
                            FromUserId = reader.GetInt32(1),
                            ToUserId = reader.GetInt32(2),
                            Text = reader.GetString(3),
                            SendTime = reader.GetDateTime(4),
                        });
                    }
                    conn.Close();

                    return messages;
                }
            }
        }
        catch (Exception ex)
        {
            return messages;
        }
    }
    public async Task<List<int>> SelectMessageUserId(int userId)
    {
        List<int> ids = new List<int>();

        try
        {
            string query = $"SELECT DISTINCT [FromUserId], [ToUserId] FROM [dbo].[Messages] Where (FromUserId = @UserId OR ToUserId = @UserId)";

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    await conn.OpenAsync();

                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        if (ids.Contains(reader.GetInt32(0)) && reader.GetInt32(0) != userId)
                            ids.Add(reader.GetInt32(0));

                        if (ids.Contains(reader.GetInt32(1)) && reader.GetInt32(1) != userId)
                            ids.Add(reader.GetInt32(1));
                            
                    }
                    conn.Close();

                    return ids;
                }
            }
        }
        catch (Exception)
        {
            return ids;
        }
    }
    public async Task<Message> SelectLastMessage(int fromUserId, int toUserId, DateTime ts)
    {
        Message message = null;
        try
        {
            string query = $"SELECT TOP (1) [Id],[FromUserId],[ToUserId],[Text],[TS] FROM [dbo].[Messages] Where (FromUserId = @UserId And ToUserId = @ToUserId) And TS < @Date ORDER BY TS DESC";

            using (SqlConnection conn = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", fromUserId);
                    cmd.Parameters.AddWithValue("@ToUserId", toUserId);
                    cmd.Parameters.AddWithValue("@Date", ts);

                    await conn.OpenAsync();

                    var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        message = new Message()
                        {
                            Id = reader.GetInt32(0),
                            FromUserId = reader.GetInt32(1),
                            ToUserId = reader.GetInt32(2),
                            Text = reader.GetString(3),
                            SendTime = reader.GetDateTime(4),
                        };
                    }
                    conn.Close();

                    return message == null ? null: message;
                }
            }
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
