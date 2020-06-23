using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xcat
{
    public partial class DonateFrm : Form
    {
        public DonateFrm()
        {
            InitializeComponent();
        }


        private void btOk_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void DonateFrm_Load(object sender, EventArgs e)
        {

            //string[] files = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            //foreach (var str in files)
            //{
            //    Console.WriteLine(str);
            //}

            LoadImage(this.pbZFB, "xcat.Asset.image.zfb.png");

            LoadImage(this.pbWX, "xcat.Asset.image.wx.png");

        }

        private void LoadImage(PictureBox pb, string image)
        {

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(image);

            if (stream != null)
            {
                pb.Image = new Bitmap(stream);
                stream.Close();
            }

        }

        private void DonateFrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
