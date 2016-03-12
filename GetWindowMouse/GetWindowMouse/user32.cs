using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilWinApi.User32
{
    /// <summary>
    /// Wrapper class for the user32.dll.
    /// </summary>
    public class user32
    {
        #region Перечисления
        public enum Gw : uint
        {
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        #region Методы
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, Gw uCmd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point p);
        #endregion
    }

    /// <summary>
    /// Provides utilities directly accessing the user32.dll 
    /// </summary>
    public static class WinUser
	{
        public static Rectangle GetRectangle(IntPtr hwnd)
        {
            var r = new user32.RECT();
            user32.GetWindowRect(hwnd, out r);
            return Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
        }

        public static string GetTitle(IntPtr hwnd)
        {
            var sb = new StringBuilder(user32.GetWindowTextLength(hwnd) * 2);
            user32.GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public static IEnumerable<IntPtr> GetWindows(IntPtr hwnd, user32.Gw gw)
        {
            for (var h = hwnd; h != IntPtr.Zero; h = user32.GetWindow(h, gw))
                yield return h;
        }
    }

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
                return WinUser.GetRectangle(this.Hwnd);
            }
        }
        public string Title
        {
            get
            {
                return WinUser.GetTitle(this.Hwnd);
            }
        }
        public bool Visible
        {
            get
            {
                return user32.IsWindowVisible(this.Hwnd);
            }
        }
        public override string ToString()
        {
            return String.Format("{0,15}\t{1,-50}\t{2}", this.Hwnd, this.Title, this.Bounds);
        }
    }
}
