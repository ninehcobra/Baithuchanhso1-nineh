using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baithuchanhso1
{
    public partial class FormViewFille : Form
    {
        private List<string> mediaPaths;
        private int itemsPerRow = 3;
        public FormViewFille(List<string> paths)
        {
            InitializeComponent();
            mediaPaths = paths;
            DisplayMediaList();
        }

        private void DisplayMediaList()
        {
            // Thiết lập các thuộc tính của FlowLayoutPanel
            flowLayoutPanel.AutoScroll = true;
            flowLayoutPanel.AutoSize = true;
            flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel.Padding = new Padding(10);

            // Thêm FlowLayoutPanel vào form
            Controls.Add(flowLayoutPanel);

            int itemCount = 0; // Biến đếm số lượng phần tử đã được thêm vào hàng hiện tại

            // Duyệt qua danh sách các đường dẫn
            foreach (string path in mediaPaths)
            {
                if (File.Exists(path))
                {
                    Control mediaControl = null;

                    if (IsImage(path))
                    {
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Image = Image.FromFile(path);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox.Size = new Size(200, 200); // Thiết lập kích thước cố định cho ảnh
                        pictureBox.Margin = new Padding(5);
                        pictureBox.Click += (sender, e) => OpenFullScreen(path);
                        mediaControl = pictureBox;
                    }
                    else if (IsVideo(path))
                    {
                        // Tạo VideoPlayerControl cho video và thiết lập đường dẫn
                        Video videoPlayerControl = new Video();
                        videoPlayerControl.SetVideoPath(path);

                        // Thêm VideoPlayerControl vào FlowLayoutPanel
                        flowLayoutPanel.Controls.Add(videoPlayerControl);
                    }
                    if (mediaControl != null)
                    {
                        // Thêm mediaControl vào FlowLayoutPanel
                        flowLayoutPanel.Controls.Add(mediaControl);
                        itemCount++;

                        // Nếu đã đủ số lượng phần tử trên hàng, thêm một hàng mới và reset biến đếm
                        if (itemCount == itemsPerRow)
                        {
                            flowLayoutPanel.SetFlowBreak(mediaControl, true); // Xuống hàng
                            itemCount = 0; // Reset biến đếm
                        }
                    }
                }
            }
        }
        private void OpenFullScreen(string path)
        { // Kiểm tra xem đường dẫn là cho ảnh hay video
            if (IsImage(path))
            {
                // Nếu là ảnh, hiển thị ảnh ở chế độ toàn màn hình
                FormFullScreenImage fullScreenImageForm = new FormFullScreenImage(path);
                fullScreenImageForm.ShowDialog();
            }
            else if (IsVideo(path))
            {
                // Nếu là video, hiển thị video ở chế độ toàn màn hình
                FormFullScreenVideo fullScreenVideoForm = new FormFullScreenVideo(path);
                fullScreenVideoForm.ShowDialog();
            }
        }

        private bool IsImage(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp";
        }

        private bool IsVideo(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            return extension == ".mp4" || extension == ".avi" || extension == ".wmv" || extension == ".mov" || extension == ".mkv";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

