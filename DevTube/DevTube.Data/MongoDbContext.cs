using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTube.Data.Properties;
using MongoDB.Driver;

namespace DevTube.Data
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; private set; }

        public IMongoClient Client { get; private set; }

        public MongoDbContext()
        {
            Client = new MongoClient(Settings.Default.DevTubeDatabaseConnectionString);
            Database = Client.GetDatabase(Settings.Default.DevTubeDatabaseName);
            }
        
    }
}
