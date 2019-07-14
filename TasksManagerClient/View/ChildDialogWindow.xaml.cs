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
using System.Windows.Shapes;

namespace TasksManagerClient.View
{
    /// <summary>
    /// Логика взаимодействия для ChildDialogWindow.xaml
    /// </summary>
    public partial class ChildDialogWindow : Window
    {
        public ChildDialogWindow()
        {
            InitializeComponent();
            CloseButton.Click += (s, e) => Close();
        }
    }
}
