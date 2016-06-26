using System;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class UserForm : Form
    {
        public User UserLogin;
        public string DbServer;
        public string DbUser;
        public string DbPass;
        public string DefStartFolder;

        private bool _programmaticallyExit;

        public UserForm(string serv, string log, string pass, string folder)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
            DefStartFolder = folder;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            var addNewFileForm = new DocumentsUploadForm(DbServer, DbUser, DbPass, DefStartFolder) { UserLogin = UserLogin, Owner = this };
            addNewFileForm.Show(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            var documentsViewForm = new DocumentsViewForm(DbServer, DbUser, DbPass, DefStartFolder) { UserLogin = UserLogin, Owner = this };
            documentsViewForm.Show(this);
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            if (UserLogin != null)
                Text = @"Пользователь '" + UserLogin.UserLogin + "'";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hide();
            var tablesViewForm = new TablesViewForm(DbServer, DbUser, DbPass, DefStartFolder) { UserLogin = UserLogin, Owner = this };
            tablesViewForm.Show(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _programmaticallyExit = true;
            Owner.Show();
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }
    }
}
