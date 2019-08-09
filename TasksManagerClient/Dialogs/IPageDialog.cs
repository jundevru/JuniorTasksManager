// Страницы внутри окна приложения

namespace TasksManagerClient.Dialogs
{
    interface IPageDialog
    {
        string Title { get;}
        void UpdatePropertyes();
    }
}
