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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Baithuchanhso1
{
    public partial class FormRegister : Form
    {

        private string nameText = "Enter your name here";
        private string emailText = "example@gmail.com";
        private string passwordText = "8+ characters";
        string language;
        string appTheme;
        public FormRegister()
        {
            InitializeComponent();
            CreateRegisterButton();
            (language, appTheme) = LoadConfig();
            LoadLanguageAndTheme();
            txtName.Text = nameText;
            txtName.ForeColor = SystemColors.GrayText;
            txtPassword.Text = passwordText;
            txtPassword.ForeColor= SystemColors.GrayText;
            txtEmail.Text = emailText;
            txtEmail.ForeColor= SystemColors.GrayText;

        }

        public void LoadLanguageAndTheme()
        {
            nameText = language == "EN" ? "Enter your name here" : "Nhập tên của bạn ở đây";
            label1.Text = language == "EN" ? "Welcome to AppChat" : "Chào mừng đến AppChat";
            label2.Text = language == "EN" ? "Register your account" : "Đăng ký tài khoản của bạn";
            label3.Text = language == "EN" ? "Name" : "Tên";
            label5.Text = language == "EN" ? "Password" : "Mật khẩu";
            label8.Text = language == "EN" ? "Image" : "Ảnh";
            button2.Text = language == "EN" ? "Choose image" : "Chọn ảnh";
            btnRegister.Text = language == "EN" ? "Register" : "Đăng ký";
            label6.Text = language == "EN" ? "Already have an account?" : "Đã có tài khoản?";
            label7.Text = language == "EN" ? "Register with" : "Đăng ký với";
            button1.Text = language == "EN" ? "Login" : "Đăng nhập";
            panel2.BackColor = appTheme == "light" ? SystemColors.Window : System.Drawing.ColorTranslator.FromHtml("#212121");
            panel1.BackColor = appTheme == "light" ? SystemColors.MenuBar : System.Drawing.ColorTranslator.FromHtml("#171717");
            btnRegister.BackColor = appTheme == "light" ? System.Drawing.ColorTranslator.FromHtml("#7B68EE") : System.Drawing.ColorTranslator.FromHtml("#841097");
            label1.ForeColor = appTheme == "light" ? SystemColors.ControlText : Color.White;
            label3.ForeColor = appTheme == "light" ? SystemColors.ControlText : Color.White;
            label4.ForeColor = appTheme == "light" ? SystemColors.ControlText : Color.White;
            label5.ForeColor = appTheme == "light" ? SystemColors.ControlText : Color.White;
            label8.ForeColor = appTheme == "light" ? SystemColors.ControlText : Color.White;
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

        void CreateRegisterButton()
        {
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            btnRegister.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, 0, 0, 0);
            btnRegister.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 0, 0, 0);
            btnRegister.BackColor = Color.MediumSlateBlue;
            btnRegister.ForeColor = Color.White;
            btnRegister.Size = new Size(100, 40);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            if (txtName.Text == nameText)
            {
                txtName.Text = "";
                txtName.ForeColor = SystemColors.WindowText;
            }
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                txtName.Text = nameText;
                txtName.ForeColor = SystemColors.GrayText;
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

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = emailText;
                txtEmail.ForeColor = SystemColors.GrayText;
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

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = passwordText;
                txtPassword.ForeColor = SystemColors.GrayText;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin formLogin = new FormLogin();
            formLogin.Show();
          
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string avatarPath = txtAvatarPath.Text;

            if (name == "" || email == "" || password == "" || avatarPath == "" || name == nameText || email == emailText || password == passwordText)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!!");
            }
            else if (IsEmailExists(email))
            {
                MessageBox.Show("Email đã tồn tại!!");
            }
            else
            {
                int id = GetNextUserId(); // Lấy id của người dùng tiếp theo

                string userInfo = $"{id}|{name}|{email}|{password}|{avatarPath}";
                WriteToFile("users.txt", userInfo);
                MessageBox.Show("Đăng ký thành công!!!");
                this.Hide();
                FormLogin form = new FormLogin();
                form.Show();
            }
        }

        private int GetNextUserId()
        {
            // Đảm bảo tệp tồn tại trước khi đọc
            if (!File.Exists("users.txt"))
            {
                return 1; // Nếu tệp không tồn tại, trả về id = 1
            }

            // Đọc tất cả các dòng trong tệp
            string[] lines = File.ReadAllLines("users.txt");

            // Nếu không có dòng nào trong tệp, trả về id = 1
            if (lines.Length == 0)
            {
                return 1;
            }

            // Lấy id của người dùng cuối cùng và tăng lên 1
            string lastLine = lines[lines.Length - 1];
            string[] userInfo = lastLine.Split('|');
            int lastUserId = int.Parse(userInfo[0]);
            return lastUserId + 1;
        }

        private bool IsEmailExists(string email)
        {
            // Đảm bảo tệp tồn tại trước khi đọc
            if (!File.Exists("users.txt"))
            {
                return false; // Nếu tệp không tồn tại, email chưa tồn tại
            }

            // Đọc tất cả các dòng trong tệp
            string[] lines = File.ReadAllLines("users.txt");

            // Kiểm tra từng dòng để tìm email
            foreach (string line in lines)
            {
                string[] userInfo = line.Split('|');
                string existingEmail = userInfo[2]; // email nằm ở vị trí thứ 2 trong dòng
                if (existingEmail == email)
                {
                    return true; // Nếu tìm thấy email, email đã tồn tại
                }
            }

            return false; // Nếu không tìm thấy email, email chưa tồn tại
        }

        private void WriteToFile(string filePath, string userInfo)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(userInfo);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Lấy đường dẫn của tệp được chọn
                    string selectedFilePath = openFileDialog.FileName;

                    // Hiển thị hình ảnh trong PictureBox
                    picAvatar.ImageLocation = selectedFilePath;

                    // Hiển thị đường dẫn của tệp trong TextBox
                    txtAvatarPath.Text = selectedFilePath;
                }
            }
        }

       
    }
}
