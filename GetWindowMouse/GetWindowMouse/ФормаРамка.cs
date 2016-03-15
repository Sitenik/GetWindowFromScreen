using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinApiCom;

namespace GetWindowMouse
{
    public partial class ФормаРамка : Form
    {
        const byte РазмерРамки = 4;
        Rectangle размерРабочегоСтола = new Rectangle();
        Timer timer = null;
        public InfoHwnd ТекущийВыбор = null;
        InfoHwnd ПретендентНаТекущийВыбор = null;
        bool ИдетОбработка = false;

        public ФормаРамка()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.AliceBlue;//цвет фона  
            this.TransparencyKey = this.BackColor;//он же будет заменен на прозрачный цвет
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual;
            размерРабочегоСтола = Screen.PrimaryScreen.WorkingArea;
            Timer_Tick(null, null);
            timer = new Timer() { Interval = 500, Enabled = true};
            timer.Tick += Timer_Tick;
        }

        private void Принт(Point pos, Control rtb)
        {
            var i = new InfoHwnd(User32.WindowFromPoint(pos));
            var p = User32Helper.GetWindows(i.Hwnd, User32.Gw.GW_HWNDNEXT)
                        .Select(h => new InfoHwnd(h))
                        .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != this && ПопаданиеМышыВФорму(pos, v) && !String.IsNullOrEmpty(v.Title))
                        .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)
                        );
            //(rtb as RichTextBox).Text = String.Join("\n", p);
        }


        private void Timer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                if(ИдетОбработка) {
                    return;
                }
                ИдетОбработка = true;
                var pos = Cursor.Position;
                var i = new InfoHwnd(User32.WindowFromPoint(pos));
                var p = User32Helper.GetWindows(i.Hwnd, User32.Gw.GW_HWNDNEXT)
                        .Select(h => new InfoHwnd(h))
                        .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != this && ПопаданиеМышыВФорму(pos, v) && !String.IsNullOrEmpty(v.Title))
                        .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)
                        ).FirstOrDefault();
                if (p != null)
                {
                    this.Correct(p.Bounds);
                    ПретендентНаТекущийВыбор = p;
                }
            }
            catch (System.Exception exc)
            {
                MessageBox.Show("Ошибка: " + exc.Message);
            }
            finally {
                ИдетОбработка = false;
            }
        }

        internal void Correct(Rectangle bounds)
        {
            //this.Visible = true;
            this.Top = bounds.Top < размерРабочегоСтола.Top ? размерРабочегоСтола.Top : bounds.Top;
            this.Left = bounds.Left < размерРабочегоСтола.Left ? размерРабочегоСтола.Left : bounds.Left;
            this.Width = bounds.Width > размерРабочегоСтола.Width ? размерРабочегоСтола.Width : bounds.Width;
            this.Height = bounds.Height > размерРабочегоСтола.Height ? размерРабочегоСтола.Height : bounds.Height;
        }

        internal bool ПопаданиеМышыВФорму(Point мышь, InfoHwnd форма)
        {
            return форма.Bounds.Contains(мышь);
        }

        private void ФормаРамка_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                DialogResult = DialogResult.Cancel;
                ТекущийВыбор = null;
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                }
            }
            this.Close();
        }

        private void ФормаРамка_MouseClick(object sender, MouseEventArgs e)
        {
            ТекущийВыбор = ПретендентНаТекущийВыбор;
            DialogResult = DialogResult.OK;
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }
            this.Close();
        }
    }
}
