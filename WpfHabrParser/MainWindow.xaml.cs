using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using WpfHabrParser.Model;
using WpfHabrParser.ViewModel;

namespace WpfHabrParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HabrArticle HabrArticle { get; set; }
        //private HabrArticleView HabrArticleView { get; set; }

        public Dictionary<int, string> CommandType;
        public int RadioButtonCode;

        public MainWindow()
        {
            InitializeComponent();

            CommandType = new Dictionary<int, string>()
            {
                {0,"Введите уникальный номер статьи:"},
                {1,"Введите слово/фразу для поиска статьи:"}
            };

            SearchingLabel.Content = CommandType[RadioButtonCode];

            //HabrArticleView = new HabrArticleView();

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           

            var radioButton = sender as RadioButton;

            RadioButtonCode = Convert.ToInt32(radioButton.Tag);

            try
            {
                HabrArticleViewItem.DataContext = null;
                
                SearchingLabel.Content = CommandType[RadioButtonCode];
            }
            catch (Exception ex){}
        }


        private void FindArticleButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (RadioButtonCode == 0) 
            {

                HabrArticle = HabrArticleController.FindArticleByIdFromText(SearchingTextBox.Text);

            }
            else
            {

                HabrArticle = HabrArticleController.FindArticleByKeyword(SearchingTextBox.Text);
            }


            if (HabrArticle != null)
            {
                //HabrArticleView.DataContext = HabrArticle;

                HabrArticleViewItem.DataContext = HabrArticle;

                //this.DataContext = HabrArticle;

            }
        }
    }
}
