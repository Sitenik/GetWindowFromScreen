using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GetWindowMouse;
using OpenCvSharp;
using WinApiCom;

namespace PPBotl
{
    public partial class Form1 : Form
    {
        public InfoHwnd ОкноПриложения = null;
        public Bitmap Скриншот = null;
        Timer timer = null;


        class Объекты
        {
            const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
            const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
            const uint MOUSEEVENTF_LEFTUP = 0x0004;
            const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
            const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
            const uint MOUSEEVENTF_MOVE = 0x0001;
            const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
            const uint MOUSEEVENTF_RIGHTUP = 0x0010;
            const uint MOUSEEVENTF_XDOWN = 0x0080;
            const uint MOUSEEVENTF_XUP = 0x0100;
            const uint MOUSEEVENTF_WHEEL = 0x0800;
            const uint MOUSEEVENTF_HWHEEL = 0x01000;

            [Flags]
            public enum MouseEventFlags
            {
                LEFTDOWN = 0x00000002,
                LEFTUP = 0x00000004,
                MIDDLEDOWN = 0x00000020,
                MIDDLEUP = 0x00000040,
                MOVE = 0x00000001,
                ABSOLUTE = 0x00008000,
                RIGHTDOWN = 0x00000008,
                RIGHTUP = 0x00000010
            }

            [DllImport("user32.dll")]
            static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

            public Объекты(Timer timer, RichTextBox _rtb, Bitmap Скриншот) 
            {
                Таймер = timer;
                rtb = _rtb;
            }

            List<Rect> Список = new List<Rect>();
            Timer Таймер;
            RichTextBox rtb;

            public void Добавить(List<Rect> СписокОбъекты)
            {
                Список.Clear();
                foreach (var item in СписокОбъекты)
                {
                    ДобавитьОбъект(item);
                }
            }

            void ДобавитьОбъект(Rect item) 
            {
                Список.Add(item);
                //Таймер.Stop();
                rtb.Text = item.ToString();

                //Cursor.Position = new System.Drawing.Point(item.Location.X + 10, item.Location.Y + 10);
                //mouse_event((int)(MouseEventFlags.LEFTDOWN), (uint)item.X + 10, (uint)item.Y + 12, 0, UIntPtr.Zero);
                //mouse_event((int)(MouseEventFlags.LEFTUP), (uint)item.X + 10, (uint)item.Y + 12, 0, UIntPtr.Zero);

                //System.Threading.Thread.Sleep(10000);// Sleep();
                //Таймер.Start();
                //mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)item.X + 10, (uint)item.Y + 12, 0, UIntPtr.Zero);

            }
        }

        Объекты Найденные = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Обработка выбратьОкно = new Обработка())
            {
                ОкноПриложения = выбратьОкно.Do();
                if (ОкноПриложения == null)
                {
                    lb.Text = "Выберите окно";
                    return;
                }
                else 
                {
                    lb.Text = ОкноПриложения.ToString();
                }
            }
            timer = new Timer() { Interval = 80, Enabled = true };
            timer.Tick += Timer_Tick;
            Найденные = new Form1.Объекты(timer, rtb, Скриншот);
        }

        ОбработкаИзображения qwe = new ОбработкаИзображения();

        private void Timer_Tick(object sender, EventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (screen_shot_game(ОкноПриложения))
            {
                //pictureBox1.Image = Скриншот;
                qwe.SetImg(ref Скриншот);
                pictureBox1.Image = Скриншот;

                System.Drawing.Point позиция = new System.Drawing.Point();
                позиция.X = Скриншот.Width / 2 - pictureBox2.Size.Width / 2;
                позиция.Y = Скриншот.Height / 2 - pictureBox2.Size.Height / 2 + 100;
                if(pictureBox2.Image != null) 
                {
                    pictureBox2.Image.Dispose();
                }
                pictureBox2.Image = Скриншот.Clone(new Rectangle(позиция, pictureBox2.Size), System.Drawing.Imaging.PixelFormat.Undefined);
                Найденные.Добавить(qwe.Координаты);
                //String координаты = "";
                //int i = 0;
                //foreach (var item in qwe.Координаты)
                //{
                //    координаты += "Объект " + i + ": X = " + item.X + "; Y = " + item.Y + Environment.NewLine;
                //}
                //rtb.Text = координаты;
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            //string elapsedTime = String.Format("{0}",ts.Milliseconds);
            //rtb.AppendText(elapsedTime + Environment.NewLine);
            //rtb.Text = elapsedTime;
        }

        bool screen_shot_game(InfoHwnd window_f)
        {
            IntPtr hdc = window_f.GetDC();
            if (hdc == IntPtr.Zero)
            {
                return false;
            }
            IntPtr hdcCompatible = Gdi32.CreateCompatibleDC(hdc);
            if(hdcCompatible == IntPtr.Zero) 
            {
                return false;
            }
            using (Graphics g = Graphics.FromHdc(hdcCompatible)) 
            {
                if(Скриншот != null) 
                {
                    Скриншот.Dispose();
                }
                Скриншот = new Bitmap(window_f.Bounds.Width, window_f.Bounds.Height, g);
                using (Graphics memoryGraphics = Graphics.FromImage(Скриншот))
                {
                    IntPtr dc = memoryGraphics.GetHdc();
                    if (dc == IntPtr.Zero)
                    {
                        return false;
                    }
                    if (!User32.PrintWindow(window_f.Hwnd, dc, 0))
                    {
                        return false;
                    }
                    memoryGraphics.ReleaseHdc(dc);
                }
            }
            //Bitmap image;
            //User32.RECT cr = User32Helper.GetClientRect(window_f.Hwnd);
            //IntPtr hBmp = Gdi32.CreateCompatibleBitmap(hdc, cr.Right - cr.Left, cr.Bottom - cr.Top);
            //IntPtr old_bitmap = (IntPtr)Gdi32.SelectObject(hdcCompatible, hBmp);
            //Gdi32.BitBlt(hdcCompatible, 0, 0, cr.Right - cr.Left, cr.Bottom - cr.Top, hdc, 0, 0, Gdi32.TernaryRasterOperations.SRCCOPY);
            //User32.ReleaseDC(window_f.Hwnd, hdc);
            //Gdi32.SelectObject(hdcCompatible, old_bitmap);
            //Gdi32.DeleteDC(hdcCompatible);
            //Gdi32.DeleteObject(old_bitmap);
            //image = Image.FromHbitmap(hBmp);
            //this.BackgroundImage = image;
            //Gdi32.DeleteObject(hBmp);
            
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            screen_shot_game(ОкноПриложения);
        }
    }
}
