using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch_03_MongoSample
{
    /// <summary>
    /// Model defined for Event Message generated from sensors
    /// </summary>
    public class EventModel
    {
        /// <summary>
        /// Default ID
        /// </summary>
        public MongoDB.Bson.BsonObjectId _id { get; set; }
        /// <summary>
        /// Site information
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// Device information installed for a site
        /// </summary>
        public int DeviceId { get; set; }
        /// <summary>
        /// Sensor information installed in Device
        /// </summary>
        public int SensorId { get; set; }
        /// <summary>
        /// Temperature Reading
        /// </summary>
        public decimal Temperature { get; set; }
        /// <summary>
        /// Overall Health of the Device
        /// </summary>
        public string TestStatus { get; set; }
        /// <summary>
        /// Message TimeStamp
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }

}
