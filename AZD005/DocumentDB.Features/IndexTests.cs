using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using NUnit.Framework;
using System;
using System.Linq;

namespace DocumentDB.Features
{
    [TestFixture]
    public class IndexTests
    {
        [Test]
        public async void ExcludeDocument()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var documentId = Guid.NewGuid().ToString();
                var document = await client.CreateDocumentAsync
                    (
                        collection.DocumentsLink,
                        new { id = documentId, surname = "Melchiori", name = "Alessandro", random = documentId },
                        new RequestOptions { IndexingDirective = IndexingDirective.Exclude }
                    );

                var exists = await client.ReadDocumentAsync(document.Resource.SelfLink);
                Assert.NotNull(exists);

                var query = String.Format("SELECT * FROM root x WHERE x.random = '{0}'", documentId);
                var search = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, query)
                    .AsEnumerable()
                    .Any();
                Assert.False(search);
            }
        }

        [Test]
        public async void ManualIndexing()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var target = new DocumentCollection { Id = DocumentDB.CollectionId };
                target.IndexingPolicy.Automatic = false;

                var collection = await client.GetOrCreateDocumentCollectionAsync(target);

                var documentId = Guid.NewGuid().ToString();
                var document = await client.CreateDocumentAsync
                    (
                        collection.DocumentsLink,
                        new { id = documentId, surname = "Melchiori", name = "Alessandro", random = documentId }
                    );

                var exists = await client.ReadDocumentAsync(document.Resource.SelfLink);
                Assert.NotNull(exists);

                var query = String.Format("SELECT * FROM root x WHERE x.random = '{0}'", documentId);
                var search = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, query)
                    .AsEnumerable()
                    .Any();
                Assert.False(search);

                var includedDocumentId = Guid.NewGuid().ToString();
                var includedDocument = await client.CreateDocumentAsync
                    (
                        collection.DocumentsLink,
                        new { id = includedDocumentId, surname = "Melchiori", name = "Alessandro", random = includedDocumentId },
                        new RequestOptions { IndexingDirective = IndexingDirective.Include }
                    );

                var includedExists = await client.ReadDocumentAsync(includedDocument.Resource.SelfLink);
                Assert.NotNull(exists);

                var includedQuery = String.Format("SELECT * FROM root x WHERE x.random = '{0}'", includedDocumentId);
                var includedSearch = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, includedQuery)
                    .AsEnumerable()
                    .Any();
                Assert.True(includedSearch);
            }
        }

        [Test]
        public async void LazyIndex()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var target = new DocumentCollection { Id = DocumentDB.CollectionId };
                target.IndexingPolicy.IndexingMode = IndexingMode.Lazy;

                var collection = await client.GetOrCreateDocumentCollectionAsync(target);

                //...
            }
        }

        [Test]
        public async void RangeIndex()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var target = new DocumentCollection { Id = DocumentDB.CollectionId };
                target.IndexingPolicy.IncludedPaths.Add(new IndexingPath
                {
                    IndexType = IndexType.Hash,
                    Path = "/"
                });
                target.IndexingPolicy.IncludedPaths.Add(new IndexingPath
                {
                    IndexType = IndexType.Range,
                    Path = @"/""age""/?",
                });

                var collection = await client.GetOrCreateDocumentCollectionAsync(target);

                //...
            }
        }

        [Test]
        public async void ExcludePaths()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var target = new DocumentCollection { Id = DocumentDB.CollectionId };
                target.IndexingPolicy.IncludedPaths.Add(new IndexingPath { Path = "/" });
                target.IndexingPolicy.ExcludedPaths.Add("/\"age\"/*");
                target.IndexingPolicy.ExcludedPaths.Add("/\"contact\"/\"value\"/*");

                var collection = await client.GetOrCreateDocumentCollectionAsync(target);

                var documents = GetDocuments();
                foreach (var document in documents)
                    await client.CreateDocumentAsync(collection.DocumentsLink, document);

                var existsByName = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT x FROM root x WHERE x.name = 'Alessandro'")
                    .AsEnumerable()
                    .Any();
                Assert.True(existsByName);

                var existsByContactType = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT x FROM root x WHERE x.contact.type = 'email'")
                    .AsEnumerable()
                    .Any();
                Assert.True(existsByContactType);

                Assert.Throws<AggregateException>(() => client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT x FROM root x WHERE x.age > 10")
                    .AsEnumerable()
                    .Any());

                Assert.Throws<AggregateException>(() => client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT x FROM root x WHERE x.contact.value = 'alessandro@codiceplastico.com'")
                    .AsEnumerable()
                    .Any());
            }
        }

        [Test]
        public async void ColelctionScan()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var target = new DocumentCollection { Id = DocumentDB.CollectionId };
                target.IndexingPolicy.IncludedPaths.Add(new IndexingPath
                {
                    IndexType = IndexType.Hash,
                    Path = "/"
                });
                target.IndexingPolicy.IncludedPaths.Add(new IndexingPath
                {
                    IndexType = IndexType.Hash,
                    Path = @"/""age""/?",
                });

                var collection = await client.GetOrCreateDocumentCollectionAsync(target);

                var document = await client.CreateDocumentAsync(collection.DocumentsLink, new { id = "1", name = "alessandro", age = 35 });

                Assert.Throws<AggregateException>(() => client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT x FROM root x WHERE x.age > 10")
                    .AsEnumerable()
                    .Any());

                var exists = client
                    .CreateDocumentQuery<dynamic>(collection.DocumentsLink, "SELECT x FROM root x WHERE x.age > 10", new FeedOptions { EnableScanInQuery = true })
                    .AsEnumerable()
                    .Any();
                Assert.True(exists);
            }
        }

        private static dynamic[] GetDocuments()
        {
            return new[]
            {
                    new 
                    { 
                        id = "1", 
                        name = "Alessandro", 
                        age = 35,
                        contact = new { type = "email", value = "alessandro@codiceplastico.com" }
                    },
                    new 
                    { 
                        id = "2", 
                        name = "Emanuele", 
                        age = 41,
                        contact = new { type = "twitter", value = "emadb" }
                    }
            };
        }

    }
}
