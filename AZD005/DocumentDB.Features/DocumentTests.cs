using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DocumentDB.Features
{
    [TestFixture]
    public class DocumentTests
    {
        [Test]
        public async void AddDocument()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();
                
                var document = new UserDocument
                {
                    Name = "Alessandro",
                    Surname = "Melchiori",
                    Contacts = new[]
                    {
                        new Contact { Type = "Email", Value = "alessandro@codiceplastico.com" },
                        new Contact { Type = "Fax", Value = "+39 030 6595241" }
                    },
                    Social = new[]
                    {
                        new Contact { Type = "twitter", Value = "amelchiori" },
                        new Contact { Type = "slideshare", Value = "melkio" },
                        new Contact { Type = "github", Value = "melkio" }
                    }
                };
                var response = await client.CreateDocumentAsync(collection.DocumentsLink, document);

                Assert.NotNull(response);
            }
        }

        [Test]
        public async void AddClass()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                var document = new User
                {
                    Name = "Emanuele",
                    Surname = "DelBono",
                    Contacts = new[]
                    {
                        new Contact { Type = "Email", Value = "emanuele@codiceplastico.com" },
                        new Contact { Type = "Fax", Value = "+39 030 6595241" }
                    },
                    Social = new[]
                    {
                        new Contact { Type = "twitter", Value = "emadb" },
                        new Contact { Type = "github", Value = "emadb" }
                    }
                };
                var response = await client.CreateDocumentAsync(collection.DocumentsLink, document);

                Assert.NotNull(response);
            }
        }

        [Test]
        public async void AddDynamic()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();

                dynamic document = new
                {
                    id = "1",
                    name = "Alessandro",
                    surname = "Melchiori",
                    title = "software developer"
                };
                var response = await client.CreateDocumentAsync(collection.DocumentsLink, document);

                Assert.NotNull(response);
            }
        }

        [Test]
        public async void EditDocument()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();
                var document = await client.CreateDocumentAsync(collection.DocumentsLink, new { id = "1", name = "alessandro" });

                await client.ReplaceDocumentAsync(document.Resource.SelfLink, new { id = "1", value = "edited" });
            }
        }

        [Test]
        public async void AddAndReadAttachment()
        {
            using (var client = DocumentDB.InitializeClient())
            {
                var collection = await client.GetOrCreateDocumentCollectionAsync();
                var document = await client.CreateDocumentAsync(collection.DocumentsLink, new { id = "1", name = "alessandro" });

                using (var stream = File.Open("Attachment.txt", FileMode.Open))
                    await client.CreateAttachmentAsync(document.Resource.AttachmentsLink, stream);

                var attachment = client
                    .CreateAttachmentQuery(document.Resource.SelfLink)
                    .AsEnumerable()
                    .FirstOrDefault();

                var content = await client.ReadMediaAsync(attachment.MediaLink);
                var buffer = new Byte[content.ContentLength];
                await content.Media.ReadAsync(buffer, 0, (int)content.ContentLength);

                var text = Encoding.UTF8.GetString(buffer);

                Assert.NotNull(text);
            }
        }
    }

    public class UserDocument : Document
    {
        public String Name
        {
            get { return GetValue<string>("Name"); }
            set { SetValue("Name", value); }
        }

        public String Surname
        {
            get { return GetValue<string>("Surname"); }
            set { SetValue("Surname", value); }
        }

        public Contact[] Contacts
        {
            get { return GetValue<Contact[]>("Contacts"); }
            set { SetValue("Contacts", value); }
        }

        public Contact[] Social
        {
            get { return GetValue<Contact[]>("Social"); }
            set { SetValue("Social", value); }
        }
    }

    public class User 
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }
        [JsonProperty(PropertyName = "surname")]
        public String Surname { get; set; }
        [JsonProperty(PropertyName = "contacts")]
        public Contact[] Contacts { get; set;}
        [JsonProperty(PropertyName = "social")]
        public Contact[] Social { get; set; }
    }

    public class Contact
    {
        [JsonProperty(PropertyName = "type")]
        public String Type { get; set; }
        [JsonProperty(PropertyName = "value")]
        public String Value { get; set; }
    }
}
