using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UtilWinApi.User32;

namespace GetWindowMouse
{
    public class Обработка
    {
        public InfoHwnd ТекущийВыбор = null;

        Timer timer = null;
        ФормаРамка рамка = null;

        private bool ПопаданиеМышыВФорму(Point мышь, InfoHwnd форма)
        {
            return форма.Bounds.Contains(мышь);
        }

        public void Do() 
        {
            var frm = new Form()
            {
                StartPosition = FormStartPosition.CenterScreen,
                Width = 800,
                Height = 500,
                Text = "TEST"
            };

            var rtb = new RichTextBox() { Parent = frm, Dock = DockStyle.Fill, WordWrap = false };
            timer = new Timer() { Interval = 1000, Enabled = true };
            рамка = new ФормаРамка();
            рамка.KeyDown += Рамка_KeyDown;
            рамка.MouseClick += Рамка_MouseClick;
            timer.Tick += (s, eе) =>
            {
                var pos = Cursor.Position;
                var i = new InfoHwnd(user32.WindowFromPoint(pos));
                var p = WinUser.GetWindows(i.Hwnd, user32.Gw.GW_HWNDNEXT)
                        .Select(h => new InfoHwnd(h))
                        .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != рамка &&  ПопаданиеМышыВФорму(pos, v))
                        .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)
                        ).FirstOrDefault();
                Принт(pos, rtb);
                if (p != null)
                {
                    рамка.Correct(p.Bounds);
                }
            };
            frm.ShowDialog();
        }

        private void Принт(Point pos, Control rtb)
        {
            var i = new InfoHwnd(user32.WindowFromPoint(pos));
            var p = WinUser.GetWindows(i.Hwnd, user32.Gw.GW_HWNDNEXT)
                        .Select(h => new InfoHwnd(h))
                        .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != рамка && ПопаданиеМышыВФорму(pos, v))
                        .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)
                        );
            (rtb as RichTextBox).Text = String.Join("\n", p);
        }

        private void Рамка_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                if(рамка != null) {
                    рамка.KeyDown -= Рамка_KeyDown;
                    рамка.MouseClick -= Рамка_MouseClick;
                    рамка.Dispose();
                    рамка = null;
                }
                if(timer != null) {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                }
            }
        }

        private void Рамка_MouseClick(object sender, MouseEventArgs e)
        {
            if (рамка != null)
            {
                рамка.KeyDown -= Рамка_KeyDown;
                рамка.MouseClick -= Рамка_MouseClick;
                рамка.Dispose();
                рамка = null;
            }
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
        }
    }
}
