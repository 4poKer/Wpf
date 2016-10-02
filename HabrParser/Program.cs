using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HabrParser
{
    class Program
    {
        static void GetDataById(int ArticleId) {

            HtmlDocument Document=null;

            try
            {
                HtmlWeb Web = new HtmlWeb();
                Document = Web.Load(Resources.PostString + ArticleId + "/");
            }
            catch (System.Net.WebException ex)
            {

                Console.WriteLine("\nНевозможно получить доступ к ресурсу!\n");

            }

            try
            {
                //Поиск названия темы
                var Theme = Document.DocumentNode.SelectSingleNode("//h1[contains(@class,'post__title')]//a");
                Console.Write("\nТема: " + Theme.InnerText);

                //Поиск названия статьи
                var SpansInTitle = Document.DocumentNode.SelectNodes("//h1[contains(@class,'post__title')]//span");
                Console.Write("\n\nНазвание статьи:\n" + SpansInTitle[1].InnerText);

                //Поиск тегов
                var Tegs = Document.DocumentNode.SelectNodes("//a[contains(@class,'hub')]");
                Console.Write("\n\nТеги:\n");
                foreach (var teg in Tegs)
                    Console.WriteLine("\t" + teg.InnerText);

                //Поиск даты публикации
                var PublicationDate = Document.DocumentNode.SelectSingleNode("//span[contains(@class,'post__time_published')]");
                Console.Write("\n\nДата публикации статьи: " + PublicationDate.InnerText);

                //Поиск статьи
                var ArticleText = Document.DocumentNode.SelectSingleNode("//div[contains(@class,'content html_format')]");
                Console.Write("\n\nТекст статьи:\n" + ArticleText.InnerText);
                Console.WriteLine("\n\n");
            }
            catch(Exception e) {

                Console.WriteLine("\nError 404: Article not found =(\n");
            }
        }

        static void FindArticleById() {

            Console.Write("\nВведите уникальный номер статьи: ");

            int ArticleId=0;

            bool isArticleId = Int32.TryParse(Console.ReadLine(), out ArticleId);

            if(isArticleId)
                GetDataById(ArticleId);
            else
                Console.Write("\nУникальный номер статьи введен неправильно!\n\n");
        }

        static void FindArticleByKeyword(){

            Console.Write("Введите слово/фразу для поиска статьи: ");
            
            string SearchPage = Resources.SearchString + Console.ReadLine();

            HtmlWeb Web = new HtmlWeb();
            HtmlDocument Document = Web.Load(SearchPage);

            var SearchArticle = Document.DocumentNode.SelectSingleNode("//div[contains(@class,'post_teaser')]");

            if (SearchArticle != null){

                int ArticleId = Convert.ToInt32(SearchArticle.Id.Split('_')[1]);

                GetDataById(ArticleId);

            }
            else 
                Console.Write("\nНе нашлось ни одной статьи по вашему запросу =(");

        }

        static void Main(string[] args)
        {
           bool isExit = false;

           while (!isExit)
           {

               Console.Write(Resources.MenuString);

               int Action; //операция в меню

               bool isAction = Int32.TryParse(Console.ReadLine(), out Action);

               if (isAction)
               {

                   while (Action > 3 || Action < 1)
                   {
                       Console.Write("\nНеизвестное значение операции!\n\n");
                       Console.Write(Resources.MenuString);

                       isAction = Int32.TryParse(Console.ReadLine(), out Action);
                   }

                   switch (Action)
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
