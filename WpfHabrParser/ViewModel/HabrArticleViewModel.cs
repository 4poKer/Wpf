using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using WpfHabrParser.Model;

namespace WpfHabrParser.ViewModel
{
    public class HabrArticleViewModel : INotifyPropertyChanged
    { 
   
        public event PropertyChangedEventHandler PropertyChanged;

        public HabrArticle HabrArticle;

        public HabrArticleViewModel(HabrArticle habrArticle)
        {
            HabrArticle = habrArticle;
        }

        public ObjectId Id { get; set; }

        public int HabrId
        {
            get { return HabrArticle.HabrId; }
            set
            {
                HabrArticle.HabrId = value;
                OnChangeProperty("HabrId");
            }
        }

        public string Theme
        {
            get { return HabrArticle.Theme; }
            set
            {
                HabrArticle.Theme = value;
                OnChangeProperty("Theme");
            }
        }

        public string Title
        {
            get { return HabrArticle.Title; }
            set
            {
                HabrArticle.Title = value;
                OnChangeProperty("Title");
            }
        }

        public List<string> Tags
        {
            get { return HabrArticle.Tags; }
            set
            {
                HabrArticle.Tags = value;
                OnChangeProperty("Tags");
            }
        }

        public string PublicationDate
        {
            get { return HabrArticle.PublicationDate; }
            set
            {
                HabrArticle.PublicationDate = value;
                OnChangeProperty("PublicationDate");
            }
        }

        private void OnChangeProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
