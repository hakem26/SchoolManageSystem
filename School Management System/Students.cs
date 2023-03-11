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
    public partial class Students : Form
    {
        public Students()
        {
            InitializeComponent();
            DisplayStudent();
        }
        SqlConnection Con = new SqlConnection(MainSMS.connectionString);
        private void DisplayStudent()
        {
            Con.Open();
            string Query = "SELECT * FROM StudentsTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds =new DataSet();
            sda.Fill(ds);
            StudentsDGV.DataSource= ds.Tables[0];
            Con.Close();
        }
        private void addBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenderCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into StudentsTbl(StuName,StuGender,StuDOB,StuClass,StuFees,StuAddress) values (@Sname,@SGender,@SDOB,@SClass,@SFees,@SAddress)", Con);
                    cmd.Parameters.AddWithValue("@Sname", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGender", StGenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDOB", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAddress", AddressTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Added Successfully");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
        private void Reset()
        {
            Key = 0;
            StNameTb.Text = "";
            FeesTb.Text = "";
            AddressTb.Text = "";
            StGenderCb.SelectedIndex = 0;
            ClassCb.SelectedIndex = 0;
        }

        int Key = 0;

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || FeesTb.Text == "" || AddressTb.Text == "" || StGenderCb.SelectedIndex == -1 || ClassCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE StudentsTbl SET StuName=@Sname,StuGender=@SGender,StuDOB=@SDOB,StuClass=@SClass,StuAddress=@SAddress,StuFees=@SFees WHERE StuId=@SID", Con);
                    cmd.Parameters.AddWithValue("@Sname", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SGender", StGenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SDOB", DOBPicker.Value.Date);
                    cmd.Parameters.AddWithValue("@SClass", ClassCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@SFees", FeesTb.Text);
                    cmd.Parameters.AddWithValue("@SAddress", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@SID", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Updated Successfully");
                    Con.Close();
                    DisplayStudent();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Please Select Student");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM StudentsTbl WHERE StuId=@StKey", Con);
                    cmd.Parameters.AddWithValue("@StKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student has been deleted successfully");
                    Con.Close();
                    DisplayStudent();
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

        private void StudentsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StNameTb.Text = StudentsDGV.SelectedRows[0].Cells[1].Value.ToString();
            StGenderCb.SelectedItem = StudentsDGV.SelectedRows[0].Cells[2].Value.ToString();
            DOBPicker.Text = StudentsDGV.SelectedRows[0].Cells[3].Value.ToString();
            ClassCb.SelectedItem = StudentsDGV.SelectedRows[0].Cells[4].Value.ToString();
            FeesTb.Text = StudentsDGV.SelectedRows[0].Cells[5].Value.ToString();
            AddressTb.Text = StudentsDGV.SelectedRows[0].Cells[6].Value.ToString();
            if (StNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(StudentsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
