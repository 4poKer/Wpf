using System.Collections.Generic;
using System.Linq;


namespace HabrParser
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
     
}
