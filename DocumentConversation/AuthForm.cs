using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class AuthForm : Form
    {
        private string _serverName;
        private string _accessLogin;
        private string _accessPass;
        private string _defStartFolder;

        public AuthForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var configPath = string.Format(@"{0}\cnf.config", Application.StartupPath);
            _serverName = string.Empty;
            _accessLogin = string.Empty;
            _accessPass = string.Empty;
            _defStartFolder = string.Empty;
            using (var sreader =
                new StreamReader(configPath, Encoding.Default))
            {
                var configuration = sreader.ReadToEnd();
                var splited = configuration.Split(new[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);
                _serverName = splited[0];
                _accessLogin = splited[1];
                _accessPass = splited[2];
                _defStartFolder = splited[3];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Логин не может быть пустым!", @"Ошибка", MessageBoxButtons.OK);
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show(@"Пароль не может быть пустым!", @"Ошибка", MessageBoxButtons.OK);
                textBox1.Text = string.Empty;
                textBox2.Text = string.Empty;
                return;
            }

            using (var conn = new SqlConnection(string.Format("Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent", _serverName, _accessLogin, _accessPass)))
            {
                var userFound = false;
                var passwordCorrect = false;
                var selectedUserLogin = textBox2.Text;
                var selectedUserPassword = textBox1.Text;
                var userGroupId = -1;
                var userGroup = string.Empty;
                User selectedUser = null;
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Users WHERE convert(varchar, UserLogin) = @0", conn);
                command.Parameters.Add(new SqlParameter("0", selectedUserLogin.ToLower()));
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userFound = true;
                        if (reader["UserPassword"].ToString() != selectedUserPassword) continue;
                        passwordCorrect = true;
                        userGroupId = Convert.ToInt32(reader["UserGroup"]);
                        selectedUser = new User(Convert.ToInt32(reader["UserID"]), reader["UserLogin"].ToString(), reader["UserPassword"].ToString(), userGroupId, Convert.ToInt32(reader["UserWorker"]));
                    }
                }
                if (!userFound)
                {
                    MessageBox.Show(@"Пользователь с таким логином не найден!", @"Ошибка", MessageBoxButtons.OK);
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    return;
                }
                if (!passwordCorrect)
                {
                    MessageBox.Show(@"Пароль введен неверно!", @"Ошибка", MessageBoxButtons.OK);
                    textBox1.Text = string.Empty;
                    textBox2.Text = string.Empty;
                    return;
                }
                command = new SqlCommand("SELECT * FROM UserRoles WHERE UserRoleId = @0", conn);
                command.Parameters.Add(new SqlParameter("0", userGroupId));
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        userGroup = reader["UserRoleTitle"].ToString();
                if (userGroup == "Администратор")
                {
                    Hide();
                    var administratorForm = new AdministratorForm(_serverName, _accessLogin, _accessPass, _defStartFolder) { UserLogin = selectedUser, Owner = this };
                    administratorForm.Show(this);
                }
                else
                {
                    Hide();
                    var userForm = new UserForm(_serverName, _accessLogin, _accessPass, _defStartFolder) { UserLogin = selectedUser, Owner = this };
                    userForm.Show(this);
                }
            }
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
