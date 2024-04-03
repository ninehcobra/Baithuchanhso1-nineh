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
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Xml.Linq;

namespace Baithuchanhso1
{
  
  
    public partial class ChatForm : Form
    {
        string PlaceholderText = "Type a message here";
        string PlaceholderTextVN = "Nhập tin nhắn ở đây";
        string userLoggedIn;
        string userChattingWith;
        string language = "EN";
        public ChatForm(string currentUsername)
        {
            InitializeComponent();
            LoadUserData(currentUsername);
            txtChat.Text = language=="EN"? PlaceholderText:PlaceholderTextVN;
            txtChat.ForeColor = SystemColors.GrayText;
            LoadUsers(currentUsername);
            userLoggedIn = currentUsername;
            this.KeyPress += new KeyPressEventHandler(Form_KeyPress);
            Focus();
            pnlChat.Controls.SetChildIndex(chatContent, 0);


        

        }
        private void Form_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Xử lý khi người dùng nhấn phím Enter
                e.Handled = true; // Ngăn chặn ký tự Enter được hiển thị trong TextBox

                string message = txtChat.Text.Trim();

                // Kiểm tra xem tin nhắn có trống không
                if (!string.IsNullOrWhiteSpace(message) && message != PlaceholderText&&message!=PlaceholderTextVN)
                {
                    // Tạo tên tập tin là "message.txt"
                    string filePath = "message.txt";

                    // Mở tập tin để ghi dữ liệu
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        // Ghi tin nhắn vào tập tin, cùng với thông tin của người gửi và người nhận (ví dụ: thời gian gửi)
                        writer.WriteLine($"{DateTime.Now}||{lblName.Text}||{lblChatName.Text}||{message}");
                    }

                    // Xóa nội dung của TextBox sau khi gửi tin nhắn thành công
                    txtChat.Text = "";
                }
                reloadMessage();               // Thực hiện hành động tương ứng với việc nhấn Enter
            }
        }

        public class UserItem
        {
            public string AvatarPath { get; set; }
            public string Username { get; set; }

            public UserItem(string avatarPath, string username)
            {
                AvatarPath = avatarPath;
                Username = username;
            }

            public override string ToString()
            {
                return Username;
            }
        }
        string appTheme;

        private void LoadUsers(string currentUser)
        {
            flowLayoutPanel1.Controls.Clear();

            if (!File.Exists("users.txt"))
            {
                MessageBox.Show("Tệp users.txt không tồn tại!");
                return;
            }

            string[] lines = File.ReadAllLines("users.txt");

            foreach (string line in lines)
            {
                string[] userInfo = line.Split('|');
                string username = userInfo[2];
                string name = userInfo[1];

                if (username != currentUser)
                {
                    string avatarPath = userInfo[4];
                    
                    FriendListItem friendListItem = new FriendListItem(username,name, avatarPath, language);

                    // Thêm sự kiện Click cho FriendListItem
                    friendListItem.Click += FriendListItem_Click;

                    flowLayoutPanel1.Controls.Add(friendListItem);
                }
            }
        }

        private void reloadMessage()
        {
            picWelcome.Visible = false;
            lblWelcome.Visible = false;
            pnlChat.Visible = true;
           
            // Lấy thông tin của người dùng từ FriendListItem
            string username = rUsername;
            string avatarPath = rAvatar;
            string name = rChatName;
            lblReceiver.Text = name;
            lblChatName.Text = username;
            userChattingWith = username;


            List<(string, string, string, string, string)> messages = LoadMessages(userLoggedIn, username);
            DisplayMessages(messages);
        }

        string rUsername;
        string rAvatar;
        string rChatName;

        private void FriendListItem_Click(object sender, EventArgs e)
        {
            picWelcome.Visible = false;
            lblWelcome.Visible=false;
            pnlChat.Visible = true;
            FriendListItem clickedItem = sender as FriendListItem;
            // Lấy thông tin của người dùng từ FriendListItem
            string username = clickedItem.UserName;
            string avatarPath = clickedItem.AvatarPath;
            string name= clickedItem.Name;
            rChatName = name;
            rUsername =username;
            rAvatar = avatarPath;
            lblChatName.Text = username;
            lblChatName.Visible = false;
            lblReceiver.Text = name;
            userChattingWith =username;

            // Hiển thị thông tin của người dùng và thay đổi màu sắc của FriendListItem được chọn
            clickedItem.BackColor = System.Drawing.ColorTranslator.FromHtml("#C7ECFC");

            txtChat.Text = language=="EN"?PlaceholderText:PlaceholderTextVN;
            txtChat.ForeColor = SystemColors.GrayText;

            // Đặt lại màu sắc của các FriendListItem khác
            foreach (FriendListItem item in flowLayoutPanel1.Controls)
            {
                if (item != clickedItem)
                {
                    if (appTheme == "dark")
                    {
                        item.BackColor = System.Drawing.ColorTranslator.FromHtml("#171717");
                    }
                    else
                    {
                        item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F4FCFE");
                    }
                }
            }

            List<(string, string, string, string,string)> messages = LoadMessages(userLoggedIn, username);
            DisplayMessages(messages);
        }

        private void DisplayMessages(List<(string, string, string, string,string)> messages)
        {
            // Xóa tất cả các control cũ khỏi FlowLayoutPanel trước khi hiển thị tin nhắn mới
            chatContent.Controls.Clear();

            // Duyệt qua từng tin nhắn trong danh sách
            foreach ((string sender, string receiver, string message, string time,string type) in messages)
            {
                if(type=="sent")
                {
                    if (IsImageOrVideoMessage(message))
                    {
                        // Tạo một ImageVideoControl
                        ImageVideoControl imageVideoControl = new ImageVideoControl(message, time);

                        // Thiết lập nội dung tin nhắn cho ImageVideoControl


                        imageVideoControl.Dock = DockStyle.Top;

                        // Thêm ImageVideoControl vào FlowLayoutPanel
                        chatContent.Controls.Add(imageVideoControl);
                    }
                    else
                    {
                        // Tin nhắn không chứa ảnh hoặc video, tạo một MessageControl bình thường
                        MessageSentControl messageControl = new MessageSentControl();

                        // Thiết lập nội dung tin nhắn cho MessageControl
                        messageControl.Message = message;
                        messageControl.Time = time;

                        messageControl.Dock = DockStyle.Top;

                        // Thêm MessageControl vào FlowLayoutPanel
                        chatContent.Controls.Add(messageControl);
                    }
                }
                if (type =="receive")
                {
                    if (IsImageOrVideoMessage(message))
                    {
                        // Tạo một ImageVideoControl
                        ImageVideoControlReceive imageVideoControl = new ImageVideoControlReceive(message, time);

                        // Thiết lập nội dung tin nhắn cho ImageVideoControl

                        imageVideoControl.AvatarPath = LoadUserAvatar(sender);
                        imageVideoControl.Dock = DockStyle.Top;

                        // Thêm ImageVideoControl vào FlowLayoutPanel
                        chatContent.Controls.Add(imageVideoControl);
                    }
                    else
                    {

                        // Tin nhắn không chứa ảnh hoặc video, tạo một MessageControl bình thường
                        MessageReceiveControl messageControl = new MessageReceiveControl();

                        // Thiết lập nội dung tin nhắn cho MessageControl
                        messageControl.Message = message;
                        messageControl.Time = time;
                        messageControl.AvatarPath = LoadUserAvatar(sender);

                        messageControl.Dock = DockStyle.Top;

                        // Thêm MessageControl vào FlowLayoutPanel
                        chatContent.Controls.Add(messageControl);
                    }
                }
               
            }
           if(chatContent.Controls.Count>0)
            {
                chatContent.ScrollControlIntoView(chatContent.Controls[chatContent.Controls.Count - 1]);
            }    
        }

        // Phương thức kiểm tra xem tin nhắn có chứa đường dẫn đến ảnh hoặc video không
        private bool IsImageOrVideoMessage(string message)
        {
            // Thực hiện kiểm tra ở đây, ví dụ kiểm tra phần mở rộng của đường dẫn
            // Trong trường hợp này, giả sử message là đường dẫn đến tệp ảnh hoặc video
            // Bạn có thể thay đổi điều kiện kiểm tra tùy thuộc vào cách bạn lưu trữ tin nhắn
            if (message.EndsWith(".jpg") || message.EndsWith(".png") || message.EndsWith(".gif") ||
                message.EndsWith(".mp4") || message.EndsWith(".avi") || message.EndsWith(".mov"))
            {
                return true;
            }
            return false;
        }

        private string LoadUserAvatar(string currentUsername)
        {
            string[] lines = File.ReadAllLines("users.txt");

            foreach (string line in lines)
            {
                string[] userInfo = line.Split('|');
                string username = userInfo[2];

                if (username == currentUsername)
                {
                    return userInfo[4];
                }
            }
            return "";
        }

        private void LoadUserData(string username)
        {
            string filePath = "users.txt";
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] userInfo = line.Split('|');
                if (userInfo.Length >= 4 && userInfo[2] == username) // Chỗ này sửa lại theo cấu trúc file users.txt của bạn
                {
                    string name = userInfo[1]; // Lấy tên người dùng từ thông tin đã đọc
                    string avatarPath = userInfo[4]; // Lấy đường dẫn đến hình ảnh avatar từ thông tin đã đọc

                    // Hiển thị tên và avatar trong chatform
                    lblName.Text = name;
                    picAvatarUser.ImageLocation = avatarPath; // Load hình ảnh từ đường dẫn
                    break; // Thoát vòng lặp sau khi tìm thấy thông tin người dùng
                }
            }
        }

      
        

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picAvatarUser_Paint(object sender, PaintEventArgs e)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pictureBox1.Width - 1, pictureBox1.Height - 1);
            pictureBox1.Region = new Region(gp);
        }

        private void txtChat_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtChat_Enter(object sender, EventArgs e)
        {
            if(txtChat.Text ==PlaceholderText||txtChat.Text==PlaceholderTextVN)
            {
                txtChat.Text = "";
                txtChat.Focus();
                txtChat.ForeColor = SystemColors.WindowText; 
            }
        }

        private void txtChat_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChat.Text))
            {
                txtChat.Text = language == "EN" ? PlaceholderText : PlaceholderTextVN;
                txtChat.ForeColor = SystemColors.GrayText;
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            pnlEmotion.Visible =!pnlEmotion.Visible;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Lấy tin nhắn từ TextBox
            string message = txtChat.Text.Trim();

            // Kiểm tra xem tin nhắn có trống không
            if (!string.IsNullOrWhiteSpace(message) && message != PlaceholderText&&message!=PlaceholderTextVN)
            {
                // Tạo tên tập tin là "message.txt"
                string filePath = "message.txt";

                // Mở tập tin để ghi dữ liệu
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    // Ghi tin nhắn vào tập tin, cùng với thông tin của người gửi và người nhận (ví dụ: thời gian gửi)
                    writer.WriteLine($"{DateTime.Now}||{userLoggedIn}||{lblChatName.Text}||{message}");
                }

                // Xóa nội dung của TextBox sau khi gửi tin nhắn thành công
                txtChat.Text = "";
            }
            reloadMessage();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string emotion = "🙂"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
            
        }

        private void AddEmotion(string emotion)
        {
            if(txtChat.Text==PlaceholderText||txtChat.Text==PlaceholderTextVN)
            {
                txtChat.Text = "";
            }    
            txtChat.AppendText(emotion); // Thêm biểu tượng cảm xúc vào cuối tin nhắn
        }

        private void SaveMessageToFile(string message)
        {
            // Tạo tên tập tin là "message.txt"
            string filePath = "message.txt";

            // Mở tập tin để ghi dữ liệu
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // Ghi tin nhắn vào tập tin, cùng với thông tin của người gửi và người nhận (ví dụ: thời gian gửi)
                writer.WriteLine($"{DateTime.Now}||{userLoggedIn}||{lblChatName.Text}||{message}");
            }
        }

        private List<(string, string, string, string,string)> LoadMessages(string currentUser, string targetUser)
        {
            string filePath = "message.txt";
            List<(string, string, string, string,string)> userMessages = new List<(string, string, string, string,string)>();

            if (File.Exists(filePath))
            {
                // Đọc từng dòng trong tập tin
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    // Tách thông tin người gửi và tin nhắn
                    string[] parts = line.Split(new string[] { "||" }, StringSplitOptions.None);
                    if (parts.Length == 4)
                    {
                        string timeSent = parts[0];
                        string sender = parts[1];
                        string receiver = parts[2];
                        string message = parts[3];
             
                        // Xử lý tin nhắn ở đây
                        if (sender == currentUser && receiver == targetUser)
                        {
                            userMessages.Add((sender, receiver, message, timeSent,"sent"));
                        }
                        if(sender==targetUser && receiver == currentUser)
                        {
                            userMessages.Add((sender, receiver, message, timeSent, "receive"));
                        }
                    }
                }
            }

            return userMessages;
        }


        private void pictureBox8_Click(object sender, EventArgs e)
        {
            string emotion = "😟"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
           
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            string emotion = "😮"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
          
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            string emotion = "😢"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
          
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
           
            string emotion = "😋"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
          
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
           
             string emotion = "😝"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
          
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            string emotion = "😭"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
          
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            string emotion = "😉"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
          

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            string emotion = "😍"; // Đây là biểu tượng cảm xúc, bạn có thể thay đổi nó tùy ý
            AddEmotion(emotion);

            // Lưu tin nhắn vào tập tin
          
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại chọn file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif | Video files (*.mp4, *.avi, *.mov) | *.mp4; *.avi; *.mov";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true; // Cho phép chọn nhiều tệp tin

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bool hasVideo = false; // Biến kiểm tra đã chọn video hay chưa
                    bool hasImage = false; // Biến kiểm tra đã chọn ảnh hay chưa

                    // Duyệt qua từng tệp tin đã chọn
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        string fileType = Path.GetExtension(filePath).ToLower();

                        if (fileType == ".mp4" || fileType == ".avi" || fileType == ".mov")
                        {
                            // Kiểm tra nếu đã chọn video trước đó
                            if (hasVideo)
                            {
                                MessageBox.Show("Chỉ có thể gửi một video.");
                                return;
                            }

                            // Đánh dấu đã chọn video
                            hasVideo = true;

                            // Tin nhắn chứa video
                            SendVideoMessage(filePath);
                        }
                        else if (fileType == ".jpg" || fileType == ".jpeg" || fileType == ".png" || fileType == ".gif")
                        {
                            // Đánh dấu đã chọn ảnh
                            hasImage = true;

                            // Tin nhắn chứa ảnh
                            SendImageMessage(filePath);
                        }
                    }

                    // Kiểm tra nếu không có video nào được chọn
                    if (!hasVideo)
                    {
                 
                    }
                }
            }
        }



        private void SendImageMessage(string imagePath)
        {
            // Gửi tin nhắn với đường dẫn đến tệp tin ảnh
            string message = imagePath;

            // Lưu tin nhắn vào tệp tin
            SaveMessageToFile(message);
            reloadMessage();
        }

        private void SendVideoMessage(string videoPath)
        {
            // Gửi tin nhắn với đường dẫn đến tệp tin video
            string message = videoPath;

            // Lưu tin nhắn vào tệp tin
            SaveMessageToFile(message);
            reloadMessage();
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.Window;
            flowLayoutPanel1.BackColor = Color.LightCyan;
            panel3.BackColor = Color.LightCyan;
            appTheme = "";
           
            panel2.BackColor = Color.Cyan;
            panel5.BackColor = SystemColors.Window;
            pnlChat.BackColor = SystemColors.Window;
            // Thay đổi màu chữ của các điều khiển trong ứng dụng thành màu đen
            foreach (Control control in this.Controls)
            {
                control.ForeColor = Color.Black;
            }
            foreach (FriendListItem item in flowLayoutPanel1.Controls)
            {


                if (appTheme == "dark")
                {
                    item.BackColor = System.Drawing.ColorTranslator.FromHtml("#171717");
                }
                else
                {
                    item.BackColor = Color.LightCyan;
                }

            }
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackColor = System.Drawing.ColorTranslator.FromHtml("#212121");
            panel2.BackColor = Color.LightSteelBlue;
            flowLayoutPanel1.BackColor = System.Drawing.ColorTranslator.FromHtml("#171717");
            panel3.BackColor= System.Drawing.ColorTranslator.FromHtml("#171717");
            appTheme = "dark";

            panel5.BackColor= System.Drawing.ColorTranslator.FromHtml("#212121");
            pnlChat.BackColor= System.Drawing.ColorTranslator.FromHtml("#212121");
            // Thay đổi màu chữ của các điều khiển trong ứng dụng thành màu trắng
            foreach (Control control in this.Controls)
            {
                control.ForeColor = Color.White;
            }

            foreach (FriendListItem item in flowLayoutPanel1.Controls)
            {
               
                
                    if (appTheme == "dark")
                    {
                        item.BackColor = System.Drawing.ColorTranslator.FromHtml("#171717");
                    }
                    else
                    {
                        item.BackColor = Color.LightCyan;
                    }
                
            }

        }


        private void chatContent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void pnlChat_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
         //   userToolStripMenuItem.Text = "User";
           // profileToolStripMenuItem.Text = "Profile";
            label1.Text = "Online";
            lblWelcome.Text = "Choose friend to start conversation!!!!";
            label3.Text = "Online";
          //  settingToolStripMenuItem.Text = "Setting";
          //  languageToolStripMenuItem.Text = "Language";
          //  englishToolStripMenuItem.Text = "English";
           // vietnameseToolStripMenuItem.Text = "Vietnamese";
         //   themeToolStripMenuItem.Text = "Theme";
          //  blackToolStripMenuItem.Text = "Dark";
          //  whiteToolStripMenuItem.Text = "Light";
            language = "EN";
            LoadUsers(userLoggedIn);
            if (txtChat.Text == PlaceholderTextVN)
            {
                txtChat.Text = PlaceholderText;
            }
        }

        private void vietnameseToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // userToolStripMenuItem.Text = "Người dùng";
          //  profileToolStripMenuItem.Text = "Thông tin cá nhân";
            label1.Text = "Trực tuyến";
            lblWelcome.Text = "Chọn bạn để bắt đầu cuộc trò chuyện";
            label3.Text = "Trực tuyến";
           // settingToolStripMenuItem.Text = "Cài đặt";
            //languageToolStripMenuItem.Text = "Ngôn ngữ";
            //englishToolStripMenuItem.Text = "Tiếng Anh";
          //  vietnameseToolStripMenuItem.Text = "Tiếng Việt";
           // themeToolStripMenuItem.Text = "Chủ đề";
           // blackToolStripMenuItem.Text = "Tối";
            //whiteToolStripMenuItem.Text = "Sáng";
            language = "VN";
            LoadUsers(userLoggedIn);
            if(txtChat.Text==PlaceholderText)
            {
                txtChat.Text = PlaceholderTextVN;
            }    
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower(); // Lấy nội dung tìm kiếm và chuẩn hóa về chữ thường

            // Biến để kiểm tra xem có tìm thấy tin nhắn nào không
            bool found = false;

            // Duyệt qua tất cả các tin nhắn để tìm kiếm
            foreach (Control control in chatContent.Controls)
            {
                if (control is MessageSentControl messageControl)
                {
                    // Kiểm tra xem tin nhắn có chứa nội dung tìm kiếm không
                    if (messageControl.Message.ToLower().Contains(searchText))
                    {
                        // Tô đỏ tin nhắn
                        messageControl.BackColor = Color.Red;

                        // Di chuyển đến vị trí của tin nhắn tìm thấy
                        chatContent.ScrollControlIntoView(messageControl);

                        // Đánh dấu rằng đã tìm thấy ít nhất một tin nhắn
                        found = true;
                    }
                    else
                    {
                        // Nếu không tìm thấy, thì khôi phục màu nền gốc
                        messageControl.BackColor = Color.Transparent;
                    }
                }
                else if (control is MessageReceiveControl messageReceiveControl)
                {
                    // Kiểm tra xem tin nhắn nhận có chứa nội dung tìm kiếm không
                    if (messageReceiveControl.Message.ToLower().Contains(searchText))
                    {
                        // Tô đỏ tin nhắn
                        messageReceiveControl.BackColor = Color.Red;

                        // Di chuyển đến vị trí của tin nhắn tìm thấy
                        chatContent.ScrollControlIntoView(messageReceiveControl);

                        // Đánh dấu rằng đã tìm thấy ít nhất một tin nhắn
                        found = true;
                    }
                    else
                    {
                        // Nếu không tìm thấy, thì khôi phục màu nền gốc
                        messageReceiveControl.BackColor = Color.Transparent;
                    }
                }
            }

            // Kiểm tra xem có tin nhắn nào được tìm thấy hay không
            if (!found)
            {
                MessageBox.Show("Không tìm thấy tin nhắn nào chứa nội dung bạn nhập.");
            }
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
