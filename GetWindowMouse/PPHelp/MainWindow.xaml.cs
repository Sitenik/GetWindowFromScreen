using System.Windows;
using GetWindowMouse;
using UtilWinApi.User32;

namespace PPHelp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public InfoHwnd ОкноПриложения = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (Обработка выбратьОкно = new Обработка()) {
                ОкноПриложения = выбратьОкно.Do();
                if(ОкноПриложения == null) {
                    lb.Content = "Выберите окно";
                    return;
                }
                else {
                    lb.Content = ОкноПриложения;
                }
            }
        }
    }
}
