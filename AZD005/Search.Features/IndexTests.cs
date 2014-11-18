using NUnit.Framework;
using System;
using System.IO;

namespace Search.Features
{
    [TestFixture]
    public class IndexTests
    {
        [Test]
        public async void CreateIndex()
        {
            await SearchEnvironment.DeleteIndex("musicstoreindex");

            var schema = File.ReadAllText("Support\\schema.json");
            await SearchEnvironment.CreateIndex("musicstoreindex", schema);

            await SearchEnvironment.LoadDocuments("musicstoreindex", File.ReadAllText("Support\\data1.json"));
            await SearchEnvironment.LoadDocuments("musicstoreindex", File.ReadAllText("Support\\data2.json"));
            await SearchEnvironment.LoadDocuments("musicstoreindex", File.ReadAllText("Support\\data3.json"));
        }
    }
}
