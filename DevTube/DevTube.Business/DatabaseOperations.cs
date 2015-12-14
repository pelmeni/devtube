using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTube.Data;

namespace DevTube.Business
{
    public static class DatabaseOperations
    {
        public static object GetStats()
        {
            return new MongoDbContext().Database.GetCollection<string>("col1")
            ;
        }
    }
}
