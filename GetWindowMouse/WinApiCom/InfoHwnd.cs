using System;
using System.Drawing;

namespace WinApiCom
{
    public class InfoHwnd
    {
        public IntPtr Hwnd;

        public InfoHwnd(IntPtr hwnd)
        {
            this.Hwnd = hwnd;
        }
        public Rectangle Bounds
        {
            get
            {
                return User32Helper.GetRectangle(this.Hwnd);
            }
        }
        public string Title
        {
            get
            {
                return User32Helper.GetTitle(this.Hwnd);
            }
        }
        public bool Visible
        {
            get
            {
                return User32.IsWindowVisible(this.Hwnd);
            }
        }
        public override string ToString()
        {
            return String.Format("{0,15}\t{1,-50}\t{2}", this.Hwnd, this.Title, this.Bounds);
        }

        public IntPtr GetDC()
        {
            return User32.GetWindowDC(this.Hwnd);
        }
    }
}
