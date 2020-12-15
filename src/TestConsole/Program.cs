using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var authEndpoint = "https://localhost:44339/token";
            var dataEndpoint = "https://localhost:44339/api/Query";

            var user = "testuser";
            var pwd = "testpwd";

            var token = TestAuth(authEndpoint, user, pwd).Result;

            if (!string.IsNullOrEmpty(token.access_token))
            {
                Console.WriteLine($"TestAuth success: token_type={token.token_type}");

                var result = TestDataQuery(dataEndpoint, token, "dbName", "any text for sql query", user).Result;
                Console.WriteLine($"TestDataQuery return:");
                foreach (System.Data.DataColumn col in result.Columns)
                    Console.Write($"{col.ColumnName}\t");
                Console.WriteLine();
                foreach (System.Data.DataColumn col in result.Columns)
                    Console.Write($"<{col.DataType.Name}>\t");
                Console.WriteLine();
                foreach (System.Data.DataRow row in result.Rows)
                {
                    foreach (var cell in row.ItemArray)
                        Console.Write($"{cell}\t");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"TestAuth failed: {token.error}");
            }

            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }

        static async Task<Token> TestAuth(string url, string user, string pwd)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var body = $"grant_type=password&username={user}&password={pwd}";
            var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Token>(result);
        }

        static async Task<System.Data.DataTable> TestDataQuery(string url, Token token, string db, string sql, string user)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.token_type, token.access_token);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var arg = new QueryArg { Database = db, Username = user, SqlText = sql, };
            var body = JsonConvert.SerializeObject(arg);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<System.Data.DataTable>(result);
        }
    }


    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }

        public string error { get; set; }
    }

    public class QueryArg
    {
        public string Username { get; set; }
        public string Database { get; set; }
        public string SqlText { get; set; }
    }
}
