using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DevTube.Api;
using DevTube.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DevTube.Business
{
    public static class DocumentsHelper
    {
        public static Document InsertDocument(Document d)
        {

            var ctx = new MongoDbContext();

            ctx.DocumentCollection.InsertOne(d);

            return d;
        }

        public static IEnumerable<Document> GetAllDocuments()
        {
            var ctx = new MongoDbContext();
            var filter=new BsonDocument();
            //return ctx.DocumentCollection.Find(filter).ToList();

            return ctx.DocumentCollection.AsQueryable().ToList();

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

        public static void InsertDocument(IEnumerable<Document> ii)
        {

            var ctx = new MongoDbContext();

            foreach (var i in ii)
            {
                var f =
                    ctx.DocumentCollection.AsQueryable()
                        .FirstOrDefault(d => d.Path == i.Path && d.Level == i.Level && d.ContentType == i.ContentType);

                if(f!=null)
                    continue;

                InsertDocument(i);

            }
            

            
        }

        public static void LinkParents(List<FSItemInfo> list)
        {
            var ctx = new MongoDbContext();

            var d =ctx.DocumentCollection.AsQueryable().ToDictionary(j => j.HashPath, j => j.Id);

            var dsrc = list.ToDictionary(j => d[j.HashPath], j => j.Parent!=null?d[j.Parent.HashPath]:null);

            foreach (var i in dsrc)
            {
                var update = Builders<Document>.Update.Set(a => a.ParentId, i.Value);

                ctx.DocumentCollection.UpdateOne(n=>n.Id==i.Key,update);
            }
        }
    }
}

