using System;
using System.Collections.Generic;
using System.Linq;


namespace HabrParser
{
    public class HabrArticle
    {
        public int Id { get; set; }

        public int HabrArticleId { get; set; }

        public string Theme { get; set; }

        public string Title { get; set; }

        public ICollection<string> Tags { get; set; }

        public string ListString
        {
            get { return string.Join(",", Tags); }
            set { Tags = value.Split(',').ToList(); }
        }

        public string PublicationDate { get; set; }

        public virtual HabrAutor HabrAutor { get; set; }
    }
}
