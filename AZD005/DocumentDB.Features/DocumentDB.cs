using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentDB.Features
{
    public class DocumentDB
    {
        public static readonly String DatabaseId = "wpc2014";
        public static readonly String CollectionId = "data";


        public static DocumentClient InitializeClient()
        {
            var uri = new Uri("https://codiceplastico.documents.azure.com:443/");
            var key = File.ReadAllText("keys.secret");

            return new DocumentClient(uri, key);
        }
    }
}
