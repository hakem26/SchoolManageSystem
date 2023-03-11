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
    public partial class Fees : Form
    {
        public Fees()
        {
            InitializeComponent();
            DisplayFees();
            FillStuId();
        }

        private void FillStuId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * from StudentsTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("StuId", typeof(int));
            dt.Load(rdr);
            StIDCb.ValueMember = "StuId";
            StIDCb.DataSource = dt;
            Con.Close();
        }

        private void DisplayFees()
        {
            Con.Open();
            string Query = "SELECT * FROM FeesTbl";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            FeesDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        SqlConnection Con = new SqlConnection(MainSMS.connectionString);
        private void backBtn_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void GetStudName()
        {
            Con.Open();
            SqlCommand Cmd = new SqlCommand("SELECT * FROM StudentsTbl WHERE StuId=@SID", Con);
            Cmd.Parameters.AddWithValue("@SID", StIDCb.SelectedValue.ToString());
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(Cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                StNameTb.Text = dr["StuName"].ToString();
            }
            Con.Close();
        }

        private void Reset()
        {
            AmountTb.Text = "";
            StNameTb.Text = "";
            StIDCb.SelectedIndex = -1;
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || AmountTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                string paymentperiode;
                paymentperiode = PeriodDate.Value.Month.ToString() + "/" + PeriodDate.Value.Year.ToString();
                Con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) from FeesTbl WHERE StuId = '" + StIDCb.SelectedValue.ToString() + "' and Month = '" + paymentperiode.ToString() + "'", Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("There is no due for this Month");
                }
                else
                {
                    // Con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO FeesTbl(StuId,StuName,Month,Amount) values (@SId,@SName,@SMonth,@SAmt)", Con);
                    cmd.Parameters.AddWithValue("@SId", StIDCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@SName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@SMonth", paymentperiode);
                    cmd.Parameters.AddWithValue("@SAmt", AmountTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Fees Successfully Paid");

                }
                Con.Close();
                DisplayFees();
                Reset();
            }
        }

        private void StIDCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetStudName();
        }

        int Key = 0;
        private void FeesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StIDCb.SelectedValue = FeesDGV.SelectedRows[0].Cells[1].Value.ToString();
            StNameTb.Text = FeesDGV.SelectedRows[0].Cells[2].Value.ToString();
            PeriodDate.Text = FeesDGV.SelectedRows[0].Cells[3].Value.ToString();
            AmountTb.Text = FeesDGV.SelectedRows[0].Cells[4].Value.ToString();

            if (StNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(FeesDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (StNameTb.Text == "" || AmountTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE FeesTbl SET StuId=@StId,StuName=@StName,Month=@AttDate,Amount=@AmtStatus WHERE PayId=@PayId", Con);
                    cmd.Parameters.AddWithValue("@StId", StIDCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StName", StNameTb.Text);
                    cmd.Parameters.AddWithValue("@AttDate", PeriodDate.Value.Date);
                    cmd.Parameters.AddWithValue("@AmtStatus", AmountTb.Text.ToString());
                    cmd.Parameters.AddWithValue("@PayId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Fees Updated Successfully");
                    Con.Close();
                    DisplayFees();
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
                MessageBox.Show("Please Select Fee");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM FeesTbl WHERE PayId=@PayId", Con);
                    cmd.Parameters.AddWithValue("@PayId", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Fee has been deleted successfully");
                    Con.Close();
                    DisplayFees();
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
