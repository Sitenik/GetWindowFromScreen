using System;
using System.Windows.Forms;
using GetMainWin;

namespace GetMainWinTest
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ОбработкаВыбораОкна тест = new ОбработкаВыбораОкна();
            тест.Do();
        }
    }
}
