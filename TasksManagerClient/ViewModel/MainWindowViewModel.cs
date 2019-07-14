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

        private AuthorizationViewModel avm;
        private RegistrationViewModel rvm = null;

        public MainWindowViewModel()
        {
            CurrentPage = new Dialogs.PageDialog();
            avm = new AuthorizationViewModel();
            avm.AuthorizationEndEvent += (user) => 
            {
                throw new NotImplementedException("Авторизация успешно пройдена");
            };
            avm.RegistrationRequiredEvent += () =>
            {
                if (rvm == null)
                {
                    rvm = new RegistrationViewModel();
                    rvm.CancelEvent += () =>
                    {
                        CurrentPage.ShowPage(avm);
                    };
                    rvm.RegistrationCompleteEvent += () =>
                    {
                        CurrentPage.ShowPage(avm);
                    };
                }
                CurrentPage.ShowPage(rvm);                
            };
            CurrentPage.ShowPage(avm);
        }
    }
}
