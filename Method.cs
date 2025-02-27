using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Budget_Family
{
    public class Method
    {
        public static void Showpassword(System.Windows.Forms.CheckBox ckb, TextBox txb)
        {
            if (ckb.Checked)
            {
                txb.UseSystemPasswordChar = false;
            }
            else
            {
                txb.UseSystemPasswordChar = true;
            }
        }

        public static bool NullTxb(TextBox txb)
        {
            if (txb.Text == "")
            {
                return true; 
                txb.Focus();
            }
            return false;
        }
    }
}
