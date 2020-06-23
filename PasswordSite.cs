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
    public partial class PasswordSite : Form
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string User { get; set; }

        public string Password { get; set; }

        public string Description { get; set; }



        public PasswordSite()
        {
            InitializeComponent();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Data(bool direction)
        {
            if (direction)
            {
                this.tbTitle.Text = this.Title;
                this.tbLink.Text = this.Link;
                this.tbUser.Text = this.User;
                this.tbPassword.Text = this.Password;
                this.tbDescription.Text = this.Description;

            }
            else
            {

                this.Title = this.tbTitle.Text;
                this.Link = this.tbLink.Text;
                this.User = this.tbUser.Text;
                this.Password = this.tbPassword.Text;
                this.Description = this.tbDescription.Text;
            }
        }


        public void InitData(string title, string link, string user, string password, string description)
        {
            this.Title = title;
            this.Link = link;
            this.User = user;
            this.Password = password;
            this.Description = description;

            this.Data(true);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbTitle.Text))
            {
                MessageBox.Show(this, "请输入标题！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbTitle.Focus();
                return;
            }


            if (string.IsNullOrEmpty(this.tbUser.Text))
            {

                MessageBox.Show(this, "请输入用户名！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbUser.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.tbPassword.Text))
            {
                MessageBox.Show(this, "请输入密码！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.tbPassword.Focus();
                return;
            }

            this.Data(false);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btview_Click(object sender, EventArgs e)
        {
            if (this.tbPassword.PasswordChar == '*')
            {
                this.tbPassword.PasswordChar = new char();
                this.btview.Text = "隐藏";
            }
            else
            {
                this.tbPassword.PasswordChar = '*';
                this.btview.Text = "查看";
            }
        }

        private void btAccess_Click(object sender, EventArgs e)
        {
            if (this.tbLink.Text.StartsWith("http://") || this.tbLink.Text.StartsWith("https://"))
            {
                System.Diagnostics.Process.Start(this.tbLink.Text);
            }
        }

        private void btCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.tbPassword.Text);
        }

        private void btMake_Click(object sender, EventArgs e)
        {
            string s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!@#$%^&*()_+`{}[];'\\|,.<>/?";    //自己定义符号
            string r = string.Empty;
            Random random = new Random();
            Enumerable.Repeat<int>(0, 9).ToList().ForEach(x => r += s[random.Next(s.Length)]);

            this.tbPassword.Text = r;

        }
    }
}
