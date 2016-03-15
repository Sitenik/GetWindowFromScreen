using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WinApiCom
{
	/// <summary>
	/// Wrapper class for the gdi32.dll.
	/// </summary>
	public class Gdi32
	{
        public enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062,
            CAPTUREBLT = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
        }

        public enum DrawingMode
		{
			R2_NOTXORPEN = 10
		}

		[DllImport("gdi32.dll")]
		public static extern bool Rectangle(IntPtr hDC, int left, int top, int right, int bottom);

		[DllImport("gdi32.dll")]
		public static extern int SetROP2(IntPtr hDC, int fnDrawMode);

		[DllImport("gdi32.dll")]
		public static extern bool MoveToEx(IntPtr hDC, int x, int y, ref Point p);

		[DllImport("gdi32.dll")]
		public static extern bool LineTo(IntPtr hdc, int x, int y);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObj);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);
    }

	/// <summary>
	/// Provides utilities directly accessing the gdi32.dll 
	/// </summary>
	public class Gdi32Helper
    {
		static private Point nullPoint = new Point(0,0);

		// Convert the Argb from .NET to a gdi32 RGB
		static private int ArgbToRGB(int rgb)
		{
			return ((rgb >> 16 & 0x0000FF)| (rgb & 0x00FF00) | (rgb << 16 & 0xFF0000));
		}
		static public void DrawXORRectangle(Graphics graphics, Pen pen, Rectangle rectangle)
		{
			IntPtr hDC = graphics.GetHdc();
			IntPtr hPen = Gdi32.CreatePen(0, (int)pen.Width, ArgbToRGB(pen.Color.ToArgb()));
			Gdi32.SelectObject(hDC, hPen);
			Gdi32.SetROP2(hDC, (int)Gdi32.DrawingMode.R2_NOTXORPEN);
			Gdi32.Rectangle(hDC, rectangle.Left, rectangle.Top, rectangle.Right,rectangle.Bottom);
			Gdi32.DeleteObject(hPen);
			graphics.ReleaseHdc(hDC);
		}

		static public void DrawXORLine(Graphics graphics, Pen pen, int x1, int y1, int x2, int y2)
		{
			IntPtr hDC = graphics.GetHdc();
			IntPtr hPen = Gdi32.CreatePen(0, (int)pen.Width, ArgbToRGB(pen.Color.ToArgb()));
			Gdi32.SelectObject(hDC, hPen);
			Gdi32.SetROP2(hDC, (int)Gdi32.DrawingMode.R2_NOTXORPEN);
			Gdi32.MoveToEx(hDC, x1, y1, ref nullPoint);
			Gdi32.LineTo(hDC, x2, y2);
			Gdi32.DeleteObject(hPen);
			graphics.ReleaseHdc(hDC);
		}
	}
}
