using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GetWindowMouse
{
    public partial class ФормаРамка : Form
    {
        class Info
        {
            public Info(IntPtr hwnd) { this.Hwnd = hwnd; }
            public Rectangle Bounds { get { return GetRectangle(this.Hwnd); } }
            public IntPtr Hwnd;
            public string Title { get { return GetTitle(this.Hwnd); } }
            public bool Visible { get { return IsWindowVisible(this.Hwnd); } }
            public override string ToString()
            {
                return String.Format("{0,15}\t{1,-50}\t{2}", this.Hwnd, this.Bounds, this.Title);
            }
        }

        enum Gw : uint
        {
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, Gw uCmd);
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(System.Drawing.Point p);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        static string GetTitle(IntPtr hwnd)
        {
            var sb = new StringBuilder(GetWindowTextLength(hwnd) * 2);
            GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }
        [StructLayout(LayoutKind.Sequential)]
        struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        static Rectangle GetRectangle(IntPtr hwnd)
        {
            var r = new RECT();
            GetWindowRect(hwnd, out r);
            return Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
        }
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr dc);

        static IEnumerable<IntPtr> GetWindows(IntPtr hwnd, Gw gw)
        {
            for (var h = hwnd; h != IntPtr.Zero; h = GetWindow(h, gw))
                yield return h;
        }
        const byte РазмерРамки = 4;
        Rectangle размерРабочегоСтола = new Rectangle();
        Timer timer = null;
        public ФормаРамка()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.AliceBlue;//цвет фона  
            this.TransparencyKey = this.BackColor;//он же будет заменен на прозрачный цвет
            this.TopMost = true;
            размерРабочегоСтола = Screen.PrimaryScreen.WorkingArea;
            timer = new Timer() { Interval = 1000, Enabled = true };
            //timer.Tick += Timer_Tick;
        }

        void Print() {
            var pos = Cursor.Position;
            var i = new Info(WindowFromPoint(pos));
            var p = GetWindows(i.Hwnd, Gw.GW_HWNDNEXT)
                    .Select(h => new Info(h))
                    .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != this)
                    .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds));
            rtb.AppendText(String.Join("\r\n", p));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var pos = Cursor.Position;
            var i = new Info(WindowFromPoint(pos));
            var p = GetWindows(i.Hwnd, Gw.GW_HWNDNEXT)
                    .Select(h => new Info(h))
                    .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != this)
                    .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)).FirstOrDefault();
            //Print();
            //rtb.Text = String.Join("\n", p);

            if (p != null)
            {
                //timer.Stop();
                this.Correct(p.Bounds);
                //timer.Start();
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            Pen pen = new Pen(Color.Red, РазмерРамки);
            e.Graphics.DrawRectangle(pen, РазмерРамки / 2, РазмерРамки / 2, this.Width - РазмерРамки, this.Height - РазмерРамки);
            base.OnPaint(e);
        }

        private void ФормаРамка_Paint(object sender, PaintEventArgs e)
        {
            //Console.WriteLine("ФормаРамка_Paint");
            //e.Graphics.Clear(Color.AliceBlue);
            

            //var pos = Cursor.Position;
            //var i = new Info(WindowFromPoint(pos));

            //var p = GetWindows(i.Hwnd, Gw.GW_HWNDNEXT)
            //            .Select(h => new Info(h))
            //            .Where(v => v.Visible && v.Title != "F0H13A34" && !String.IsNullOrEmpty(v.Title))
            //            .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)).FirstOrDefault();
            //rtb.Text = String.Join("\n", p);
            //if (p != null)
            //{
            //    this.Correct(p.Bounds);
            //}
        }

        internal void Correct(Rectangle bounds)
        {
            this.Visible = true;
            
            this.Top = bounds.Top < размерРабочегоСтола.Top ? размерРабочегоСтола.Top : bounds.Top;
            this.Left = bounds.Left < размерРабочегоСтола.Left ? размерРабочегоСтола.Left : bounds.Left;
            this.Width = bounds.Width > размерРабочегоСтола.Width ? размерРабочегоСтола.Width : bounds.Width;
            this.Height = bounds.Height > размерРабочегоСтола.Height ? размерРабочегоСтола.Height : bounds.Height;
        }

        private void ФормаРамка_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Click...");
        }
    }
}
