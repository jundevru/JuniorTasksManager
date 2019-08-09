
using System;
/// <summary>
/// Логика диалогов
/// </summary>
namespace TasksManagerClient.ApplicationLogic
{
    enum PageDialogResult
    {
        Completed,
        Canceled
    }
    interface IPageDialogLogic
    {
        event Action<PageDialogResult> EndEvent;
        void StartDialog();

    }
}
