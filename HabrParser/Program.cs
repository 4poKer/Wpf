using System;
using System.Linq;
using HtmlAgilityPack;


namespace HabrParser
{

    public class Program
    {
        public static void GetArticleById(int articleId)
        {

            HabrContext habrContext = new HabrContext();

            var habrArticle = habrContext.HabrArticles.FirstOrDefault(a => a.HabrArticleId == articleId);

            if (habrArticle == null)
                habrArticle = HabrController.GetHabrArticleByIdFromInternet(articleId);


            HabrController.ShowHabrArticle(habrArticle);
        }

        public static void FindArticleById()
        {

            Console.Write("\nВведите уникальный номер статьи: ");

            var articleId = 0;

            bool isArticleId = Int32.TryParse(Console.ReadLine(), out articleId);

            if (isArticleId)
                GetArticleById(articleId);
            else
                Console.Write("\nУникальный номер статьи введен неправильно!\n\n");
        }

        public static void FindArticleByKeyword()
        {

            Console.Write("Введите слово/фразу для поиска статьи: ");

            string searchPage = Resources.searchString + Console.ReadLine();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load(searchPage);

            var searchArticle = document.DocumentNode.SelectSingleNode("//div[contains(@class,'post_teaser')]");

            if (searchArticle != null)
            {

                int articleId = Convert.ToInt32(searchArticle.Id.Split('_')[1]);

                GetArticleById(articleId);

            }
            else
                Console.Write("\nНе нашлось ни одной статьи по вашему запросу =(");

        }

        public static void FindArticlesByAutorNickName()
        {

            Console.Write("\nВведите ник автора: ");

            var nickName = Console.ReadLine();

            HabrContext habrContext = new HabrContext();

            var  habrArticlesList =  habrContext.HabrArticles.Where(a => a.HabrAutor.NickName == nickName);

            if (habrArticlesList.Count() > 0)
            {
                for(int i = 0;i< habrArticlesList.Count();++i)
                {
                    Console.WriteLine("\n//////////" + (i+1) + "//////////");
                    HabrController.ShowHabrArticle(habrArticlesList.ToList()[i]);
                }
            }
            else
            {
                Console.WriteLine("\nНет сохраненных статей автора "+ nickName +"\n");
            }

        }

        public static void FindAutorByNickName()
        {
            Console.Write("\nВведите ник автора: ");

            var habrAutor = HabrController.GetHabrAutorByNickName(Console.ReadLine());

            HabrController.ShowHabrAutor(habrAutor);
        }

        public static void Main(string[] args)
        {

            var isExit = false;

            while (!isExit)
            {

                Console.Write(Resources.menuString);

                int menuAction; //операция в меню

                var isAction = Int32.TryParse(Console.ReadLine(), out menuAction);

                if (isAction)
                {

                    while (menuAction > 5 || menuAction < 1)
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

                        FindAutorByNickName();
                        break;

                     case 4:

                        FindArticlesByAutorNickName();
                        break;

                     case 5:

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