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
    public partial class PasswordLoginFrm : Form
    {
        public PasswordLoginFrm()
        {
            InitializeComponent();
            this.Load += PasswordLoginFrm_Load;
            this.Shown += PasswordLoginFrm_Shown;
        }

        private void PasswordLoginFrm_Shown(object sender, EventArgs e)
        {
            this.tbPassword.Focus();
        }

        private bool isfirst = false;

        private void PasswordLoginFrm_Load(object sender, EventArgs e)
        {
            string file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xcat.pwd");

            if (!System.IO.File.Exists(file))
            {
                this.lbTitle.Text = "请填写初始口令:";
                this.isfirst = true;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            string text = this.tbPassword.Text.Trim();
            string file = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xcat.pwd");

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show(this, "请输入口令!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbPassword.Focus();
                return;
            }

            PasswordRecord pr = new PasswordRecord(file);

            if (isfirst)
            {
                pr.Password = text;
                pr.Save();
            }
            else
            {
                if (!pr.Read(text))
                {
                    MessageBox.Show(this, "密码错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.tbPassword.Focus();
                    this.tbPassword.SelectAll();
                    return;
                }
            }

            PasswordFrm frm = new PasswordFrm(pr);

            frm.Show();
#if SINGLE
            this.Hide();
#else
            this.Close();
#endif

        }

        private void Form_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btOK_Click(this.btOK, EventArgs.Empty);
            }
        }
    }
}
