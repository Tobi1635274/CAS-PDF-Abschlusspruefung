using CAS_API.Models;
using System.Data.SqlClient;

namespace CAS_API.Helper
{
#pragma warning disable 1591
    public class DocumentHelper
    {

        public static async Task<Document?> GetDocumentById(string documentId, string connectionString)
        {
            Document? document = null;
            var con = new SqlConnection(connectionString);
            con.Open();
            var cmd = new SqlCommand($"SELECT * FROM dbo.Documents where Id = {documentId}", con);
            var dataReader = await cmd.ExecuteReaderAsync();
            while (await dataReader.ReadAsync())
            {
                document = new Document() { Id = (int)dataReader.GetValue(dataReader.GetOrdinal("Id")), Dateiname = (string)dataReader.GetValue(dataReader.GetOrdinal("Dateiname")), Pfad = (string)dataReader.GetValue(dataReader.GetOrdinal("Pfad")), LetzteAenderung = (DateTime)dataReader.GetValue(dataReader.GetOrdinal("LetzteAenderung")) };
            }
            con.Close();
            return document;
        }
    }
#pragma warning restore 1591
}
