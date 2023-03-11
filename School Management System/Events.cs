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
    public partial class Events : Form
    {
        public Events()
        {
            InitializeComponent();
            DisplayEvents();
        }

        private void DisplayEvents()
        {
            Con.Open();
            string Query = "SELECT * FROM EventsTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            EventsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Reset()
        {
            EDescTb.Text = "";
            EDurationTb.Text = "";
        }
        SqlConnection Con = new SqlConnection(MainSMS.connectionString);
        private void addBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EDurationTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO EventsTbl(EduDesc,EduDate,EduDuration) VALUES (@EventsDescription,@EventsDate,@EventsDuration)", Con);
                    cmd.Parameters.AddWithValue("@EventsDescription", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EventsDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EventsDuration", EDurationTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Events Added Successfully");
                    Con.Close();
                    DisplayEvents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please Select Events");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM EventsTbl WHERE EduId=@EKey", Con);
                    cmd.Parameters.AddWithValue("@EKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event has been deleted successfully");
                    Con.Close();
                    DisplayEvents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        int Key = 0;

        private void EventsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            EDescTb.Text = EventsDGV.SelectedRows[0].Cells[1].Value.ToString();
            EDate.Text = EventsDGV.SelectedRows[0].Cells[2].Value.ToString();
            EDurationTb.Text = EventsDGV.SelectedRows[0].Cells[3].Value.ToString();

            if (EDescTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(EventsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (EDescTb.Text == "" || EDurationTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE EventsTbl SET EduDesc=@EDesc,EduDate=@EDate,EduDuration=@EDuration WHERE EduId=@EID", Con);
                    cmd.Parameters.AddWithValue("@EDesc", EDescTb.Text);
                    cmd.Parameters.AddWithValue("@EDate", EDate.Value.Date);
                    cmd.Parameters.AddWithValue("@EDuration", EDurationTb.Text);
                    cmd.Parameters.AddWithValue("@EID", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Event Updated Successfully");
                    Con.Close();
                    DisplayEvents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
    }
}
