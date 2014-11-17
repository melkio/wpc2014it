using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using NUnit.Framework;
using System;
using System.Linq;

namespace DocumentDB.Features
{
    [TestFixture]
    public class DatabaseTests 
    {
        [Test]
        public async void Run()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var database = client
                    .CreateDatabaseQuery()
                    .Where(x => x.Id == DocumentDB.DatabaseId)
                    .AsEnumerable<Database>()
                    .SingleOrDefault();

                if (database == null)
                {
                    database = await client.CreateDatabaseAsync(new Database { Id = DocumentDB.DatabaseId });
                }

                String requestContinuation = null;
                do
                {
                    var options = new FeedOptions
                    {
                        MaxItemCount = 1,
                        RequestContinuation = requestContinuation
                    };
                    
                    var feed = await client.ReadDatabaseFeedAsync(options);
                    var current = feed.SingleOrDefault();
                    
                    requestContinuation = feed.ResponseContinuation;
                } while (!String.IsNullOrEmpty(requestContinuation));

                await client.DeleteDatabaseAsync(database.SelfLink);
            }
        }

        
    }
}
