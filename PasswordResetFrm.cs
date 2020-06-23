using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xcat
{
    public partial class PasswordResetFrm : Form
    {
        public PasswordResetFrm()
        {
            InitializeComponent();
        }

        public string Password { get;set;}

        public PasswordResetFrm(string password):this()
        {
            this.Password = password;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if(this.Password != this.tbPassword.Text)
            {
                MessageBox.Show(this, "原口令不正确!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbPassword.Focus();
                this.tbPassword.SelectAll();
                return;
            }

            if (string.IsNullOrEmpty(this.tbNewPassword.Text))
            {
                MessageBox.Show(this, "新口令不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbNewPassword.Focus();
                return;
            }

            this.Password = this.tbNewPassword.Text;

            this.DialogResult = DialogResult.OK;

            this.Close();

        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
