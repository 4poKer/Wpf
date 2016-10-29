using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using static HabrParser.HabrModel;

namespace HabrParser
{
    public static class HabrController
    {
        public static HabrArticle GetHabrArticleByIdFromInternet(int articleId)
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

                //получение NickName автора
                var autorNickName = GetAutorNickName(document);

                var habrArticle = new HabrArticle
                {
                    HabrArticleId = articleId,
                    Theme = document.DocumentNode.SelectSingleNode("//h1[contains(@class,'post__title')]//a").InnerText,
                    Title = document.DocumentNode.SelectNodes("//h1[contains(@class,'post__title')]//span")[1].InnerText,
                    Tags = tempTags,
                    PublicationDate =
                        document.DocumentNode.SelectSingleNode("//span[contains(@class,'post__time_published')]")
                            .InnerText,


                    HabrAutor = GetHabrAutorByNickName(autorNickName)
                };

                //добавление в БД
                var habrContext = new HabrContext();
                habrContext.HabrArticles.Add(habrArticle);
                habrContext.SaveChanges();
                //

                return habrArticle;

            }
            catch (Exception ex)
            {

                Console.WriteLine("\nError 404: Article not found =(\n");
                return null;

            }

        }

        public static HabrAutor GetHabrAutorByNickName(string autorNickName)
        {
            var habrContext = new HabrContext();

            var habrAutor = habrContext.HabrAutors.FirstOrDefault(a => a.NickName == autorNickName);

            if (habrAutor == null)
                habrAutor = GetHabrAutorByNickNameFromInternet(autorNickName);

            habrContext.Dispose();
           
            return habrAutor;
        }

        public static HabrAutor GetHabrAutorByNickNameFromInternet(string autorNickName)
        {

                var web = new HtmlWeb();
                var document = web.Load(Resources.userString + autorNickName + "/");

                autorNickName = GetAutorNickName(document);

                if (autorNickName == null)
                {
                    Console.WriteLine("\nAutor not found=(\n");

                    return null;
                }

                string name;

                try
                {
                    name = document.DocumentNode.SelectSingleNode("//a[contains(@class,'author-info__name')]").InnerText.Split(' ')[0];
                }
                catch (Exception e)
                {
                    name = null;
                }

                string surname;

                try
                {
                    surname = document.DocumentNode.SelectSingleNode("//a[contains(@class,'author-info__name')]").InnerText.Split(' ')[1];
                }
                catch (Exception e)
                {
                    surname = null;
                }

                string aboutInfo;

                try
                {
                    aboutInfo = document?.DocumentNode?.SelectSingleNode("//dd[contains(@class,'summary')]").InnerText;
                }
                catch (Exception e)
                {
                    aboutInfo = null;
                }


            try
                {
                    var habrAutor = new HabrAutor
                    {
                        NickName = autorNickName,
                        Name = name,
                        Surname = surname,
                        AboutInfo = aboutInfo,
                    };

                    //добавление в БД
                    var habrContext = new HabrContext();
                    habrContext.HabrAutors.Add(habrAutor);
                    habrContext.SaveChanges();
                    //
 
                    habrContext.Dispose();

                return habrAutor;


                }
                catch (Exception e)
                {

                    return new HabrAutor {NickName = autorNickName,};
                }
        }

        public static string GetAutorNickName(HtmlDocument document)
        {
            string autorNickName = null;

            try
            {
                autorNickName = document.DocumentNode.SelectSingleNode("//a[contains(@class,'author-info__nickname')]").InnerText;
            }
            catch (Exception e)
            {
                try
                {
                    autorNickName = document.DocumentNode.SelectSingleNode("//a[contains(@class,'post-type__value_author')]").InnerText;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            autorNickName = autorNickName?.Trim('@');
            return autorNickName;
        }

        public static void ShowHabrArticle(HabrArticle habrArticle)
        {
            if(habrArticle!= null){

                Console.Write("\nТема: " + habrArticle.Theme);

                Console.Write("\n\nНазвание статьи: " + habrArticle.Title);

                Console.Write("\n\nТеги:\n");
                foreach (var tag in habrArticle.Tags)
                    Console.WriteLine("\t" + tag);

                Console.Write("\n\nДата публикации статьи: " + habrArticle.PublicationDate);

                Console.Write("\n\nНик автора: " + habrArticle.HabrAutor.NickName);

                Console.WriteLine("\n\n");

            }
        }

        public static void ShowHabrAutor(HabrAutor habrAutor)
        {
            if (habrAutor != null)
            {

                Console.Write("\nНик: " + habrAutor.NickName);

                if(habrAutor.Name != null)
                {
                    Console.Write("\n\nИмя: " + habrAutor.Name);
                }

                if (habrAutor.Surname != null)
                {
                    Console.Write("\n\nФамилия: " + habrAutor.Surname);
                }

                if (habrAutor.AboutInfo != null)
                {
                    Console.Write("\n\nИнформация об авторе:\n" + habrAutor.AboutInfo);
                }

                Console.WriteLine("\n\n");

            }
        }
    }
}
