using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GetWindowMouse;
using WinApiCom;

namespace PPHelp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public InfoHwnd ОкноПриложения = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (Обработка выбратьОкно = new Обработка()) {
                ОкноПриложения = выбратьОкно.Do();
                if(ОкноПриложения == null) {
                    lb.Content = "Выберите окно";
                    return;
                }
                else {
                    lb.Content = ОкноПриложения;
                }
            }

            screen_shot_game(ОкноПриложения);

        }

        bool screen_shot_game(InfoHwnd window_f)
        {
            IntPtr hdc = window_f.GetDC();
            if(hdc == IntPtr.Zero) {
                return false;
            }
            IntPtr hdcCompatible = Gdi32.CreateCompatibleDC(hdc);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHdc(hdcCompatible);
            Bitmap bmp = new Bitmap(window_f.Bounds.Width, window_f.Bounds.Height, g);
            Graphics memoryGraphics = Graphics.FromImage(bmp);
            IntPtr dc = memoryGraphics.GetHdc();
            bool success = User32.PrintWindow(window_f.Hwnd, dc, 0);

            BitmapImage bitmapSource = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapSource));
                enc.Save(outStream);

                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = bitmapSource;
                this.Background = myBrush;
            }

            memoryGraphics.ReleaseHdc(dc);

            //RECT cr;
            //GetClientRect(window_f, &cr);
            //HBITMAP hBmp = CreateCompatibleBitmap(hdc, cr.right - cr.left, cr.bottom - cr.top);
            //HWND hSrcWnd;
            //HDC hSrcDC;
            //HBITMAP old_bitmap = (HBITMAP)SelectObject(hdcCompatible, hBmp);
            //BitBlt(hdcCompatible, 0, 0, cr.right - cr.left, cr.bottom - cr.top, hdc, 0, 0, SRCCOPY);
            //ReleaseDC(window_f, hdc);
            //SelectObject(hdcCompatible, old_bitmap);
            //DeleteDC(hdcCompatible);
            //DeleteObject(old_bitmap);
            //StoreBitmapFile("3.bmp", hBmp);
            //DeleteObject(hBmp);
            return true;
        }
    }
}
