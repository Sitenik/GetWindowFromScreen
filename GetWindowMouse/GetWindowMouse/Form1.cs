using System;
using System.Windows.Forms;

namespace GetWindowMouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Form2 новаяФорма = null;

        private void button1_Click(object sender, EventArgs e)
        {
            новаяФорма = new Form2();
            новаяФорма.KeyDown += НоваяФорма_KeyDown;
            новаяФорма.MouseClick += НоваяФорма_MouseClick;
            новаяФорма.Show();
        }

        private void НоваяФорма_MouseClick(object sender, MouseEventArgs e)
        {
            (sender as Form).MouseClick -= НоваяФорма_MouseClick;
            (sender as Form).Dispose();
            sender = null;
       }

        private void НоваяФорма_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27 && sender != null)
            {
                (sender as Form).KeyDown -= НоваяФорма_KeyDown;
                (sender as Form).Dispose();
                sender = null;
            }
        }
    }
}
