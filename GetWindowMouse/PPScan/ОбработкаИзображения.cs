using System;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PPScan
{
    public class ОбработкаИзображения
    {
        public Bitmap Результат = null;
        Mat шаблон;

        public ОбработкаИзображения(Bitmap Скриншот)
        {
            if (Скриншот == null)
            {
                throw new Exception("Входной параметр _исходник не задан");
            }

            Mat исходник = BitmapConverter.ToMat(Скриншот);

            string ФайлШаблон = "tamplate.jpg";

            TemplateMatchModes МетодСравнения = TemplateMatchModes.CCoeffNormed;

            шаблон = new Mat(ФайлШаблон, ImreadModes.AnyColor);
            if (шаблон == null)
            {
                throw new Exception("Шаблон не найден");
            }

            Mat СерыйИсходник = new Mat();
            Mat СерыйШаблон = new Mat();

            СерыйИсходник = исходник.CvtColor(ColorConversionCodes.BGR2GRAY);
            СерыйШаблон = шаблон.CvtColor(ColorConversionCodes.BGR2GRAY);

            Mat РабочееИзображение = new Mat(new OpenCvSharp.Size(исходник.Rows - шаблон.Rows + 1, исходник.Cols - шаблон.Cols + 1), MatType.CV_32F, 1);
            РабочееИзображение = СерыйИсходник.MatchTemplate(СерыйШаблон, МетодСравнения);
            РабочееИзображение.Threshold(0.93, 1.0, ThresholdTypes.Tozero);

            for (int i = 0; i < 100; i++)
            {
                double minval = 0;
                double maxval = 0;
                double threshold = 0.93;
                OpenCvSharp.Point minloc = new OpenCvSharp.Point();
                OpenCvSharp.Point maxloc = new OpenCvSharp.Point();
                РабочееИзображение.MinMaxLoc(out minval, out maxval, out minloc, out maxloc);
                if (maxval >= threshold)
                {
                    исходник.Rectangle(new Rect(maxloc, new OpenCvSharp.Size(шаблон.Width, шаблон.Height)), new Scalar(0, 255, 0), 2);
                    Rect outRect = new Rect();
                    РабочееИзображение.FloodFill(maxloc, Scalar.Black, out outRect, Scalar.White);
                    Console.WriteLine(i);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
