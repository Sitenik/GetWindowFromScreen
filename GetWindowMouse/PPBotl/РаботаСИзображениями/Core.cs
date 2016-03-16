using System;
using GetMainWin;
using WinApiCom;

namespace PPBotl.РаботаСИзображениями
{
    public class Core : IDisposable
    {
        protected InfoHwnd _ОкноПриложения;
        public InfoHwnd ОкноПриложения 
        {
            get 
            {
                return _ОкноПриложения;
            }
        }

        public Core() 
        {
            /// Конструктор
        }

        private bool Init() 
        {
            try
            {
                /// Инициализация
            }
            catch (Exception exc)
            {
                Console.WriteLine(String.Format("{0}:{1}", this.GetType().FullName, exc.Message));
                return false;
            }
            return true;
        }


        public void ВыбратьОкноПриложения() 
        {
            using (ОбработкаВыбораОкна ОкноПриложенияHelper = new ОбработкаВыбораОкна()) 
            {
                _ОкноПриложения = ОкноПриложенияHelper.Do();
            }
        }

        public void Dispose()
        {
        }
    }
}
