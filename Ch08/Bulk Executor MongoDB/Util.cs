using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Bulk_Executor_MongoDB
{
    
    class Utils
    {
        /// <summary>
        /// Get the collection if it exists, null if it doesn't.
        /// </summary>
        /// <returns>The requested collection.</returns>
        static internal DocumentCollection GetCollectionIfExists(DocumentClient client, string databaseName, string collectionName)
        {
            if (GetDatabaseIfExists(client, databaseName) == null)
            {
                return null;
            }

            return client.CreateDocumentCollectionQuery(UriFactory.CreateDatabaseUri(databaseName))
                .Where(c => c.Id == collectionName).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Get the database if it exists, null if it doesn't.
        /// </summary>
        /// <returns>The requested database.</returns>
        static internal Database GetDatabaseIfExists(DocumentClient client, string databaseName)
        {
            return client.CreateDatabaseQuery().Where(d => d.Id == databaseName).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Create a partitioned collection.
        /// </summary>
        /// <returns>The created collection.</returns>
        static internal async Task<DocumentCollection> CreatePartitionedCollectionAsync(DocumentClient client, string databaseName,
            string collectionName, int collectionThroughput)
        {
            PartitionKeyDefinition partitionKey = new PartitionKeyDefinition
            {
                Paths = new Collection<string> { ConfigurationManager.AppSettings["CollectionPartitionKey"] }
            };
            DocumentCollection collection = new DocumentCollection { Id = collectionName, PartitionKey = partitionKey };

            try
            {
                collection = await client.CreateDocumentCollectionAsync(
                    UriFactory.CreateDatabaseUri(databaseName),
                    collection,
                    new RequestOptions { OfferThroughput = collectionThroughput });
            }
            catch (Exception e)
            {
                throw e;
            }

            return collection;
        }

        
    static internal BsonDocument GenerateRandomDocumentString(String id, String partitionKeyProperty, string partitionKeyValue)
        {
            return new BsonDocument()
            {
                { partitionKeyProperty, partitionKeyValue },
                { "Name", "TestDoc"},
                {"description", "1.99"},
                {"f1", "3hrkjh3h4h4h3jk4h"},
                {"f2", "dhfkjdhfhj4434434"},
                {"f3", "nklfjeoirje434344"},
                {"f4", "pjfgdgfhdgfgdhbd6"},
                {"f5", "gyuerehvrerebrhjh"},
                {"f6", "3434343ghghghgghj"}
            };
        }
    }
}
