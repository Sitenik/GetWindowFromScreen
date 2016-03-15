using System;
using System.Collections.Generic;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PPBotl
{
    public class ОбработкаИзображения : IDisposable
    {
        Mat шаблон;
        public List<Rect> Координаты = new List<Rect>();

        public ОбработкаИзображения()
        {
            string ФайлШаблон = "tamplate.jpg";
            шаблон = new Mat(ФайлШаблон, ImreadModes.AnyColor);
            if (шаблон == null)
            {
                throw new Exception("Шаблон не найден");
            }
        }

        public void SetImg(ref Bitmap Скриншот)
        {
            if (Скриншот == null)
            {
                throw new Exception("Входной параметр _исходник не задан");
            }
            Координаты.Clear();
            TemplateMatchModes МетодСравнения = TemplateMatchModes.CCoeffNormed;
            using (Mat исходник = BitmapConverter.ToMat(Скриншот))
            {
                using (Mat СерыйИсходник = исходник.CvtColor(ColorConversionCodes.BGR2GRAY))
                {
                    using (Mat СерыйШаблон = шаблон.CvtColor(ColorConversionCodes.BGR2GRAY))
                    {
                        using (Mat РабочееИзображение = new Mat(new OpenCvSharp.Size(исходник.Rows - шаблон.Rows + 1, исходник.Cols - шаблон.Cols + 1), MatType.CV_32F, 1))
                        {
                            using (Mat РабочееИзображениеTmp = СерыйИсходник.MatchTemplate(СерыйШаблон, МетодСравнения))
                            {
                                using (Mat РабочееИзображениеTmpTmp = РабочееИзображениеTmp.Threshold(0.93, 1.0, ThresholdTypes.Tozero))
                                {
                                    while (true)
                                    {
                                        double minval = 0;
                                        double maxval = 0;
                                        double threshold = 0.93;
                                        OpenCvSharp.Point minloc = new OpenCvSharp.Point();
                                        OpenCvSharp.Point maxloc = new OpenCvSharp.Point();
                                        РабочееИзображениеTmpTmp.MinMaxLoc(out minval, out maxval, out minloc, out maxloc);
                                        if (maxval >= threshold)
                                        {
                                            Rect объект = new Rect(maxloc, new OpenCvSharp.Size(шаблон.Width, шаблон.Height));
                                            исходник.Rectangle(объект, new Scalar(0, 255, 0), 2);
                                            Rect outRect = new Rect();
                                            Координаты.Add(объект);
                                            РабочееИзображениеTmpTmp.FloodFill(maxloc, Scalar.Black, out outRect, Scalar.White);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Скриншот.Dispose();
                Скриншот = исходник.ToBitmap();
            }

            //if (исходник != null)
            //{
            //    Скриншот = исходник.ToBitmap();
            //}
            //РабочееИзображениеTmpTmp.Dispose();
            //РабочееИзображениеTmp.Dispose();
            //РабочееИзображение.Dispose();
            //СерыйИсходник.Dispose();
            //СерыйШаблон.Dispose();
            //исходник.Dispose();
        }

        public void Dispose()
        {
            if(шаблон != null) 
            {
                шаблон.Dispose();
            }
        }
    }
}
