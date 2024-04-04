using AxWMPLib;
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
    public partial class FormFullScreenVideo : Form
    {
        public FormFullScreenVideo(string videoPath)
        {
            InitializeComponent();
            InitializeVideoPlayer(videoPath);
        }

        private void InitializeVideoPlayer(string videoPath)
        {
            // Tạo một control AxWindowsMediaPlayer



            // Thiết lập đường dẫn cho video
            axWindowsMediaPlayer.URL = videoPath;

         
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Đóng form khi nhấn phím ESC
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    
}
}
