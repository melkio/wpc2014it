using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DocumentDB.Features
{
    public static class DocumentDBExtensionMethods
    {
        public static async Task<Database> GetOrCreateDatabaseAsync(this DocumentClient client)
        {
            var database = client
                .CreateDatabaseQuery()
                .Where(x => x.Id == DocumentDB.DatabaseId)
                .AsEnumerable<Database>()
                .SingleOrDefault();

            if (database == null)
                database = await client.CreateDatabaseAsync(new Database { Id = DocumentDB.DatabaseId });

            return database;
        }

        public static async Task<DocumentCollection> GetOrCreateDocumentCollectionAsync(this DocumentClient client, DocumentCollection target)
        {
            var database = await client.GetOrCreateDatabaseAsync();

            var collection = client
                .CreateDocumentCollectionQuery(database.CollectionsLink)
                .Where(x => x.Id == DocumentDB.CollectionId)
                .AsEnumerable<DocumentCollection>()
                .SingleOrDefault();

            if (collection != null)
                await client.DeleteDocumentCollectionAsync(collection.SelfLink);

            collection = await client.CreateDocumentCollectionAsync(database.SelfLink, target);

            return collection;
        }

        public static async Task<DocumentCollection> GetOrCreateDocumentCollectionAsync(this DocumentClient client)
        {
            var target = new DocumentCollection { Id = DocumentDB.CollectionId };
            return await client.GetOrCreateDocumentCollectionAsync(target);
        }

        public static async Task CleanEnvironmentAndLoadDocuments(this DocumentClient client)
        {
            var database = await client.GetOrCreateDatabaseAsync();
            await client.DeleteDatabaseAsync(database.SelfLink);

            var collection = await client.GetOrCreateDocumentCollectionAsync();

            var documents = Directory.GetFiles("Documents")
                .Select(x => File.ReadAllText(x))
                .Select(x => JsonConvert.DeserializeObject<dynamic>(x));

            foreach (var document in documents)
                await client.CreateDocumentAsync(collection.DocumentsLink, document);
        }
    }
}
