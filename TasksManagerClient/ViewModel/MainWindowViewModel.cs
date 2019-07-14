using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TasksManagerClient.DB;

namespace TasksManagerClient.ViewModel
{
    class MainWindowViewModel : DependencyObject
    {

        public Dialogs.PageDialog CurrentPage
        {
            get { return (Dialogs.PageDialog)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(Dialogs.PageDialog), typeof(MainWindowViewModel), new PropertyMetadata(null));


        public MainWindowViewModel()
        {
            CurrentPage = new Dialogs.PageDialog();
            AuthorizationViewModel avm = new AuthorizationViewModel();
            CurrentPage.ShowPage(avm);
        }
    }
}
