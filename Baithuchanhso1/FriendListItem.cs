using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Baithuchanhso1
{
    public partial class FriendListItem : UserControl
    {
        public FriendListItem(string username,string name, string avatarPath,string language = "")
        {
            InitializeComponent();
            Name = name;
            AvatarPath = avatarPath;
            UserName=username;
            if (language =="VN" )
            {
                label1.Text = "Trực tuyến";
            }
            else if(language =="EN")
            {
                label1.Text = "Online";
            }
        }

        private string username;

      public string UserName
        {
            get { return username; }
            set { username = value; }
        }

        public string Name
        {
            get { return lblUsername.Text; }
            set { lblUsername.Text = value; }
        }

        public string AvatarPath
        {
            get { return pictureBoxAvatar.ImageLocation; }
            set { pictureBoxAvatar.ImageLocation = value; }
        }
    }
}
