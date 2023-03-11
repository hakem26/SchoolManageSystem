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
    public partial class Teachers : Form
    {
        public Teachers()
        {
            InitializeComponent();
            DisplayTeachers();
        }
        private void Reset()
        {
            TNameTb.Text = "";
            TGender.SelectedIndex = 0;
            SubjectCb.SelectedIndex = 0;
            TMobileTb.Text = "";
            TAddressTb.Text = "";
        }

        SqlConnection Con = new SqlConnection(MainSMS.connectionString);
        private void DisplayTeachers()
        {
            Con.Open();
            string Query = "Select * from TeachersTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TeachersDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void addBtn_Click(object sender, EventArgs e)
        {
            if (TNameTb.Text == "" || TMobileTb.Text == "" || TAddressTb.Text == "" || TGender.SelectedIndex == -1 || SubjectCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO TeachersTbl(TchrName,TchrGender,TchrPhone,TchrSubject,TchrAddress,TchrDOB) values (@Tname,@TGender,@TPhone,@TSubject,@TAddress,@TDOB)", Con);
                    cmd.Parameters.AddWithValue("@Tname", TNameTb.Text);
                    cmd.Parameters.AddWithValue("@TGender", TGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", TMobileTb.Text);
                    cmd.Parameters.AddWithValue("@TSubject", SubjectCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAddress", TAddressTb.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Added Successfully");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        int Key = 0;

        private void TeachersDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TNameTb.Text = TeachersDGV.SelectedRows[0].Cells[1].Value.ToString();
            TGender.SelectedItem = TeachersDGV.SelectedRows[0].Cells[2].Value.ToString();
            TDOB.Text = TeachersDGV.SelectedRows[0].Cells[3].Value.ToString();
            SubjectCb.SelectedItem = TeachersDGV.SelectedRows[0].Cells[4].Value.ToString();
            TMobileTb.Text = TeachersDGV.SelectedRows[0].Cells[5].Value.ToString();
            TAddressTb.Text = TeachersDGV.SelectedRows[0].Cells[6].Value.ToString();
            if (TNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TeachersDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please Select Teacher");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM TeachersTbl WHERE TchrId=@TKey", Con);
                    cmd.Parameters.AddWithValue("@TKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher has been deleted successfully");
                    Con.Close();
                    DisplayTeachers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (TNameTb.Text == "" || TMobileTb.Text == "" || TAddressTb.Text == "" || TGender.SelectedIndex == -1 || SubjectCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE TeachersTbl SET TchrName=@Tname,TchrGender=@TGender,TchrPhone=@TPhone,TchrSubject=@TSubject,TchrAddress=@TAddress,TchrDOB=@TDOB WHERE TchrId=@TeachID", Con);
                    cmd.Parameters.AddWithValue("@Tname", TNameTb.Text);
                    cmd.Parameters.AddWithValue("@TGender", TGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TPhone", TMobileTb.Text);
                    cmd.Parameters.AddWithValue("@TSubject", SubjectCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@TAddress", TAddressTb.Text);
                    cmd.Parameters.AddWithValue("@TDOB", TDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@TeachID", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Teacher Updated Successfully");
                    Con.Close();
                    DisplayTeachers();
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
    }
}
