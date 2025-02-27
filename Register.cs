using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;


namespace Budget_Family
{
    public partial class Register : Form
    {

        bool[] isDone = new bool[5];
        TextBox[] textBoxes;
        
        public Register()
        {
            InitializeComponent();
            textBoxes = new TextBox[] { txbAccountName, txbName, txbAge, txbPassword, txbPasswordConfirm };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Register_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit.", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void ckbPassword_CheckedChanged(object sender, EventArgs e)
        {
            Method.Showpassword(ckbPassword, txbPassword);
        }

        private void ckbPasswordConfirm_CheckedChanged(object sender, EventArgs e)
        {
            Method.Showpassword(ckbPasswordConfirm, txbPasswordConfirm);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            StringBuilder errorMessages = new StringBuilder();
            bool ok = true;
            if (!IsValidAge(txbAge, errorMessages))
            {
                isDone[0] = false;
            }

            if (!IsValidName(txbName, errorMessages))
            {
                isDone[1] = false;
            }

            if (!IsValidAccountName(txbAccountName, errorMessages))
            {
                isDone[2] = false;
            }

            if (!IsValidPassword(txbPassword, errorMessages))
            {
                isDone[3] = false;
            }

            NullText(textBoxes, errorMessages);
            ComparePassword(errorMessages);
            string accountId = txbAccountName.Text;
            if (CheckAccountIdExists(accountId))
            {
                errorMessages.AppendLine("AccountId has been exists.");
            }
            else
            {
                isDone[4] = true;
            }

            if (errorMessages.Length > 0)
            {
                MessageBox.Show(errorMessages.ToString(), "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var i in isDone)
            {
                if (i == false)
                {
                    ok = false;
                }
                else if (i == null)
                {
                    ok = true;
                }
                else
                {
                    ok = true;
                }
            }

                if (ok)
                {
                    accountId = txbAccountName.Text;
                    string name = txbName.Text;
                    int age = int.Parse(txbAge.Text);
                    string password = txbPassword.Text;
                    string authoriza = "user";
                    if (InsertAccount(accountId, name, age, password, authoriza))
                    {
                        MessageBox.Show("Register successfully.");
                        this.Hide();
                        Login login = new Login();
                        login.Show();

                    }
                    else
                    {
                        MessageBox.Show("Register failed.");
                    }
                }
            }
        

        private void ComparePassword(StringBuilder errorMessages)
        {
            if (txbPassword.Text != txbPasswordConfirm.Text)
            {
                errorMessages.AppendLine("Password and Confirm Password are not the same.");
                txbPassword.Clear();
                txbPasswordConfirm.Clear();
                txbPassword.Focus();
                isDone[1] = false;
            }
            else
            {
                isDone[1] = true;
            }
        }

       

       
        private void NullText(TextBox[] textBoxes, StringBuilder errorMessages)
        {
            
            List<System.Windows.Forms.TextBox> emptyTextBoxes = new List<System.Windows.Forms.TextBox>();
            foreach (System.Windows.Forms.TextBox tb in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(tb.Text)) 
                {
                    emptyTextBoxes.Add(tb); 
                }
            }
            if (emptyTextBoxes.Count > 0)
            {
                errorMessages.AppendLine($"Have {emptyTextBoxes.Count} empty . Please fill information."
                                );

                emptyTextBoxes[0].Focus();
            }
            else
            {
                isDone[2] = true;
            }
        }

        private bool CheckAccountIdExists(string accountId)
        {
            string connectionString = "Data Source=LAPTOP-A08PHTR5\\SQLEXPRESS;Initial Catalog=BudgetFamily;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT COUNT(1) FROM Account WHERE AccountId = @AccountId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@AccountId", SqlDbType.NVarChar).Value = accountId;

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private bool InsertAccount(string accountId, string name, int age, string password, string authoriza)
        {
            string connectionString = "Data Source=LAPTOP-A08PHTR5\\SQLEXPRESS;Initial Catalog=BudgetFamily;Integrated Security=True;Encrypt=False";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                INSERT INTO Account (AccountID, Name, Age, Password, Authoriza)
                VALUES (@AccountID, @Name, @Age, @Password, @Authoriza)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@AccountID", SqlDbType.NVarChar).Value = accountId;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = name;
                    cmd.Parameters.Add("@Age", SqlDbType.Int).Value = age;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = password;
                    cmd.Parameters.Add("@Authoriza", SqlDbType.NVarChar).Value = authoriza;

                    int rowsAffected = cmd.ExecuteNonQuery();


                    return rowsAffected > 0;
                }
            }
        }

        public bool IsValidName(TextBox txbName, StringBuilder errorMessages)
        {
            if (txbName.Text.Length < 2 || txbName.Text.Length > 19)
            {
                errorMessages.AppendLine("Name must be at least 2 and max 20 characters.");
                txbName.Clear();
                txbName.Focus();
                return false;
            }
            return true;
        }

        public bool IsValidAccountName(TextBox txbAccountName, StringBuilder errorMessages)
        {
            if (txbAccountName.Text.Length < 3 || txbAccountName.Text.Length > 19)
            {
                errorMessages.AppendLine("Account Name must be at least 3 and max 20 characters.");
                txbAccountName.Clear();
                txbAccountName.Focus();
                return false;
            }
            return true;
        }

        public bool IsValidPassword(TextBox txbPassword, StringBuilder errorMessages)
        {
            if (txbPassword.Text.Length < 6 || txbPassword.Text.Length > 10)
            {
                errorMessages.AppendLine("Password must be at least 6 and max 10 characters.");
                txbPassword.Clear();
                txbPassword.Focus();
                return false;
            }
            return true;
        }


            private bool IsValidAge(TextBox txbAge, StringBuilder errorMessages)
        {
           

            if (!int.TryParse(txbAge.Text, out int age))
            {
                errorMessages.AppendLine("Age must be a valid integer.");
                txbAge.Clear(); 
                txbAge.Focus(); 
                return false;
            }

            
            if (age < 0 || age > 120) 
            {
                errorMessages.AppendLine("Age must be between 0 and 120.");
                txbAge.Clear();
                txbAge.Focus();
                return false;
            }

            return true;
        }


    }

}


