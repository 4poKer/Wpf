using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;


namespace HabrParser
{

    class Program
    {
        static void GetDataById(int articleId) {

            var client = new MongoClient(Resources.mongoDbConnectionString);

            var server = client.GetServer();
            var mongoDatabase = server.GetDatabase(Resources.dbName);

            var habrArticleCollection = mongoDatabase.GetCollection<HabrArticle>("HabrArticle");

            HabrArticle habrArticle = null;

            habrArticle = habrArticleCollection.AsQueryable().FirstOrDefault(ha => ha.HabrId == articleId);

            if (habrArticle == null)
            {
                HtmlDocument document = null;

                try
                {
                    HtmlWeb web = new HtmlWeb();
                    document = web.Load(Resources.postString + articleId + "/");
                }
                catch (System.Net.WebException ex)
                {

                    Console.WriteLine("\nНевозможно получить доступ к ресурсу!\n");

                }

                try
                {

                    habrArticle = new HabrArticle(document, articleId);
                    habrArticleCollection.Insert(habrArticle);
                    habrArticle.Show();

                }
                catch (Exception e)
                {

                    Console.WriteLine("\nError 404: Article not found =(\n");
                }
            }
            else
            {
                habrArticle.Show();
            }
        }

        static void FindArticleById() {

            Console.Write("\nВведите уникальный номер статьи: ");

            int articleId=0;

            bool isArticleId = Int32.TryParse(Console.ReadLine(), out articleId);

            if(isArticleId)
                GetDataById(articleId);
            else
                Console.Write("\nУникальный номер статьи введен неправильно!\n\n");
        }

        static void FindArticleByKeyword(){

            Console.Write("Введите слово/фразу для поиска статьи: ");
            
            string searchPage = Resources.searchString + Console.ReadLine();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(searchPage);

            var searchArticle = document.DocumentNode.SelectSingleNode("//div[contains(@class,'post_teaser')]");

            if (searchArticle != null){

                int articleId = Convert.ToInt32(searchArticle.Id.Split('_')[1]);

                GetDataById(articleId);

            }
            else 
                Console.Write("\nНе нашлось ни одной статьи по вашему запросу =(");

        }

        static void Main(string[] args)
        {

           bool isExit = false;

           while (!isExit)
           {

               Console.Write(Resources.menuString);

               int menuAction; //операция в меню

               bool isAction = Int32.TryParse(Console.ReadLine(), out menuAction);

               if (isAction)
               {

                   while (menuAction > 3 || menuAction < 1)
                   {
                       Console.Write("\nНеизвестное значение операции!\n\n");
                       Console.Write(Resources.menuString);

                       Int32.TryParse(Console.ReadLine(), out menuAction);
                   }

                   switch (menuAction)
                   {
                       case 1:

                           FindArticleById();
                           break;

                       case 2:

                           FindArticleByKeyword();
                           break;

                       case 3:

                           Console.WriteLine("\nСпасибо, что воспользовались нашим сервисом!\nДля прололжения нажмите любую клавишу");
                           Console.ReadLine();
                           isExit = true;
                           break;
                   }

               }
               else
                   Console.Write("\nНеизвестное значение операции!\n\n");

           }

        }
    }
}
