using System;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class AdministratorForm : Form
    {
        public User UserLogin;
        public string DbServer;
        public string DbUser;
        public string DbPass;
        public string DefStartFolder;

        private bool _programmaticallyExit;
        public AdministratorForm(string serv, string log, string pass, string folder)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
            DefStartFolder = folder;
        }

        private void AdministratorForm_Load(object sender, EventArgs e)
        {
            if (UserLogin != null)
                Text = @"Администратор '" + UserLogin.UserLogin + @"'";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            var userConfigForm = new UsersConfigForm(DbServer, DbUser, DbPass) { Owner = this };
            userConfigForm.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _programmaticallyExit = true;
            Owner.Show();
            Close();
        }

        private void AdministratorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Hide();
            var depForm = new DepartmentForm(DbServer, DbUser, DbPass) { Owner = this };
            depForm.Show(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Hide();
            var postForm = new PostsForm(DbServer, DbUser, DbPass) { Owner = this };
            postForm.Show(this);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Hide();
            var clientsForm = new ClientsForm(DbServer, DbUser, DbPass) { Owner = this };
            clientsForm.Show(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Hide();
            var docTypesForm = new DocTypesForm(DbServer, DbUser, DbPass) { Owner = this };
            docTypesForm.Show(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hide();
            var workersForm = new WorkersForm(DbServer, DbUser, DbPass) { Owner = this };
            workersForm.Show(this);
        }
    }
}
