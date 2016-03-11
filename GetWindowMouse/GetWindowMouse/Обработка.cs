using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Norfy.GDI;

namespace GetWindowMouse
{
    public class Обработка
    {
        public static class winapi
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
            public static extern bool Ellipse(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
            [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
            public static extern bool LineTo(IntPtr hdc, int nLeftRect, int nTopRect);
            [DllImport("user32.dll", EntryPoint = "GetDC")]
            public static extern IntPtr GetDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);
            [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
            public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr CreateWindowEx(
               uint dwExStyle,
               IntPtr lpClassName,
               string lpWindowName,
               uint dwStyle,
               int x,
               int y,
               int nWidth,
               int nHeight,
               IntPtr hWndParent,
               IntPtr hMenu,
               IntPtr hInstance,
               IntPtr lpParam
            );
            public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll", EntryPoint = "ReleaseDC", SetLastError = true)]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern ushort RegisterClass([In] ref WNDCLASS lpWndClass);
            [DllImport("user32.dll")]
            public static extern IntPtr DefWindowProc(IntPtr hWnd, IntPtr uMsg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);
            [DllImport("user32.dll")]
            public static extern bool UpdateWindow(IntPtr hWnd);
            [StructLayout(LayoutKind.Sequential)]
            public struct WNDCLASS
            {
                public ClassStyles style;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public WndProcDelegate lpfnWndProc;
                public int cbClsExtra;
                public int cbWndExtra;
                public IntPtr hInstance;
                public IntPtr hIcon;
                public IntPtr hCursor;
                public IntPtr hbrBackground;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string lpszMenuName;
                [MarshalAs(UnmanagedType.LPTStr)]
                public string lpszClassName;
            }
            [Flags]
            public enum ClassStyles : uint
            {
                ByteAlignClient = 0x1000,
                ByteAlignWindow = 0x2000,
                ClassDC = 0x40,
                DoubleClicks = 0x8,
                DropShadow = 0x20000,
                GlobalClass = 0x4000,
                HorizontalRedraw = 0x2,
                NoClose = 0x200,
                OwnDC = 0x20,
                ParentDC = 0x80,
                SaveBits = 0x800,
                VerticalRedraw = 0x1
            }
            [Flags()]
            public enum WindowStyles : uint
            {
                WS_BORDER = 0x800000,
                WS_CAPTION = 0xc00000,
                WS_CHILD = 0x40000000,
                WS_CLIPCHILDREN = 0x2000000,
                WS_CLIPSIBLINGS = 0x4000000,
                WS_DISABLED = 0x8000000,
                WS_DLGFRAME = 0x400000,
                WS_GROUP = 0x20000,
                WS_HSCROLL = 0x100000,
                WS_MAXIMIZE = 0x1000000,
                WS_MAXIMIZEBOX = 0x10000,
                WS_MINIMIZE = 0x20000000,
                WS_MINIMIZEBOX = 0x20000,
                WS_OVERLAPPED = 0x0,
                WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
                WS_POPUP = 0x80000000u,
                WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
                WS_SIZEFRAME = 0x40000,
                WS_SYSMENU = 0x80000,
                WS_TABSTOP = 0x10000,
                WS_VISIBLE = 0x10000000,
                WS_VSCROLL = 0x200000
            }
            [Flags]
            public enum WindowStylesEx : uint
            {
                WS_EX_ACCEPTFILES = 0x00000010,
                WS_EX_APPWINDOW = 0x00040000,
                WS_EX_CLIENTEDGE = 0x00000200,
                WS_EX_COMPOSITED = 0x02000000,
                WS_EX_CONTEXTHELP = 0x00000400,
                WS_EX_CONTROLPARENT = 0x00010000,
                WS_EX_DLGMODALFRAME = 0x00000001,
                WS_EX_LAYERED = 0x00080000,
                WS_EX_LAYOUTRTL = 0x00400000,
                WS_EX_LEFT = 0x00000000,
                WS_EX_LEFTSCROLLBAR = 0x00004000,
                WS_EX_LTRREADING = 0x00000000,
                WS_EX_MDICHILD = 0x00000040,
                WS_EX_NOACTIVATE = 0x08000000,
                WS_EX_NOINHERITLAYOUT = 0x00100000,
                WS_EX_NOPARENTNOTIFY = 0x00000004,
                WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
                WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
                WS_EX_RIGHT = 0x00001000,
                WS_EX_RIGHTSCROLLBAR = 0x00000000,
                WS_EX_RTLREADING = 0x00002000,
                WS_EX_STATICEDGE = 0x00020000,
                WS_EX_TOOLWINDOW = 0x00000080,
                WS_EX_TOPMOST = 0x00000008,
                WS_EX_TRANSPARENT = 0x00000020,
                WS_EX_WINDOWEDGE = 0x00000100
            }
            public enum ShowWindowCommands : int
            {
                Hide = 0,
                Normal = 1,
                ShowMinimized = 2,
                Maximize = 3,
                ShowMaximized = 3,
                ShowNoActivate = 4,
                Show = 5,
                Minimize = 6,
                ShowMinNoActive = 7,
                ShowNA = 8,
                Restore = 9,
                ShowDefault = 10,
                ForceMinimize = 11
            }
            public static IntPtr WndProc(IntPtr hWnd, uint message, IntPtr wParam, IntPtr lParam)
            {
                winapi.Ellipse(hWnd, 0, 0, 111, 111);
                return DefWindowProc(hWnd, (IntPtr)message, wParam, lParam);
            }
        }
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
            static private int ArgbToRGB(int rgb)
            {
                return ((rgb >> 16 & 0x0000FF) | (rgb & 0x00FF00) | (rgb << 16 & 0xFF0000));
            }
            public void Do()
            {
                PAINTSTRUCT paintStruct = new PAINTSTRUCT();
                BeginPaint(this.Hwnd, ref paintStruct);

                IntPtr hdc = winapi.GetWindowDC(this.Hwnd);
                if(hdc != IntPtr.Zero) {
                    //Rectangle(hDC, 20, 20, 100, 100);
                    Pen pen = new Pen(Color.Red, 4);
                    IntPtr hPen = Gdi32.CreatePen(0, (int)pen.Width, ArgbToRGB(pen.Color.ToArgb()));
                    Gdi32.SelectObject(hdc, hPen);
                    //Gdi32.SetROP2(hdc, (int)Gdi32.DrawingMode.R2_NOTXORPEN);
                    //Gdi32.Rectangle(hdc, 0, 0, 200, 200);
                    Point sdf = new Point();
                    Gdi32.MoveToEx(hdc, 10, 10, ref sdf);
                    winapi.LineTo(hdc, this.Bounds.Width - 11, 10);
                    winapi.LineTo(hdc, this.Bounds.Width - 11, this.Bounds.Height - 10);
                    winapi.LineTo(hdc, 10, this.Bounds.Height - 10);
                    winapi.LineTo(hdc, 10, 10);



                    Gdi32.DeleteObject(hPen);
                    winapi.ReleaseDC(this.Hwnd, hdc);
                }
                
                

                Console.WriteLine(GetWindowDC(this.Hwnd));
            }

        }
        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr handle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public static string GetWindowClassName(IntPtr hWnd)
        {
            StringBuilder buffer = new StringBuilder(128);

            GetClassName(hWnd, buffer, buffer.Capacity);

            return buffer.ToString();
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
        public struct RECT { public int Left; public int Top; public int Right; public int Bottom; }
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

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////
        /// /////////////////////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public int fErase;
            public RECT rcPaint;
            public int fRestore;
            public int fIncUpdate;
            public int Reserved1;
            public int Reserved2;
            public int Reserved3;
            public int Reserved4;
            public int Reserved5;
            public int Reserved6;
            public int Reserved7;
            public int Reserved8;
        }

        [DllImport("User32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT paintStruct);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindow(IntPtr hwnd);
        //BeginPaint(HWND hwnd, PAINTSTRUCT FAR * lpps);
        /// /// /////////////////////////////////////////////////////////////////////////
        /// /// /////////////////////////////////////////////////////////////////////////
        /// /// /////////////////////////////////////////////////////////////////////////

        Timer timer = null;
        ФормаРамка рамка = null;

        public void Do() 
        {
            var frm = new Form() { StartPosition = FormStartPosition.CenterScreen, Width = 800, Height = 500, TopMost = true, Text = "TEST" };
            var rtb = new RichTextBox() { Parent = frm, Dock = DockStyle.Fill, WordWrap = false };
            rtb.Text = "asdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdf";
            timer = new Timer() { Interval = 5000, Enabled = true };
            //рамка = new ФормаРамка();
            //рамка.KeyDown += Рамка_KeyDown;
            //рамка.Visible = true; 
            //timer.Stop();
            timer.Tick += (s, eе) =>
            {
                var pos = Cursor.Position;
                var i = new Info(WindowFromPoint(pos));
                var p = GetWindows(i.Hwnd, Gw.GW_HWNDNEXT)
                        .Select(h => new Info(h))
                        .Where(v => v.Visible && Control.FromHandle(v.Hwnd) != rtb && v.Title != "F0H13A34" && !String.IsNullOrEmpty(v.Title))
                        .Where(v => !v.Bounds.IsEmpty && v.Bounds.IntersectsWith(i.Bounds)).FirstOrDefault();
                rtb.Text = String.Join("\n", p);
                
                if (p != null)
                {
                    p.Do();
                        //var tttt = GetWindows(WindowFromPoint(pos), Gw.GW_HWNDNEXT).Where(v => v == p.Hwnd).FirstOrDefault();





                        //var sdfsdf = GetWindow(tttt, Gw.GW_HWNDNEXT);

                    //Control ffff = Control.FromHandle(sdfsdf);
                    //Control mainForm = Form.FromHandle(p.);
                    //Console.WriteLine("mainForm = " + sdfsdf);
                    //Process[] processes = Process.GetProcesses();
                    //foreach (Process p in processes)
                    //{
                    //    if (p.StartInfo.FileName == "Name EXE")
                    //    {
                    //        IntPtr hWindow = p.MainWindowHandle;
                    //        Control mainForm = Form.FromHandle(hWindow);
                    //        ControlCollection childControls = mainForm.Controls;
                    //    }
                    //}



                    //timer.Stop();
                    //рамка.Correct(p.Bounds);
                    //timer.Start();
                }
                //    timer.Stop();
                //    // Получение дескриптора рабочего стола
                //    //IntPtr d = GetDC(IntPtr.Zero);
                //    if (рамка == null)
                //    {
                //        рамка = new ФормаРамка();

                //    }
                //    рамка.Visible = false;
                //    рамка.Left = p.Bounds.Left;
                //    рамка.Top = p.Bounds.Top;
                //    рамка.Width = p.Bounds.Width;
                //    рамка.Height = p.Bounds.Height;
                //    рамка.Visible = true;

                //    //// Create pen.
                //    //Pen blackPen = new Pen(Color.Aquamarine, 4);

                //    //Bitmap myBitmap = new Bitmap(p.Bounds.Width - 4, p.Bounds.Height - 4);
                //    //Graphics gr = Graphics.FromImage(myBitmap);
                //    //gr.DrawRectangle(blackPen, 0, 0, p.Bounds.Width - 4, p.Bounds.Height - 4);

                //    //// Создание объекта для работы с графикой через дескриптор
                //    //Graphics gfx = Graphics.FromHdc(d);

                //    //// Здесь используйте объект gfx для рисования всякого разного
                //    //gfx.SmoothingMode = SmoothingMode.HighSpeed;
                //    //gfx.Flush(new FlushIntention() { });
                //    ////gfx.DrawImage((Image)myBitmap, new Point(p.Bounds.X, p.Bounds.Y));
                //    //// Освобождение дескриптора
                //    //ReleaseDC(d);

                //    //Application.DoEvents();
                //    timer.Start();
                //}

            };
            frm.ShowDialog();
        }

        private void Рамка_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                if(рамка != null) {
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
    }
}
