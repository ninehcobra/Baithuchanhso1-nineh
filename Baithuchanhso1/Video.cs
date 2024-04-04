using AxWMPLib;
using System;
using System.Windows.Forms;

namespace Baithuchanhso1
{
    public partial class Video : UserControl
    {
        private AxWindowsMediaPlayer axWindowsMediaPlayer;

        public Video()
        {
            InitializeComponent();
            InitializeVideoPlayer();
        }

        private void InitializeVideoPlayer()
        {
            // Tạo control AxWindowsMediaPlayer
            axWindowsMediaPlayer = new AxWindowsMediaPlayer();
            axWindowsMediaPlayer.BeginInit(); // Bắt đầu quá trình khởi tạo
            axWindowsMediaPlayer.Dock = DockStyle.Fill;

            // Thêm control vào UserControl
            Controls.Add(axWindowsMediaPlayer);

            axWindowsMediaPlayer.EndInit(); // Kết thúc quá trình khởi tạo

            // Khi control đã được khởi tạo, đặt thuộc tính autoStart
            axWindowsMediaPlayer.settings.autoStart = false;
        }

        // Phương thức để thiết lập đường dẫn của video
        public void SetVideoPath(string path)
        {
            // Kiểm tra đường dẫn video có hợp lệ không
            if (!string.IsNullOrEmpty(path))
            {
                axWindowsMediaPlayer.URL = path;
            }
            else
            {
                // Xử lý trường hợp đường dẫn không hợp lệ
                MessageBox.Show("Đường dẫn video không hợp lệ.");
            }
        }
    }
}
