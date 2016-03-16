using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinApiCom;

namespace PPBotl.РаботаСИзображениями
{
    enum StatusWork 
    {
        BMPNotReady = 0,
        BMPReady = 1,
        BMPInWork = 2,
        BMPFailure = 3
    }
    public class Скриншотер
    {
        private Core Ядро;
        private String TestTimeString = "";
        private StatusWork GetStatus = StatusWork.BMPNotReady;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        public Bitmap asdfsdf = null;

        public Скриншотер(Core ядро) 
        {
            Ядро = ядро;
        }

        public void GetScreenShot(ref Bitmap Скриншот)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                IntPtr hdc = Ядро.ОкноПриложения.GetDC();
                if (hdc == IntPtr.Zero)
                {
                    throw new Exception("Не удалось получить дескриптор изображений для выбранного окна");
                }
                IntPtr hdcCompatible = Gdi32.CreateCompatibleDC(hdc);
                if (hdcCompatible == IntPtr.Zero)
                {
                    throw new Exception("Не удалось получить совместимый дескриптор изображений для выбранного окна");
                }
                using (Graphics g = Graphics.FromHdc(hdcCompatible))
                {
                    if (Скриншот != null)
                    {
                        Скриншот.Dispose();
                    }
                    Скриншот = new Bitmap(Ядро.ОкноПриложения.Bounds.Width, Ядро.ОкноПриложения.Bounds.Height, g);
                    using (Graphics memoryGraphics = Graphics.FromImage(Скриншот))
                    {
                        IntPtr dc = memoryGraphics.GetHdc();
                        if (dc == IntPtr.Zero)
                        {
                            throw new Exception("Не удалось получить дескриптор изображений для скрина");
                        }
                        if (!User32.PrintWindow(Ядро.ОкноПриложения.Hwnd, dc, 0))
                        {
                            throw new Exception("Не удалось получить скрин из выбранного окна");
                        }
                        memoryGraphics.ReleaseHdc(dc);
                    }
                }

            }
            finally 
            {
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                TestTimeString = String.Format("{0}", ts.Milliseconds);
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










            //if (screen_shot_game(Bitmap Скриншот))
            //{
            //    //pictureBox1.Image = Скриншот;
            //    qwe.SetImg(ref Скриншот);
            //    pictureBox1.Image = Скриншот;

            //    System.Drawing.Point позиция = new System.Drawing.Point();
            //    позиция.X = Скриншот.Width / 2 - pictureBox2.Size.Width / 2;
            //    позиция.Y = Скриншот.Height / 2 - pictureBox2.Size.Height / 2 + 100;
            //    if (pictureBox2.Image != null)
            //    {
            //        pictureBox2.Image.Dispose();
            //    }
            //}

            
        }

        public void GetScreenShotAsync(ref Bitmap Скриншот)
        {
            try
            {
                this.GetStatus = StatusWork.BMPInWork;
                if(Скриншот != null) 
                {
                    Скриншот.Dispose();
                }
                InitializeBackgroundWorker();
                asdfsdf = Скриншот;
                backgroundWorker.RunWorkerAsync(Скриншот);
                Скриншот = asdfsdf;
                this.GetStatus = StatusWork.BMPReady;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                this.GetStatus = StatusWork.BMPFailure;
            }
        }

        private Bitmap BuildBitmap(Bitmap Скриншот)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                if (Скриншот != null)
                {
                    Скриншот.Dispose();
                }
                IntPtr hdc = Ядро.ОкноПриложения.GetDC();
                if (hdc == IntPtr.Zero)
                {
                    throw new Exception("Не удалось получить дескриптор изображений для выбранного окна");
                }
                IntPtr hdcCompatible = Gdi32.CreateCompatibleDC(hdc);
                if (hdcCompatible == IntPtr.Zero)
                {
                    throw new Exception("Не удалось получить совместимый дескриптор изображений для выбранного окна");
                }
                using (Graphics g = Graphics.FromHdc(hdcCompatible))
                {
                    Скриншот = new Bitmap(Ядро.ОкноПриложения.Bounds.Width, Ядро.ОкноПриложения.Bounds.Height, g);
                    using (Graphics memoryGraphics = Graphics.FromImage(Скриншот))
                    {
                        IntPtr dc = memoryGraphics.GetHdc();
                        if (dc == IntPtr.Zero)
                        {
                            throw new Exception("Не удалось получить дескриптор изображений для скрина");
                        }
                        if (!User32.PrintWindow(Ядро.ОкноПриложения.Hwnd, dc, 0))
                        {
                            throw new Exception("Не удалось получить скрин из выбранного окна");
                        }
                        memoryGraphics.ReleaseHdc(dc);
                    }
                }
            }
            finally
            {
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            TestTimeString = String.Format("{0}", ts.Milliseconds);
            return Скриншот;
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new System.ComponentModel.BackgroundWorker();

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                // The operation completed normally.
                string msg = String.Format("Result = {0}", e.Result);
                MessageBox.Show(msg);
            }
            asdfsdf = e.Result as Bitmap;
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;

            // Extract the argument.
            Bitmap arg = (Bitmap)e.Argument;

            // Start the time-consuming operation.
            e.Result = BuildBitmap(arg);

            // If the operation was canceled by the user, 
            // set the DoWorkEventArgs.Cancel property to true.
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
        }
    }
}
