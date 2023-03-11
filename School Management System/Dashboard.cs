using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            Dashboard_Load();
        }
        SqlConnection Con = new SqlConnection(MainSMS.connectionString);
        private void CountStudent()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM StudentsTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            StLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }

        private void CountTeachers()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM TeachersTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            TLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void CountEvents()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM EventsTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ELbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void Sumfees()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT SUM(Amount) FROM FeesTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            DollorLb.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }
        private void Dashboard_Load()
        {
            CountStudent();
            CountTeachers();
            CountEvents();
            Sumfees();
        }

        private void BackBtnLabel_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void StLbl_Click(object sender, EventArgs e)
        {

        }
    }
}
