using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GetWindowMouse;
using UtilWinApi.User32;

namespace GetWindowMouse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Выбор : Window
    {
        System.Drawing.Rectangle размерРабочегоСтола = new System.Drawing.Rectangle();
        const byte РазмерРамки = 4;
        System.Windows.Forms.Timer timer = null;
        public InfoHwnd ТекущийВыбор = null;
        InfoHwnd ПретендентНаТекущийВыбор = null;
        bool ИдетОбработка = false;

        public Выбор()
        {
            InitializeComponent();

            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency  = true;
            this.Background = new SolidColorBrush(Color.FromArgb(0, 34, 34, 34));
            //this.TransparencyKey = this.BackColor;//он же будет заменен на прозрачный цвет
            this.Topmost = true;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.BorderThickness = new Thickness(РазмерРамки);


            //размерРабочегоСтола = Screen.PrimaryScreen.WorkingArea;
            //Timer_Tick(null, null);
            //timer = new Timer() { Interval = 500, Enabled = true };
            //timer.Tick += Timer_Tick;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
    }
}
