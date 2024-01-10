using System.Data.SqlClient;

namespace minutebank.Base
{
    public interface IDbConnectionClass
    {
        T? GetEntity<T>(string query, Func<SqlDataReader, T> mapFunc);
        List<T> GetEntities<T>(string query, Func<SqlDataReader, T> mapFunc);
        int AddEntity<T>(string query, Dictionary<string, object> dictionary);

        void UpdateEntity<T>(string query);

        List<T> ExecuteStoredProcedure<T>(string storedProcedureName, Dictionary<string, object> parameters, Func<SqlDataReader, T> mapFunc);
    }
}