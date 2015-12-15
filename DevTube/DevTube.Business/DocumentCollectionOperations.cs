using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTube.Api;
using DevTube.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DevTube.Business
{
    public static class DocumentCollectionOperations
    {
        public static Document InsertDocument(Document d)
        {

            var ctx = new MongoDbContext();

            ctx.DocumentCollection.InsertOne(d);
            



            //ctx.DocumentCollection.FindSync()
            return d;
        }

        public static IEnumerable<Document> GetAllDocuments()
        {
            var ctx = new MongoDbContext();
            var filter=new BsonDocument();
            return ctx.DocumentCollection.Find(filter).ToList();

        }

        public static IEnumerable<Document> FindAll(this IMongoCollection<Document> col)
        {
            var filter = new BsonDocument();
            return col.Find(filter).ToEnumerable();
        }

        public static void InsertOrUpdateDocument(Document doc)
        {
            if (doc.Id == null)
                InsertDocument(doc);
            else
                UpdateDocument(doc);

        }

        private static void UpdateDocument(Document doc)
        {
            var ctx = new MongoDbContext();
            var update = Builders<Document>.Update
                .Set(i => i.Body, doc.Body)
                .Set(i => i.ContentType, doc.ContentType)
                .Set(i => i.Header, doc.Header)
                .Set(i => i.Path, doc.Path)
                .Set(i => i.Size, doc.Size)
                .Set(i => i.Body, doc.Body)
                .Set(i => i.Thumb, doc.Thumb);


            ctx.DocumentCollection.UpdateOne(i => i.Id == doc.Id, update);
        }

        public static Document Get(string id) { 
            
            var ctx = new MongoDbContext();
            return ctx.DocumentCollection.Find(i=>i.Id==id).FirstOrDefault();
        }

        public static void Delete(string id)
        {
            var ctx = new MongoDbContext();

            ctx.DocumentCollection.FindOneAndDelete(i => i.Id == id);

        }
    }
}

