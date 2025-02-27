using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Budget_Family
{
    public partial class Login : Form
    {
        public static string Name, Authoriza,AccountName;
        public Login()
        {
            InitializeComponent();
        }

        string connection = @"Data Source=LAPTOP-A08PHTR5\SQLEXPRESS;Initial Catalog=BudgetFamily;Integrated Security=True;Encrypt=False";
        
        SqlConnection conn;
        
       
        
        private void Login_Load(object sender, EventArgs e)
        {
            conn= new SqlConnection(connection);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Method.Showpassword(checkBox1, txbPassword);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool isDone = false;
            string AccountID = txbAccount.Text;
            string Password = txbPassword.Text;

            using (SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-A08PHTR5\SQLEXPRESS;Initial Catalog=BudgetFamily;Integrated Security=True;Encrypt=False"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Password, Authoriza, Name,AccountID FROM Account WHERE AccountID=@AccountID", conn))
                {
                    cmd.Parameters.AddWithValue("@AccountID", AccountID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader.GetString(0);
                            Authoriza = reader.GetString(1);
                            Name = reader.GetString(2);
                            AccountName = reader.GetString(3);
                            if (storedPassword == Password) 
                            {
                                
                                isDone = true;
                            }
                            else
                            {
                                
                                txbAccount.Focus();
                                MessageBox.Show("Invalid username or password.");
                            }
                        }
                        else
                        {
                            txbAccount.Focus();
                            MessageBox.Show("Invalid username or password.");
                        }
                    }
                }
            }
            if (isDone)
            {

                this.Hide();
                App dashboard = new App();
                dashboard.Show();
            }



        }

        private void txbAccount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register register = new Register();
            register.Show();
        }
    }
}
