using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace School_Management_System
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            Splash_Load();
        }
        int Startpoint = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Startpoint += 10;
            ProgressBar1.Value = Startpoint;
            Percentage.Text = Startpoint + "%";
            if (ProgressBar1.Value == 100)
            {
                ProgressBar1.Value = 0;
                timer1.Stop();
                Login Obj = new Login();
                this.Hide();
                Obj.Show();
            }
        }

        private void Splash_Load()
        {
            timer1.Start();
        }
    }
}
