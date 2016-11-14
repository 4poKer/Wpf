using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabrWpf.Model;
using MongoDB.Driver;
using System.Configuration;

namespace HabrWpf
{
    public static class MongoDbDriver
    {
        private static readonly string _dbName = ConfigurationManager.AppSettings["dbName"];

        private static IMongoDatabase _mongoDatabase;

        private static IMongoCollection<HabrArticle> _habrArticleCollection;

        private static void Connect()
        {

            IMongoClient client = new MongoClient();
            _mongoDatabase = client.GetDatabase(_dbName);

        }

        public static HabrArticle GetHabrArticleByIdFromDb(int articleId)
        {
            Connect();

            _habrArticleCollection = _mongoDatabase?.GetCollection<HabrArticle>(_dbName);

            return _habrArticleCollection?.AsQueryable()?.FirstOrDefault(ha => ha.HabrId == articleId);

        }

        public static void AddHabrArticleToDb(HabrArticle habrArticle)
        {
            Connect();

            _habrArticleCollection?.InsertOne(habrArticle);

        }

    }
}
