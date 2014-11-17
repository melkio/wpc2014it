using NUnit.Framework;
using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;

namespace DocumentDB.Features
{
    [TestFixture]
    public class QueryTests
    {
        [Test]
        public async void QueryAll()
        {
            using (var client = DocumentDB.InitializeClient())
            await client.CleanEnvironmentAndLoadDocuments();

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documents = client
                    .CreateDocumentQuery(collection.DocumentsLink)
                    .ToList();

                Assert.AreEqual(2, documents.Count);
            }

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documents = client
                    .CreateDocumentQuery(collection.DocumentsLink, "SELECT * FROM root")
                    .ToList();

                Assert.AreEqual(2, documents.Count);
            }
        }

        [Test]
        public async void QueryWithFilter()
        {
            using (var client = DocumentDB.InitializeClient())
                await client.CleanEnvironmentAndLoadDocuments();

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documents = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink)
                    .AsEnumerable()
                    .Where(x => x.name == "Alessandro")
                    .ToList();

                Assert.AreEqual(1, documents.Count);
            }

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documents = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT * FROM root x WHERE x.name = 'Alessandro'")
                    .ToList();

                Assert.AreEqual(1, documents.Count);
            }
        }

        [Test]
        public async void RangeQuery()
        {
            using (var client = DocumentDB.InitializeClient())
                await client.CleanEnvironmentAndLoadDocuments();

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documents = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink)
                    .AsEnumerable()
                    .Where(x => x.age < 40 && x.age > 30)
                    .ToList();

                Assert.AreEqual(1, documents.Count);
            }
        }

        [Test]
        public async void QuerySubDocuments()
        {
            using (var client = DocumentDB.InitializeClient())
                await client.CleanEnvironmentAndLoadDocuments();

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documents = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT x FROM x IN root.contacts")
                    .ToList();

                Assert.AreEqual(4, documents.Count);
            }

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documents = client
                    .CreateDocumentQuery<User>(collection.DocumentsLink)
                    .SelectMany(x => x.Contacts)
                    .ToList();

                Assert.AreEqual(4, documents.Count);
            }
        }

        [Test]
        public async void QueryWithPaging()
        {
            using (var client = DocumentDB.InitializeClient())
                await client.CleanEnvironmentAndLoadDocuments();

            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var count = 0;
                var query = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT * FROM root", new FeedOptions { MaxItemCount = 1 })
                    .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    var current = await query.ExecuteNextAsync();
                    count++;
                }

                Assert.AreEqual(2, count);
            }
        }
    }
}
