using System.Drawing;
using System.Windows.Forms;

namespace GetWindowMouse
{
    public partial class ФормаРамка : Form
    {
        const byte РазмерРамки = 4;
        Rectangle размерРабочегоСтола = new Rectangle();

        public ФормаРамка()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.AllowTransparency = true;
            this.BackColor = Color.AliceBlue;//цвет фона  
            this.TransparencyKey = this.BackColor;//он же будет заменен на прозрачный цвет
            this.TopMost = true;
            this.StartPosition = FormStartPosition.Manual; ;
            размерРабочегоСтола = Screen.PrimaryScreen.WorkingArea;
        }

        internal void Correct(Rectangle bounds)
        {
            this.Visible = true;
            this.Top = bounds.Top < размерРабочегоСтола.Top ? размерРабочегоСтола.Top : bounds.Top;
            this.Left = bounds.Left < размерРабочегоСтола.Left ? размерРабочегоСтола.Left : bounds.Left;
            this.Width = bounds.Width > размерРабочегоСтола.Width ? размерРабочегоСтола.Width : bounds.Width;
            this.Height = bounds.Height > размерРабочегоСтола.Height ? размерРабочегоСтола.Height : bounds.Height;
        }
    }
}
