using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baithuchanhso1
{
    public partial class FormFullScreenImage : Form
    {
        private string imagePath;
        public FormFullScreenImage(string imagePath)
        {
            InitializeComponent();
            this.imagePath = imagePath;
            ShowFullScreenImage();
        }

        private void ShowFullScreenImage()
        {
            // Đọc ảnh từ đường dẫn và hiển thị nó ở chế độ toàn màn hình
            try
            {
                Image image = Image.FromFile(imagePath);
                pictureBox.Image = image;
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể hiển thị ảnh: " + ex.Message);
                Close();
            }
        }

        private void FormFullScreenImage_KeyDown(object sender, KeyEventArgs e)
        {
            // Đóng form khi nhấn phím Escape
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
