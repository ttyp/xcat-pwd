using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xcat
{
    public partial class PasswordFrm : Form
    {
        public PasswordFrm()
        {
            InitializeComponent();
            this.lv.OwnerDraw = true;
            this.lv.DrawColumnHeader += lv_DrawColumnHeader;
            this.lv.DrawItem += lv_DrawItem;
            this.lv.DrawSubItem += lv_DrawSubItem;
            this.Load += PasswordFrm_Load;
        }

        private void PasswordFrm_Load(object sender, EventArgs e)
        {

#if SINGLE
            this.toolbar.Items.Add(new ToolStripSeparator());
            
            ToolStripButton tsbDonate = new System.Windows.Forms.ToolStripButton();

            tsbDonate.Image = global::xcat.Properties.Resources.donate;
            tsbDonate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            tsbDonate.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbDonate.Name = "tsbEdit";
            tsbDonate.Size = new System.Drawing.Size(43, 56);
            tsbDonate.Text = "捐赠";
            tsbDonate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            tsbDonate.Click += (s, ee) =>
            {
                DonateFrm frm = new DonateFrm();
                frm.ShowDialog();

            };
      

            this.toolbar.Items.Add(tsbDonate);
#endif

            foreach (var item in this.PasswordRecord.Records)
            {
                var pi = item.Value;

                ListViewItem lvi = new ListViewItem(pi.Name);

                lvi.Tag = pi.Password;

                lvi.SubItems.Add(pi.Link);
                lvi.SubItems.Add(pi.User);
                lvi.SubItems.Add("******");
                lvi.SubItems.Add(pi.Memo);

                this.lv.Items.Add(lvi);
            }
        }

        public PasswordFrm(PasswordRecord pr):this()
        {
            this.PasswordRecord = pr;
        }

        public PasswordRecord PasswordRecord {get;set;}

        private void tsbExit_Click(object sender, EventArgs e)
        {
#if SINGLE
            Application.Exit();
#else
            this.Close();
#endif
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            PasswordSite frm = new PasswordSite();

            if (frm.ShowDialog(this) == DialogResult.OK)
            {

                ListViewItem item = new ListViewItem(frm.Title);

                item.SubItems.Add(frm.Link);
                item.SubItems.Add(frm.User);
                item.SubItems.Add("******");
                item.SubItems.Add(frm.Description);
                item.Tag = frm.Password;

                this.lv.Items.Add(item);

                this.save();

            }
        }

        private void tsbRemove_Click(object sender, EventArgs e)
        {
            for (var i = this.lv.Items.Count - 1; i >= 0; i--)
            {
                if (this.lv.Items[i].Selected)
                {
                    this.lv.Items.RemoveAt(i);
                }
            }

            this.save();
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            if (lv.SelectedItems.Count > 0)
            {
                SiteEdit(lv.SelectedItems[0]);
            }
        }

        private void SiteEdit(ListViewItem item)
        {
            PasswordSite frm = new PasswordSite();

            string title = item.Text;


            string link = item.SubItems[1].Text;

            string user = item.SubItems[2].Text;
            string password = item.Tag.ToString();

            string description = item.SubItems[4].Text;

            frm.InitData(title, link, user,password,description);

            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                item.Text = frm.Title;

                item.SubItems[1].Text = frm.Link;
                item.SubItems[2].Text = frm.User;
                item.SubItems[3].Text = "******";
                item.Tag = frm.Password;
                item.SubItems[4].Text = frm.Description;

                this.save();
            }
        }

        private void save()
        {
            this.PasswordRecord.Records.Clear();

            for (var i = 0;i< this.lv.Items.Count; i++)
            {
                ListViewItem item = this.lv.Items[i];

                this.PasswordRecord.Add(new PasswordRecord.PasswordInfo()
                {
                    Name = item.Text,
                    Link = item.SubItems[1].Text,
                    User = item.SubItems[2].Text,
                    Password = item.Tag.ToString(),
                    Memo = item.SubItems[4].Text
          
                });
            }

            this.PasswordRecord.Save();
        }

        private void tsbReset_Click(object sender, EventArgs e)
        {
            PasswordResetFrm frm = new PasswordResetFrm(this.PasswordRecord.Password);

            if(frm.ShowDialog(this)== DialogResult.OK)
            {
                this.PasswordRecord.Password = frm.Password;
                this.PasswordRecord.Save();
            }
        }

        private void PasswordFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.lv.SelectedItems.Count > 0)
            {
                SiteEdit(this.lv.SelectedItems[0]);
            }
        }


        private void lv_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.SubItems[3].Text = e.Item.Tag.ToString();
            }
            else
            {
                e.Item.SubItems[3].Text = "******";
            }
        }

        private void lv_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            
           e.DrawDefault = true;
        }

        private void lv_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lv_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

    }
}
