using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;

namespace Ch_03_MongoSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ///Get the connectionstring, name of database & collection name from App.config
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            string databaseName = ConfigurationManager.AppSettings["DatabaseName"];
            string collectionName = ConfigurationManager.AppSettings["CollectionName"];

            //Connect to the Azure Cosmos DB using MongoClient
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(databaseName);
            IMongoCollection<EventModel> sampleCollection
                = database.GetCollection<EventModel>(collectionName);

            //This will hold list of object needs to insert together
            List<EventModel> objList = new List<EventModel>();

            //Loop through Days, right now I am considering only 1 day but feel free to change
            for (int day = 1; day >= 1; day--)
            {
                //loop through the hour
                for (int hour = 1; hour <= 24; hour++)
                {
                    //loop through the minute
                    for (int minute = 1; minute <= 60; minute++)
                    {
                        //loop through the seconds
                        for (int second = 1; second <= 60; second++)
                        {
                            //Loop through the sites
                            for (int site = 1; site <= 10; site++)
                            {
                                //Loop through the Devices
                                for (int device = 1; device <= 15; device++)
                                {
                                    //Loop through the sensors
                                    for (int sensor = 1; sensor <= 4; sensor++)
                                    {
                                        //initialize the message object
                                        var obj = new EventModel()
                                        {
                                            _id = new BsonObjectId(new ObjectId()),
                                            SiteId = site,
                                            //It will help uniquely generating DeviceId basis the site
                                            DeviceId = device + site * 1000,
                                            //This will help uniquely generating SensorId basis the Device
                                            SensorId = sensor + ((device + site * 1000) * 1000),
                                            TimeStamp = DateTime.Now,
                                            Temperature = 20.9M,
                                            TestStatus = "Pass",
                                            deviceidday = device.ToString() + DateTime.Now.ToShortDateString()
                                        };
                                        //add into the list
                                        objList.Add(obj);
                                    }
                                }
                                //indicate Site's messages are added
                                Console.WriteLine("site:" + site);
                            }
                            //indicate the second roll over completed
                            Console.WriteLine("second" + second);
                            //inserting the messages collected in one minute interval
                            sampleCollection.InsertMany(objList);
                            //clear the list to get ready for next minute sequence
                            objList.Clear();
                        }
                        //indicate the minute roll over completed
                        Console.WriteLine("minute" + minute);
                    }
                    //indicate the hour roll over completed
                    Console.WriteLine("hour" + hour);
                }
                //indicate the Day roll over completed
                Console.WriteLine("day" + day);

            }

        }

    }
}
