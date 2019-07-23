using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TasksManagerClient.Dialogs;
using TasksManagerClient.Model;

namespace TasksManagerClient.ViewModel
{
    class MainWindowViewModel : DependencyObject, IPageDialogPresenter
    {

        /// <summary>
        /// Текущая отображаемая страница
        /// </summary>
        public PageDialog CurrentPage
        {
            get { return (PageDialog)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(PageDialog), typeof(MainWindowViewModel), new PropertyMetadata(null));


        /// <summary>
        /// Текущий авторизованный пользователь
        /// </summary>
        public User CurrentUser
        {
            get { return (User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(User), typeof(MainWindowViewModel), new PropertyMetadata(null));

        #region ICommands
        public ICommand ExitCommand => new Helpers.CommandsDelegate((obj) =>
        {
            if (CurrentUser == null)
                Application.Current.Shutdown();
            else
            {
                CurrentUser = null;
                CurrentPage.ShowPage(avm);
            }
        }, (obj) => { return true; });
        public ICommand TaskCommand => new Helpers.CommandsDelegate((obj) =>
        {
            CurrentPage.ShowPage(tvm);
        }, (obj) => { return true; });
        #endregion

        #region ViewModels 
        private AuthorizationViewModel avm;
        private RegistrationViewModel rvm = null;
        private TasksViewModel tvm = null;
        #endregion


        public MainWindowViewModel()
        {
            CurrentUser = null;

            tvm = new TasksViewModel(this);

            avm = new AuthorizationViewModel();
            avm.AuthorizationEndEvent += (user) => 
            {
                CurrentUser = user;
                ShowPage(tvm);
            };
            avm.RegistrationRequiredEvent += () =>
            {
                if (rvm == null)
                {
                    rvm = new RegistrationViewModel();
                    rvm.CancelEvent += () =>
                    {
                        ShowPage(avm);
                    };
                    rvm.RegistrationCompleteEvent += () =>
                    {
                        ShowPage(avm);
                    };
                }
                ShowPage(rvm);                
            };

            CurrentPage = new PageDialog();
            ShowPage(avm);
        }

        public void ShowPage(IPageDialog page)
        {
            CurrentPage.ShowPage(page);
        }
    }
}
