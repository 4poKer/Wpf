using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MongoDB.Bson;

namespace HabrParser
{
    public class HabrArticle
    {
        public ObjectId Id { get; set; }

        public int HabrId { get; set; }

        public string Theme { get; set; }

        public string Title { get; set; }

        public List<string> Tags { get; set; }

        public string PublicationDate { get; set; }

    }
}
