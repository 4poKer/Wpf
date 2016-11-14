using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using HabrWpf.Model;


namespace HabrWpf.ViewModel
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged

        #region FindBtnClickCommand
        private RelayCommand _findBtnClickCommand;

        public ICommand FindBtnClickCommand
        {
            get
            {
                if (_findBtnClickCommand == null)
                {
                    _findBtnClickCommand = new RelayCommand(FindBtnClickCommandExecute, FindBtnClickCommandCanExecute);
                }
                return _findBtnClickCommand;
            }
        }

        public bool FindBtnClickCommandCanExecute(object sender)
        {
            return true;
        }

        private void FindBtnClickCommandExecute(object sender)
        {
          
            if (RadioButtonProperty=="FindById")
            {
                HabrArticle = HabrArticleController.FindArticleByIdFromText(SearchedText);
            }
            else
            {
                HabrArticle = HabrArticleController.FindArticleByKeyword(SearchedText);
            }

            OnPropertyChanged("HabrArticle");
        }
        #endregion FindBtnClickCommand

        #region RadioButtonChecked
        private string _radioButtonProperty;
        public string RadioButtonProperty
        {
            get { return _radioButtonProperty; }
            set { _radioButtonProperty = value; OnPropertyChanged("RadioButtonProperty"); }
        }
        #endregion RadioButtonChecked

        public string SearchedText { get; set; }

        public object HabrArticle { get; set; }
      
        public MainWindowViewModel()
        {

            RadioButtonProperty = "FindById";
     
        }

    }
}
