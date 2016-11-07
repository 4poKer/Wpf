using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HtmlAgilityPack;
using WpfHabrParser.Model;
using WpfHabrParser.Properties;
using WpfHabrParser.ViewModel;

namespace WpfHabrParser
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

                MessageBox.Show("Невозможно получить доступ к ресурсу!");
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

                MessageBox.Show("Error 404: Article not found =(");
                return null;

            }

        }

        public static HabrArticle GetDataById(int articleId)
        {

            var habrArticle = MongoDbDriver.GetHabrArticleByIdFromDb(articleId);

            if (habrArticle == null)
            {
                habrArticle = GetHabrArticleFromInternet(articleId);

                if(habrArticle != null)
                    MongoDbDriver.AddHabrArticleToDb(habrArticle);
            }

            return habrArticle;

        }

        public static HabrArticle FindArticleByIdFromText(string mayBeIdFromText)
        {
            int articleId;

            var isArticleId = Int32.TryParse(mayBeIdFromText, out articleId);

            if (isArticleId)
            {

                return GetDataById(articleId);
               
            }
            else
            {
                MessageBox.Show("Уникальный номер статьи введен неправильно!");
                return null;
            }
        }

        public static HabrArticle FindArticleByKeyword(string keyWord)
        {
            var searchPage = Resources.searchString + keyWord;

            var web = new HtmlWeb();
            var document = web.Load(searchPage);

            var searchArticle = document.DocumentNode.SelectSingleNode("//div[contains(@class,'post_teaser')]");

            if (searchArticle != null)
            {

                var articleId = Convert.ToInt32(searchArticle.Id.Split('_')[1]);

                return GetDataById(articleId);

            }
            else
            {
                MessageBox.Show("Не нашлось ни одной статьи по вашему запросу =(");

                return null;
            }
        }


    }
}
