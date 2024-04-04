using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Baithuchanhso1
{
    public partial class ImageVideoControl : UserControl
    {
        private string _filePath;

        public ImageVideoControl()
        {
            InitializeComponent();
        }

        public ImageVideoControl(string filePath,string time)
        {
            InitializeComponent();
            _filePath = filePath;
            lblTime.Text = time;
            LoadMedia();
        }

        private void LoadMedia()
        {
            string extension = Path.GetExtension(_filePath).ToLower();

            // Kiểm tra loại file và hiển thị ảnh hoặc video tương ứng
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
            {
                // Hiển thị ảnh
                pictureBox.ImageLocation = _filePath;
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Size = new Size(245, 245);
                // Set kích thước cố định là 245x245
                if (_filePath.StartsWith(@"C:\Users\ttbex\source\repos\Baithuchanhso1\Baithuchanhso1\bin\Debug\emotion"))
                {
                    pictureBox.Size = new Size(45, 45);
                }
                pictureBox.Visible = true;
                axWindowsMediaPlayer1.Visible = false;
                this.Height= pictureBox.Height+5;
            }
            else if (extension == ".mp4" || extension == ".avi" || extension == ".mov")
            {
                // Hiển thị video
                axWindowsMediaPlayer1.URL = _filePath;
                axWindowsMediaPlayer1.settings.autoStart = false; //
                axWindowsMediaPlayer1.stretchToFit = true;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.Size = new Size(245, 245); // Set kích thước cố định là 245x245
                axWindowsMediaPlayer1.Visible = true;
                pictureBox.Visible = false;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            Form imageForm = new Form();
            imageForm.Text = "Full Size Image";
            imageForm.StartPosition = FormStartPosition.CenterScreen; // Hiển thị form ở giữa màn hình
            imageForm.Size = pictureBox.Image.Size; // Đặt kích thước của form bằng kích thước của hình ảnh

            // Tạo PictureBox mới để hiển thị hình ảnh
            PictureBox pictureBoxLarge = new PictureBox();
            pictureBoxLarge.Image = pictureBox.Image;
            pictureBoxLarge.SizeMode = PictureBoxSizeMode.Zoom; // Hiển thị hình ảnh theo tỷ lệ
            pictureBoxLarge.Dock = DockStyle.Fill; // Đầy đủ kích thước trong form

            // Thêm PictureBox vào Form
            imageForm.Controls.Add(pictureBoxLarge);

            // Hiển thị Form
            imageForm.ShowDialog();
        }
    }
}
