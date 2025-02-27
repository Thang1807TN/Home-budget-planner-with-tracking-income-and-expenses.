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

namespace Budget_Family
{
    public partial class Show : Form
    {

        List<string> listType = new List<string>() {"Income","Expense"};
        List<string> listCategoryEx = new List<string>() { "Food","Bill", "Transport", "Entertainment", "Health", "Education", "Different" };
        List<string> listCategoryIn = new List<string>() { "Salary", "Gift", "Different" };


        string connection = @"Data Source=LAPTOP-A08PHTR5\SQLEXPRESS;Initial Catalog=BudgetFamily;Integrated Security=True;Encrypt=False";
        string sql;
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;
        
        public Show()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Show_Load(object sender, EventArgs e)
        {
            cboType.DataSource = listType;
            ShowData();
            dtpkExpense.Format = DateTimePickerFormat.Custom;
            dtpkExpense.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            App app = new App();
            app.Show();
        }

        public void ShowData()
        {
            listView1.Items.Clear();
            conn = new SqlConnection(connection);
            conn.Open();
            sql = @"SELECT PersonName, Amount, InorEx, Date,Comment,Category FROM BudgetPlan";
            cmd = new SqlCommand(sql, conn);
            dr = cmd.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
               listView1.Items.Add(dr[0].ToString());
                listView1.Items[i].SubItems.Add(dr[1].ToString());
                listView1.Items[i].SubItems.Add(dr[3].ToString());
                listView1.Items[i].SubItems.Add(dr[2].ToString());
                listView1.Items[i].SubItems.Add(dr[5].ToString());
                listView1.Items[i].SubItems.Add(dr[4].ToString());
                i++;
            }
            conn.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

            if (CheckAmount())
            {
                listView1.Items.Clear();
                conn = new SqlConnection(connection);
                conn.Open();
                sql = @"INSERT INTO BudgetPlan (PersonName, Amount, InorEx, Date, Comment,Category,AccountName) VALUES (N'" + Login.Name + @"',N'" + txbAmount.Text + @"', N'" + cboType.Text + @"', N'" + dtpkExpense.Value + @"', N'" + txbComment.Text + @"', N'" + cboCategory.Text + @"',N'" + Login.AccountName + @"')";
                cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                dtpkExpense.Value = DateTime.Now;
                txbAmount.Text = "";
                txbComment.Text = "";
                ShowData();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void lbComment_Click(object sender, EventArgs e)
        {

        }

        private void listView1_Click(object sender, EventArgs e)
        {
            
            txbAmount.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txbComment.Text = listView1.SelectedItems[0].SubItems[5].Text;
            cboType.Text = listView1.SelectedItems[0].SubItems[3].Text;
            dtpkExpense.Text = listView1.SelectedItems[0].SubItems[2].Text;
            cboCategory.Text = listView1.SelectedItems[0].SubItems[4].Text;
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.Text == "Expense")
            {
                cboCategory.DataSource = listCategoryEx;
            }
            else
            {
                cboCategory.DataSource = listCategoryIn;
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if(Login.Authoriza=="Admin")
            {
                if (CheckAmount())
                {
                    Updatebtn();
                }
            }
            else
            {
                if (CheckAuthorization())
                {
                    if (CheckAmount())
                    {
                        Updatebtn();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Login.Authoriza == "Admin")
            {
                if (CheckAuthorization())
                {
                    Deletebtn();
                }
            }
            else
            {
                if (CheckAuthorization())
                {
                    if (CheckAmount())
                    {
                        Deletebtn();
                    }
                }
            }
        }
        

        public bool CheckAuthorization()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string sql = @"SELECT AccountName FROM BudgetPlan WHERE Date = @Date";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", dtpkExpense.Value);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read()) 
                        {
                            string Name = dr["AccountName"].ToString();

                            if (Name != Login.AccountName)
                            {
                                MessageBox.Show("You don't have permission to change or delete this data, you can only modify or delete data that you have added!",
                                                "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("No data found for the selected date!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }
        }

        public void Updatebtn()
        {
            listView1.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connection))
            {

                conn.Open();
                string sql = @"UPDATE BudgetPlan 
                           SET Amount = @Amount, 
                               InorEx = @InorEx, 
                               Comment = @Comment, 
                               Category = @Category 
                           WHERE Date = @Date";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Amount", txbAmount.Text);
                    cmd.Parameters.AddWithValue("@InorEx", cboType.Text);
                    cmd.Parameters.AddWithValue("@Comment", txbComment.Text);
                    cmd.Parameters.AddWithValue("@Category", cboCategory.Text);
                    cmd.Parameters.AddWithValue("@Date", dtpkExpense.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Update successful!", "Notificate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Don't have data to update!", "Notificate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }

            }
            dtpkExpense.Value = DateTime.Now;
            txbAmount.Text = "";
            txbComment.Text = "";
            ShowData();
        }

        public void Deletebtn()
        {
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string sql = "DELETE FROM BudgetPlan WHERE Date = @Date";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Date", dtpkExpense.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Delete successfull!", "Notificate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Don't have data to delete!", "Notificate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                ShowData();
            }
        }

        public bool CheckAmount()
        {
            if (!decimal.TryParse(txbAmount.Text, out decimal amount))
            {
                MessageBox.Show("Please enter a valid number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbAmount.Focus();
                return false;
               
            }

            
            if (amount < 0)
            {
                MessageBox.Show("The amount cannot be negative!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbAmount.Text = ""; txbAmount.Focus();
                return false;
                
            }
            return true;
        }
    }
}
