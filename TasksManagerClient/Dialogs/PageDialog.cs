using System;
using System.Windows;
using System.Windows.Controls;

namespace TasksManagerClient.Dialogs
{
    /// <summary>
    /// Используется для отображения страниц в основном окне программы
    /// </summary>
    class PageDialog : DependencyObject
    {

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PageDialog), new PropertyMetadata(""));

        public UserControl View
        {
            get { return (UserControl)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
        }
        public static readonly DependencyProperty ViewProperty =
            DependencyProperty.Register("View", typeof(UserControl), typeof(PageDialog), new PropertyMetadata(null));

        IPageDialog dialog;

        public void ShowPage(IPageDialog dialog)
        {          
            #region Колхоз на тему: по быстрому найти подходящий View
            string viewTypeName = dialog.GetType().FullName.Replace("ViewModel", "View");
            Type viewType = Type.GetType(viewTypeName);
            View = Activator.CreateInstance(viewType) as UserControl;
            #endregion
            this.dialog = dialog;
            Title = dialog.Title;
            View.DataContext = dialog;
        }

    }
}
