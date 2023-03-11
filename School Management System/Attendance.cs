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
    public partial class Attendance : Form
    {
        public Attendance()
        {
            InitializeComponent();
            DisplayAttendance();
            FillStuId();
        }

        private void FillStuId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT StuId from StudentsTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("StuId", typeof(int));
            dt.Load(rdr);
            StIdCb.ValueMember = "StuId";
            StIdCb.DataSource = dt;
            Con.Close();
        }
        private void GetStudName()
        {
            Con.Open();
            SqlCommand Cmd = new SqlCommand("SELECT * FROM StudentsTbl WHERE StuId=@SID", Con);
            Cmd.Parameters.AddWithValue("@SID", StIdCb.SelectedValue.ToString());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(Cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                StNameTb.Text = dr["StuName"].ToString();
            }
            Con.Close();
        }

        SqlConnection Con = new SqlConnection(MainSMS.connectionString);
        private void DisplayAttendance()
        {
            Con.Open();
            string Query = "SELECT * from AttendanceTble";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AttendanceDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Reset()
        {
            AttStatus.SelectedIndex = -1;
            StNameTb.Text = "";
            StIdCb.SelectedIndex = -1;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || AttStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO AttendanceTble(AttStId,AttStName,AttDate,AttStatus) values (@StId,@StName,@AttDate,@AttStatus)", Con);
                    cmd.Parameters.AddWithValue("@StId", StIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@AttDate", AttDate.Value.Date);
                    cmd.Parameters.AddWithValue("@AttStatus", AttStatus.SelectedItem.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance has been taken successfully");
                    Con.Close();
                    DisplayAttendance();
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

        private void StIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStudName();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please Select Attendance");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM AttendanceTble WHERE AttNum=@AttNum", Con);
                    cmd.Parameters.AddWithValue("@AttNum", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance has been deleted successfully");
                    Con.Close();
                    DisplayAttendance();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        int Key = 0;
        private void AttendanceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StIdCb.SelectedValue = AttendanceDGV.SelectedRows[0].Cells[1].Value.ToString();
            StNameTb.Text = AttendanceDGV.SelectedRows[0].Cells[2].Value.ToString();
            AttDate.Text = AttendanceDGV.SelectedRows[0].Cells[3].Value.ToString();
            AttStatus.SelectedItem = AttendanceDGV.SelectedRows[0].Cells[4].Value.ToString();

            if (StNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(AttendanceDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || AttStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE AttendanceTble SET AttStId=@StId,AttStName=@StName,AttDate=@AttDate,AttStatus=@AttStatus WHERE AttNum=@AttNum", Con);
                    cmd.Parameters.AddWithValue("@StId", StIdCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@AttDate", AttDate.Value.Date);
                    cmd.Parameters.AddWithValue("@AttStatus", AttStatus.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@AttNum", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Attendance Updated Successfully");
                    Con.Close();
                    DisplayAttendance();
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
