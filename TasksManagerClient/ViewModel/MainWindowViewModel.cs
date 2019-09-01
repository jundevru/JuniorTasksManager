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
using TasksManagerClient.ApplicationLogic;
using TasksManagerClient.Statics;
using System.Windows.Media;

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


        #region ICommands
        /// <summary>
        /// Выход из пользовалеля (приложения)
        /// </summary>
        public ICommand ExitCommand => new Helpers.CommandsDelegate((obj) =>
        {
            if (CurrentUser.Instance.User == null)
                Application.Current.Shutdown();
            else
            {
                cvm.Disconnect();
                CurrentUser.Instance.User = null;
                stPage.StartDialog();
            }
        }, (obj) => { return true; });
        /// <summary>
        /// Переход к списку задач
        /// </summary>
        public ICommand TaskCommand => new Helpers.CommandsDelegate((obj) =>
        {
            CurrentPage.ShowPage(tvm);
        }, (obj) => { return CurrentUser.Instance.User != null; });
        /// <summary>
        /// Переход к списку сообщений
        /// </summary>
        public ICommand MessagesCommand => new Helpers.CommandsDelegate((obj) =>
        {
            CurrentPage.ShowPage(cvm);
        }, (obj) => { return CurrentUser.Instance.User != null; });
        #endregion

        #region Logic
        StartTurnOffPageDialogLogic stPage;
        #endregion

        TasksViewModel tvm;
        ChatViewModel cvm;

        public MainWindowViewModel()
        {
            CurrentPage = new PageDialog();
            stPage = new StartTurnOffPageDialogLogic(this);
            stPage.EndEvent += (res) => {
                if (res == PageDialogResult.Completed)
                {
                    tvm = new TasksViewModel(this);
                    UpdatesMessagesManager.Instance.Add(tvm);
                    cvm = new ChatViewModel();
                    UpdatesMessagesManager.Instance.Add(cvm);
                    CurrentPage.ShowPage(tvm);
                }
                else
                    throw new NotImplementedException("Диалог авторизации не должен возвращать отмену");
            };
            stPage.StartDialog();
        }

        public void ShowPage(IPageDialog page)
        {
            page.UpdatePropertyes();
            CurrentPage.ShowPage(page);
        }
    }
}
