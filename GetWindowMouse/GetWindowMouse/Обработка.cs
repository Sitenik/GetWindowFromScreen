using System;
using System.Windows.Forms;
using UtilWinApi.User32;

namespace GetWindowMouse
{
    public class Обработка : IDisposable
    {
        public InfoHwnd ТекущийВыбор = null;
        ФормаРамка рамка = null;

        Выбор фвыафыва = null;

        public InfoHwnd Do()
        {
            try
            {
                //рамка = new ФормаРамка();
                //if(рамка.ShowDialog() == DialogResult.OK) {
                //    return рамка.ТекущийВыбор;
                //}
                фвыафыва = new Выбор();
                фвыафыва.ShowDialog();
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка: " + exc.Message);
            }
            return null;
        }

        public void Dispose()
        {
            if (рамка != null)
            {
                рамка.Dispose();
            }
        }
    }
}
