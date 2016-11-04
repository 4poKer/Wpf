using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HabrParser
{
    class HabrContext : DbContext
    {
        // Имя будущей базы данных можно указать через
        // вызов конструктора базового класса
        public HabrContext() : base("HabrHabr")
        { }

        // Отражение таблиц базы данных на свойства с типом DbSet
        public DbSet<HabrAutor> HabrAutors { get; set; }
        public DbSet<HabrArticle> HabrArticles { get; set; }

    }
}
