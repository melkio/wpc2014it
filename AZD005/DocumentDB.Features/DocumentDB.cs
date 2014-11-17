using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using NUnit.Framework;
using System;
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
            var key = "l1noRUNBKCVSwo/iHJa+jVvWT0MT83MwcU6P3Wrj7yr8cBnk7wgLx9h1wrF74RA15v6t9LwofwI+YpNTRolryw==";

            return new DocumentClient(uri, key);
        }
    }
}
