using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MongoDB.Bson;

namespace HabrParser
{
    public static class HabrArticleController
    {

        public static HabrArticle GetHabrArticleFromInternet(int articleId)
        {

            HtmlDocument document;

            try
            {

                var web = new HtmlWeb();
                document = web.Load(Resources.postString + articleId + "/");

            }
            catch (System.Net.WebException ex)
            {

                Console.WriteLine("\nНевозможно получить доступ к ресурсу!\n");
                return null;

            }

            try
            {

                var tags = document.DocumentNode.SelectNodes("//a[contains(@class,'hub')]");
                var tempTags = new List<string>();                        
                tempTags.AddRange(tags.Select(x => x.InnerText));

             
                var habrArticle = new HabrArticle
                {
                    HabrId = articleId,
                    Theme = document.DocumentNode.SelectSingleNode("//h1[contains(@class,'post__title')]//a").InnerText,
                    Title = document.DocumentNode.SelectNodes("//h1[contains(@class,'post__title')]//span")[1].InnerText,
                    Tags = tempTags,
                    PublicationDate = document.DocumentNode.SelectSingleNode("//span[contains(@class,'post__time_published')]").InnerText

                };

                    return habrArticle;

                }
                catch (Exception ex)
                {

                    Console.WriteLine("\nError 404: Article not found =(\n");
                    return null;

                }

        }

        public static void Show(HabrArticle habrArticle)
        {
            if(habrArticle!=null){

                Console.Write("\nТема: " + habrArticle.Theme);

                Console.Write("\n\nНазвание статьи:\n" + habrArticle.Title);

                Console.Write("\n\nТеги:\n");
                foreach (var tag in habrArticle.Tags)
                    Console.WriteLine("\t" + tag);

                Console.Write("\n\nДата публикации статьи: " + habrArticle.PublicationDate);

                Console.WriteLine("\n\n");

            }
        }
    }
}
