using System;
using System.Windows;

namespace TasksManagerClient.Logs
{
    public class MessageBoxLogger : ILogger
    {
        public void Write(string message, Exception ex)
        {
            MessageBox.Show(ex.Message, message);
        }
    }
}
