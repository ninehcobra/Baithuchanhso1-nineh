using System;
using System.Drawing;
using System.Windows.Forms;

namespace Baithuchanhso1
{
    public partial class MessageSentControl : UserControl
    {
        int maxWidth = 800;

        public MessageSentControl()
        {
            InitializeComponent();
            txtMessage.WordWrap = true; // Đặt WordWrap của textbox thành true
            this.AutoSize = true; // Thiết lập AutoSize thành true
            this.Resize += MessageControl_Resize; // Đăng ký sự kiện Resize
        }

        private void MessageControl_Resize(object sender, EventArgs e)
        {
            AdjustTextBoxWidth();
            AdjustTextBoxHeight();
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                txtMessage.Text = InsertLineBreaks(value);
                AdjustTextBoxWidth();
                AdjustTextBoxHeight();
            }
        }

        private string _time;

        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                lblTime.Text = value;

            }
        }

        public void HighlightText(int startIndex, int length, Color color)
        {
            if (txtMessage.InvokeRequired)
            {
                txtMessage.Invoke(new MethodInvoker(() => HighlightText(startIndex, length, color)));
            }
            else
            {
                txtMessage.Select(startIndex, length);
                txtMessage.ForeColor = color;
            }
        }
        private string InsertLineBreaks(string input)
        {
            string wrappedText = "";
            string[] words = input.Split(' ');

            int lineLength = 0;
            foreach (string word in words)
            {
                if (lineLength + word.Length > (maxWidth+20) / (int)txtMessage.Font.Size)
                {
                    wrappedText = wrappedText.TrimEnd() + "\n" + word + " "; // Chèn "\n" vào cuối từ trước
                    lineLength = word.Length;
                }
                else
                {
                    wrappedText += word + " ";
                    lineLength += word.Length;
                }
            }

            return wrappedText.TrimEnd(); // Loại bỏ khoảng trắng dư thừa và ký tự xuống dòng cuối cùng
        }

        private void AdjustTextBoxHeight()
        {
            // Tính toán chiều cao của TextBox dựa trên độ dài của tin nhắn
            int lines = txtMessage.Text.Split('\n').Length;
            int textBoxHeight = lines * txtMessage.Font.Height + txtMessage.Margin.Vertical + 10;

            // Thiết lập chiều cao mới cho TextBox
            txtMessage.Height = textBoxHeight;
        }

        private void AdjustTextBoxWidth()
        {
            // Tính toán chiều rộng mới cho TextBox dựa trên độ dài của tin nhắn
            SizeF textSize = TextRenderer.MeasureText(_message, txtMessage.Font);
            txtMessage.Width = Math.Min(((int)Math.Ceiling(textSize.Width) + 20), maxWidth);
        }

        public void ChangeLabelColor(Color color)
        {
            txtMessage.ForeColor = color;
        }
    }
}
