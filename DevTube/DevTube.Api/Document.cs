using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DevTube.Api
{
    public class Document 
    {
        [Description("Описание")]
        public string Body
        {
            get;

            set;
        }
        [Description("Тип документа")]
        public string ContentType
        {
            get;

            set;
        }
        [Description("Заголовок")]
        public string Header
        {
            get;

            set;
        }
        [BsonRepresentation(BsonType.ObjectId)]
        [Description("Идентификатор")]
        public string Id
        {
            get;

            set;
        }
        [Description("Путь к файлу")]
        public string Path
        {
            get;

            set;
        }
        [Description("Размер в байтах")]
        public long Size
        {
            get;

            set;
        }
        [Description("Миниатюрка")]
        public byte[] Thumb
        {
            get;

            set;
        }
        [Description("Уровень")]
        public int Level { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [Description("Идентификатор родителя")]
        public string ParentId
        {
            get;

            set;
        }
        public string HashPath { get; set; }
    }
}
