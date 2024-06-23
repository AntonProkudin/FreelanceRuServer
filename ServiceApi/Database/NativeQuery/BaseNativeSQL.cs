namespace ServiceApi.Database.NativeQuery;

public class BaseNativeSQL
{
    public static string? connectionString;
    public BaseNativeSQL()
    {
        connectionString = "Data Source=PECHENEG\\SQLEXPRESS;Initial Catalog=FreelanceRu;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}
