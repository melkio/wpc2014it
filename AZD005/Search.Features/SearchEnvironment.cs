using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Search.Features
{
    public class SearchEnvironment
    {
        private static readonly String ApiVersion = "api-version=2014-07-31-Preview";
        private static readonly String ServiceUrl = "https://codiceplastico.search.windows.net";
        private static readonly String PrimaryKey = File.ReadAllText("keys.secret");

        public static async Task<Boolean> DeleteIndex(String indexName)
        {
            var uri = String.Format("{0}/indexes/{1}?{2}", ServiceUrl, indexName, ApiVersion);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", PrimaryKey);
                var response = await client.DeleteAsync(uri); 
                
                return response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound;
            }
        }

        public static async Task<Boolean> CreateIndex(String indexName, String schema)
        {
            var uri = String.Format("{0}/indexes/{1}?{2}", ServiceUrl, indexName, ApiVersion);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", PrimaryKey);
                var response = await client.PutAsync(new Uri(uri), new StringContent(schema, Encoding.UTF8, "application/json"));

                return response.StatusCode == HttpStatusCode.Created;
            }
        }

        public static async Task<Boolean> LoadDocuments(String indexName, String documents)
        {
            //https://codiceplastico.search.windows.net/indexes/musicstoreindex/docs/index?api-version=2014-07-31-Preview
            var uri = String.Format("{0}/indexes/{1}/docs/index?{2}", ServiceUrl, indexName, ApiVersion);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", PrimaryKey);
                var response = await client.PostAsync(new Uri(uri), new StringContent(documents, Encoding.UTF8, "application/json"));

                return response.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}
