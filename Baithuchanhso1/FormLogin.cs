using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Baithuchanhso1
{
    public partial class FormLogin : Form
    {
        string language;
        string appTheme;
        public FormLogin()
        {
            InitializeComponent();
            (language,appTheme)=LoadConfig();
            LoadLanguageAndTheme();
            txtPassword.Text = passwordText;
            txtPassword.ForeColor = SystemColors.GrayText;
            txtEmail.Text = emailText;
            txtEmail.ForeColor = SystemColors.GrayText;
        }

        public void LoadLanguageAndTheme ()
        {
            lblWelcome.Text = language == "EN" ? "Welcome to AppChat" : "Chào mừng đến AppChat";
            lblWelcome.ForeColor = appTheme == "light" ? SystemColors.ActiveCaptionText : Color.White;
            lblPass.Text = language == "EN" ? "Password" : "Mật khẩu";
            lblPass.ForeColor=appTheme=="light"?SystemColors.ControlText:Color.White;
            label4.ForeColor = appTheme == "light" ? SystemColors.ControlText : Color.White;
            lblDes.Text = language == "EN" ? "Log in here" : "Đăng nhập tại đây";
            lblLoginWith.Text = language == "EN" ? "Login with" : "Đăng nhập";
            lblAlready.Text = language == "EN" ? "Not have account?" : "Chưa có tài khoản?";
            label3.Text = language == "EN" ? "Forgot password?" : "Quên mật khẩu?";
            btnRegister.Text = language == "EN" ? "Login" : "Đăng nhập";
            button1.Text = language == "EN" ? "Register" : "Đăng ký";
            panel2.BackColor=appTheme=="light"?SystemColors.Window: System.Drawing.ColorTranslator.FromHtml("#212121");
            panel1.BackColor= appTheme=="light"?SystemColors.MenuBar: System.Drawing.ColorTranslator.FromHtml("#171717");
            btnRegister.BackColor = appTheme == "light" ? System.Drawing.ColorTranslator.FromHtml("#7B68EE") : System.Drawing.ColorTranslator.FromHtml("#841097");
        }

        public static (string, string) LoadConfig()
        {
            const string FilePath = "config.txt";
            // Kiểm tra xem file tồn tại không
            if (File.Exists(FilePath))
            {
                // Đọc dữ liệu từ file
                string data = File.ReadAllText(FilePath);

                // Tách dữ liệu thành language và theme
                string[] parts = data.Split(',');
                if (parts.Length == 2)
                {
                    return (parts[0], parts[1]);
                }
            }

            SaveConfig("EN", "light");
            return ("EN", "light");


            // Trả về giá trị mặc định nếu file không tồn tại hoặc dữ liệu không hợp lệ

        }
        public static void SaveConfig(string language, string theme)
        {
            const string FilePath = "config.txt";
            // Tạo nội dung cần ghi vào file
            string data = $"{language},{theme}";

            // Ghi dữ liệu vào file
            File.WriteAllText(FilePath, data);
        }

        private string emailText = "example@gmail.com";
        private string passwordText = "Enter password here";
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = emailText;
                txtEmail.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == emailText)
            {
                txtEmail.Text = "";
                txtEmail.ForeColor = SystemColors.WindowText;
            }
          
        }

        private void panel2_Leave(object sender, EventArgs e)
        {

        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = passwordText;
                txtPassword.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == passwordText)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = SystemColors.WindowText;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormRegister form = new FormRegister();
            form.Show();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtEmail.Text;
            string password = txtPassword.Text;

            if (IsValidUser(username, password))
            {
                MessageBox.Show(language=="EN"?"Login success!": "Đăng nhập thành công!");
                this.Hide();
                ChatForm form= new ChatForm(username);
                form.Show();
                // Nếu đăng nhập thành công, bạn có thể mở form hoặc thực hiện hành động khác ở đây
            }
            else
            {
                MessageBox.Show(language == "EN" ? "Password or username not correct!":"Tên đăng nhập hoặc mật khẩu không chính xác!");
            }

        }

        private bool IsValidUser(string username, string password)
        {
            string[] lines = File.ReadAllLines("users.txt");

            foreach (string line in lines)
            {
                string[] userInfo = line.Split('|');
                if (userInfo.Length >= 4 && userInfo[2] == username && userInfo[3] == password)
                {
                    return true;
                }
            }

            return false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            string userEmail = txtEmail.Text;
            string newPassword = GenerateRandomPassword();

            if (IsEmailExists(userEmail))
            {
                // Email tồn tại trong tệp users.txt
                SendNewPasswordEmail(txtEmail.Text, newPassword);

                MessageBox.Show(language == "EN" ? "New password was sent to your email!":"Mật khẩu mới đã được gửi đến địa chỉ email của bạn!", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Tiếp tục xử lý gửi email mật khẩu mới ở đây
            }
            else
            {
                // Email không tồn tại trong tệp users.txt
                MessageBox.Show(language == "EN" ? "This email isn't exits!":"Địa chỉ email không tồn tại trong hệ thống!");
            }
            // Gửi email chứa mật khẩu mới
           
        }

        private bool IsEmailExists(string userEmail)
        {
            string[] lines = File.ReadAllLines("users.txt");

            foreach (string line in lines)
            {
                string[] userInfo = line.Split('|');
                if (userInfo.Length >= 4 && userInfo[2] == userEmail) // Kiểm tra xem email có trong tệp không (ở vị trí thích hợp)
                {
                    return true;
                }
            }

            return false;
        }

        private string GenerateRandomPassword()
        {
            // Tạo mật khẩu ngẫu nhiên, ví dụ: mật khẩu gồm 8 ký tự bao gồm chữ cái và số
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SendNewPasswordEmail(string recipientEmail, string newPassword)
        {
            SavePasswordToFile(recipientEmail, newPassword);
            string senderEmail = "giaohangle290302@gmail.com"; // Thay bằng địa chỉ email của bạn
            string senderPassword = "oqjyyhpjsrsdjpiu"; // Thay bằng mật khẩu của bạn
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;



            try
            {
                using (MailMessage mail = new MailMessage(senderEmail, recipientEmail))
                {
                    mail.Subject = "Mật khẩu mới";
                    mail.Body = "Mật khẩu mới của bạn là: " + newPassword;

                    using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);

                        try
                        {
                            smtpClient.Send(mail);
                       
                        }
                        catch (SmtpException ex)
                        {
                            MessageBox.Show("Không thể gửi email: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi :" + ex.Message,"Thông báo lỗi",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavePasswordToFile(string recipientEmail, string newPassword)
        {
            // Lưu mật khẩu mới xuống tệp văn bản
            string filePath = "users.txt"; // Đường dẫn tới tệp văn bản lưu thông tin người dùng
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] userInfo = lines[i].Split('|');
                if (userInfo.Length >= 4 && userInfo[2] == recipientEmail)
                {
                    // Cập nhật mật khẩu mới
                    userInfo[3] = newPassword;
                    lines[i] = string.Join("|", userInfo);
                    break; // Dừng vòng lặp sau khi cập nhật xong
                }
            }

            // Ghi lại các dòng đã được cập nhật vào tệp
            File.WriteAllLines(filePath, lines);
        }
    }
}
