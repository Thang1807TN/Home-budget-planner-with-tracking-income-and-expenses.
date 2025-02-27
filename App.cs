using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Budget_Family
{
    
    public partial class App : Form
    {
        string connection = @"Data Source=LAPTOP-A08PHTR5\SQLEXPRESS;Initial Catalog=BudgetFamily;Integrated Security=True;Encrypt=False";
        string sql;
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader dr;

        public bool isClose = true;
        public App()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            isClose = false;
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (isClose)
                Application.Exit();
        }

        private void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (isClose)
            //{
            //    if (MessageBox.Show("Do you want to exit.", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }

        private void App_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isClose)
            {
                Application.Exit();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void App_Load(object sender, EventArgs e)
        {

            if(Login.Authoriza == "user")
            {
                btnChangeBudget.Visible = false;
                btnChangeBudget.Enabled = false;
                txbBudget.ReadOnly= true;
            }

            label2.Text = DateTime.Now.ToString("Y");
            label2.Left = (this.ClientSize.Width - label2.Width) / 2;
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;
           
            lbNameAccount.Text = "Hello "+Login.Name;
            ShowData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void lbBill_Click(object sender, EventArgs e)
        {

        }

        private void lbIncome_Click(object sender, EventArgs e)
        {

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            this.Hide();
            Show show = new Show();
            show.Show();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

        }

        private void lbNameAccount_Click(object sender, EventArgs e)
        {

        }

        private void ShowData()
        {
            object income = null;
            object expense = null;

            using (SqlConnection conn = new SqlConnection(connection))
            {
                try
                {
                    conn.Open();

                    string sql = "SELECT SUM(Amount) FROM BudgetPlan WHERE InorEx = 'Income'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        income = cmd.ExecuteScalar();
                        txbIncome.Text = (income != DBNull.Value) ? income.ToString() : "0";
                    }

                    sql = "SELECT Budget FROM BudgetPlan WHERE PersonName = 'Budget'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txbBudget.Text = dr["Budget"].ToString();
                        }
                        else
                        {
                            txbBudget.Text = "Do not have data!";
                        }
                    }

                    sql = "SELECT SUM(Amount) FROM BudgetPlan WHERE Category = 'Bill'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        txbBill.Text = (result != DBNull.Value) ? result.ToString() : "0";
                    }

                    sql = "SELECT SUM(Amount) FROM BudgetPlan WHERE InorEx = 'Expense'";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        expense = cmd.ExecuteScalar();
                        txbExpense.Text = (expense != DBNull.Value) ? expense.ToString() : "0";
                    }

                    txbBalance.Text = (Convert.ToDouble(txbIncome.Text) - Convert.ToDouble(txbExpense.Text)).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnChangeBudget_Click(object sender, EventArgs e)
        {
            conn = new SqlConnection(connection);
            conn.Open();
            sql = @"UPDATE BudgetPlan SET Budget = N'" + txbBudget.Text + @"' WHERE PersonName = 'Budget'";
            cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            ShowData();
        }

        private void App_Resize(object sender, EventArgs e)
        {
            //label1.Left = (this.ClientSize.Width - label1.Width) / 2;
            //label2.Left = (this.ClientSize.Width - label2.Width) / 2;

            //// Căn giữa tên tài khoản
            //lbNameAccount.Left = (this.ClientSize.Width - lbNameAccount.Width) / 2;

            //// Điều chỉnh kích thước của các TextBox
            //int padding = 20;
            //int width = this.ClientSize.Width - 2 * padding;

            //txbIncome.Width = width;
            //txbExpense.Width = width;
            //txbBudget.Width = width;
            //txbBill.Width = width;
            //txbBalance.Width = width;

            //// Điều chỉnh vị trí của các TextBox
            //txbIncome.Left = padding;
            //txbExpense.Left = padding;
            //txbBudget.Left = padding;
            //txbBill.Left = padding;
            //txbBalance.Left = padding;

            // Điều chỉnh ListView nếu có
            //listView1.Width = width;
            //listView1.Left = padding;
            //listView1.Height = this.ClientSize.Height - listView1.Top - padding;
        }
    }
}
