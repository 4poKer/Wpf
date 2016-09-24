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

            HtmlDocument document=null;

            try
            {
                HtmlWeb web = new HtmlWeb();
                document = web.Load("http://www.habrahabr.ru/post/" + ArticleId + "/");
            }
            catch (System.Net.WebException ex)
            {

                Console.WriteLine("\nНевозможно получить доступ к ресурсу!\n");

            }

            try
            {
                //Поиск названия темы
                var Theme = document.DocumentNode.SelectSingleNode("//h1[contains(@class,'post__title')]//a");
                Console.Write("\nТема: " + Theme.InnerText);

                //Поиск названия статьи
                var SpansInTitle = document.DocumentNode.SelectNodes("//h1[contains(@class,'post__title')]//span");
                Console.Write("\n\nНазвание статьи:\n" + SpansInTitle[1].InnerText);

                //Поиск тегов
                var Tegs = document.DocumentNode.SelectNodes("//a[contains(@class,'hub')]");
                Console.Write("\n\nТеги:\n");
                foreach (var teg in Tegs)
                    Console.WriteLine("\t" + teg.InnerText);

                //Поиск даты публикации
                var PublicationDate = document.DocumentNode.SelectSingleNode("//span[contains(@class,'post__time_published')]");
                Console.Write("\n\nДата публикации статьи: " + PublicationDate.InnerText);

                //Поиск статьи
                var ArticleText = document.DocumentNode.SelectSingleNode("//div[contains(@class,'content html_format')]");
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

            try
            {
                ArticleId = Convert.ToInt32(Console.ReadLine());
            }
            catch (System.FormatException e) {

                Console.Write("\nУникальный номер статьи введен неправильно!\n\n");
                return;
            }

            GetDataById(ArticleId);

        }

        static void FindArticleByKeyword(){

            Console.Write("Введите слово/фразу для поиска статьи: ");
            
            string SearchPage = "http://www.habrahabr.ru/search/?q=" + Console.ReadLine();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(SearchPage);

            var SearchArticle = document.DocumentNode.SelectSingleNode("//div[contains(@class,'post_teaser')]");

            if (SearchArticle != null){

                int ArticleId = Convert.ToInt32(SearchArticle.Id.Split('_')[1]);

                GetDataById(ArticleId);

            }
            else {

                Console.Write("\nНе нашлось ни одной статьи по вашему запросу =(");

            }

        }

        static void Main(string[] args)
        {
            string Menu = "Поиск статей на Хабре\n 1)Найти статью по уникальному номеру\n 2)Поиск статьи по ключевой фразе\n 3)Выход\nВыберите действие(1-3): ";

            Console.Write(Menu);

            int action = 0; //операция в меню

            try
            {
                action = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
            }

            bool isExit = false;

            while (!isExit)
            {

                while (action > 3 || action < 1)
                {
                    Console.Write("\nНеизвестное значение операции!\n\n");
                    Console.Write(Menu);
                    try
                    {
                        action = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                    }
                }

                switch (action)
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


                if (action != 3)
                {
                    Console.Write(Menu);
                    try
                    {
                        action = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }
    }
}
