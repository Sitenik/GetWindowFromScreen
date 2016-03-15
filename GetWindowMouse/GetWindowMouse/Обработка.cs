using System;
using System.Windows.Forms;
using WinApiCom;

namespace GetWindowMouse
{
    public class Обработка : IDisposable
    {
        public InfoHwnd ТекущийВыбор = null;
        ФормаРамка рамка = null;

        public InfoHwnd Do()
        {
            try
            {
                рамка = new ФормаРамка();
                if (рамка.ShowDialog() == DialogResult.OK)
                {
                    return рамка.ТекущийВыбор;
                }
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
