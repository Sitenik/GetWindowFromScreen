using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace WinApiCom
{
    /// <summary>
    /// Wrapper class for the user32.dll.
    /// </summary>
    public class User32
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

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        #endregion
    }

    /// <summary>
    /// Provides utilities directly accessing the user32.dll 
    /// </summary>
    public static class User32Helper
    {
        public static Rectangle GetRectangle(IntPtr hwnd)
        {
            var r = new User32.RECT();
            User32.GetWindowRect(hwnd, out r);
            return Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Bottom);
        }

        public static string GetTitle(IntPtr hwnd)
        {
            var sb = new StringBuilder(User32.GetWindowTextLength(hwnd) * 2);
            User32.GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public static IEnumerable<IntPtr> GetWindows(IntPtr hwnd, User32.Gw gw)
        {
            for (var h = hwnd; h != IntPtr.Zero; h = User32.GetWindow(h, gw))
                yield return h;
        }

        public static User32.RECT GetClientRect(IntPtr hWnd)
        {
            User32.RECT result;
            User32.GetClientRect(hWnd, out result);
            return result;
        }
    }
}
