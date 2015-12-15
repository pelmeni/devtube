using DevTube.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace DevTube.Models
{
    public class DocumentViewModel:Document
    {
        public static DocumentViewModel From(Document doc)
        {
            return new DocumentViewModel
            {
                Id = doc.Id,
                Path = doc.Path,
                Size = doc.Size,
                Thumb = doc.Thumb,
                Body = doc.Body,
                ContentType = doc.ContentType,
                Header = doc.Header,
                Level = doc.Level,
            };
        }
        public Document ToDocument()
        {
            var doc = this;
            return new Document
            {
                Id = doc.Id,
                Path = doc.Path,
                Size = doc.Size,
                Thumb = doc.Thumb,
                Body = doc.Body,
                ContentType = doc.ContentType,
                Header = doc.Header,
                Level = doc.Level
            };
        }

        public string VPath => ("~/Files/" + Path);
    }
}