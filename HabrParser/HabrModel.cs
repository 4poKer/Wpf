using System.Collections.Generic;
using System.Linq;


namespace HabrParser
{
    public class HabrModel
    {
        public class HabrAutor
        {
            
            public int Id { get; set; }

            public string NickName { get; set; }

            public string Name { get; set; }

            public string Surname { get; set; }

            public string AboutInfo { get; set; }

            public virtual List<HabrArticle> HabrArticles { get; set; }

        }

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
}
