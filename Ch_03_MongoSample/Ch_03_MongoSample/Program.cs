using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;

namespace Ch_03_MongoSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConnectionString, name of database & collection to connect
            //All those values will be acquired from App.config's setting section
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            string databaseName = ConfigurationManager.AppSettings["DatabaseName"];
            string collectionName = ConfigurationManager.AppSettings["CollectionName"];

            //Mongo client object
            MongoClient client = new MongoClient(connectionString);
            //Switch to specific database
            IMongoDatabase database = client.GetDatabase(databaseName);

            //While selecting the collection, we can specify the read preference
            MongoCollectionSettings collSettings = new MongoCollectionSettings()
            {
                ReadPreference = new ReadPreference(ReadPreferenceMode.Secondary)
            };

            //Adding a record into primary instances
            var messageId = new MongoDB.Bson.BsonObjectId(new MongoDB.Bson.ObjectId());
            var deviceId = new Random(1).Next();
            IMongoCollection<EventModel> productCollection = database.GetCollection<EventModel>(collectionName, collSettings);
            productCollection.InsertOne(new EventModel { _id = messageId, SiteId = 1, DeviceId = deviceId, SensorId = 1, Temperature = 20.05M, TestStatus = "Dormant", TimeStamp = DateTime.Now });
            EventModel result = null;

            //Loop through till the record gets replicated to secondary instance
            while (result == null)
            {    //Reading the newly inserted record from secondary instance
                result = productCollection.Find<EventModel>(x => x.DeviceId == deviceId).FirstOrDefault<EventModel>();
            }

            Console.WriteLine("Message Time:" + result.TimeStamp.ToString("dd/mm/yyyy hh:mm:ss"));
            Console.Read();
        }
    }
}
