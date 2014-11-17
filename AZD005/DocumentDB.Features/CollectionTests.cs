using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using NUnit.Framework;
using System;
using System.Linq;

namespace DocumentDB.Features
{
    [TestFixture]
    public class CollectionTests
    {
        [Test]
        public async void Run()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var database = await client.GetOrCreateDatabaseAsync();
                var collection = client
                    .CreateDocumentCollectionQuery(database.CollectionsLink)
                    .Where(x => x.Id == DocumentDB.CollectionId)
                    .AsEnumerable<DocumentCollection>()
                    .SingleOrDefault();

                if (collection == null)
                {
                    var target = new DocumentCollection { Id = DocumentDB.CollectionId };
                    target.IndexingPolicy.Automatic = true;
                    collection = await client.CreateDocumentCollectionAsync(database.SelfLink, target);
                }

                await client.DeleteDocumentCollectionAsync(collection.SelfLink);
                await client.DeleteDatabaseAsync(database.SelfLink);
            }
        }
    }
}
