using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilWinApi.User32;

namespace GetWindowMouse
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            //AllowTransparency = true;
            //BackColor = Color.AliceBlue;//цвет фона  
            //TransparencyKey = this.BackColor;//он же будет заменен на прозрачный цвет
            this.Location = КоординатыВЦентреФормы();
        }

        private Point КоординатыВЦентреФормы()
        {
            var координатыМышы = Cursor.Position;
            Point новаяТочка = new Point();
            новаяТочка.X = координатыМышы.X - this.Width / 2;
            новаяТочка.Y = координатыМышы.Y - this.Height / 2;
            return новаяТочка;
        }

        private IntPtr НайтиОкноПодФормой()
        {
            var pos = Cursor.Position;
            var i = new InfoHwnd(user32.WindowFromPoint(pos));
            var p = WinUser.GetWindows(i.Hwnd, user32.Gw.GW_HWNDNEXT)
                    .Select(h => new InfoHwnd(h))
                    .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != this)
                    .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)
                    );
            rtb.AppendText(Environment.NewLine + String.Join("\n", p));

            return user32.GetWindow(this.Handle, user32.Gw.GW_HWNDNEXT);
        }

        private void ИзменитьРазмерПоОкну()
        {

        }

        private void Form2_MouseLeave(object sender, EventArgs e)
        {
            this.Location = КоординатыВЦентреФормы();
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = Cursor.Position;
            var i = new InfoHwnd(user32.WindowFromPoint(pos));
            var p = WinUser.GetWindows(i.Hwnd, user32.Gw.GW_HWNDNEXT)
                    .Select(h => new InfoHwnd(h))
                    .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != this && ПопаданиеМышыВФорму(pos, v))
                    .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)
                    );
            rtb.Text = String.Format("Мышь X = {0}, Y = {1}{2}:", pos.X, pos.Y, Environment.NewLine) + String.Join("\n", p);
        }

        private bool ПопаданиеМышыВФорму(Point мышь, InfoHwnd форма)
        {
            return форма.Bounds.Contains(мышь);
        }
    }
}
