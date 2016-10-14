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

        //public string ArticleText { get; set; }

        public HabrArticle(HtmlDocument document, int articleId)
        {
            this.HabrId = articleId;

            this.Theme = document.DocumentNode.SelectSingleNode("//h1[contains(@class,'post__title')]//a").InnerText;

            this.Title = document.DocumentNode.SelectNodes("//h1[contains(@class,'post__title')]//span")[1].InnerText;

            this.Tags = new List<string>();

            var tags = document.DocumentNode.SelectNodes("//a[contains(@class,'hub')]");

            foreach (var tag in tags)
                this.Tags.Add(tag.InnerText.ToString());

            this.PublicationDate =
                document.DocumentNode.SelectSingleNode("//span[contains(@class,'post__time_published')]").InnerText;

            /*this.ArticleText =
                document.DocumentNode.SelectSingleNode("//div[contains(@class,'content html_format')]").InnerText;*/

        }

        public void Show()
        {
            Console.Write("\nТема: " + this.Theme);

            Console.Write("\n\nНазвание статьи:\n" +this.Title);

            Console.Write("\n\nТеги:\n");
            foreach (var tag in this.Tags)
                Console.WriteLine("\t" + tag);

            Console.Write("\n\nДата публикации статьи: " + this.PublicationDate);

            //Console.Write("\n\nТекст статьи:\n" + this.ArticleText);

            Console.WriteLine("\n\n");
        }
    }
}
