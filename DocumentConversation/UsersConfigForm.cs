using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class UsersConfigForm : Form
    {
        private List<User> _dataBaseUsers;
        private List<UserGroup> _dataBaseGroups;
        private List<WorkerT> _dataBaseWorkers;
        public string DbServer;
        public string DbUser;
        public string DbPass;

        private bool _programmaticallyExit;

        public UsersConfigForm(string serv, string log, string pass)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            listBox1.Items.Clear();
            if (textBox1.Text == string.Empty)
                foreach (var user in _dataBaseUsers)
                    listBox1.Items.Add(user);
            else
                foreach (
                    var user in
                        _dataBaseUsers.Where(user => user.ToString().ToLower().Contains(textBox1.Text.ToLower())))
                    listBox1.Items.Add(user);
        }

        private void UsersConfigForm_Load(object sender, EventArgs e)
        {
            _dataBaseUsers = new List<User>();
            _dataBaseGroups = new List<UserGroup>();
            _dataBaseWorkers = new List<WorkerT>();
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Users", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _dataBaseUsers.Add(new User(
                            Convert.ToInt32(reader["UserID"]),
                            reader["UserLogin"].ToString(),
                            reader["UserPassword"].ToString(),
                            Convert.ToInt32(reader["UserGroup"]),
                            Convert.ToInt32(reader["UserWorker"])));
                    }
                }

                command = new SqlCommand("SELECT * FROM UserRoles", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _dataBaseGroups.Add(new UserGroup(
                            Convert.ToInt32(reader["UserRoleId"]),
                            reader["UserRoleTitle"].ToString()));
                    }
                }

                command = new SqlCommand("SELECT * FROM Workers", conn);

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        _dataBaseWorkers.Add(new WorkerT(
                            Convert.ToInt32(reader["WorkerId"]),
                            reader["WorkerFIO"].ToString(),
                            reader["WorkerPhone"].ToString(),
                            reader["WorkerMail"].ToString(),
                            Convert.ToInt32(reader["WorkerPost"])
                            ));
            }

            listBox1.Items.Clear();
            foreach (var user in _dataBaseUsers)
                listBox1.Items.Add(user);

            comboBox1.Items.Clear();
            foreach (var group in _dataBaseGroups)
                comboBox1.Items.Add(group);
            foreach (var worker in _dataBaseWorkers)
                comboBox2.Items.Add(worker);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem as User;
            if (selected == null)
            {
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
            }
            else
            {
                textBox2.Text = selected.UserLogin;
                textBox3.Text = selected.UserPassword;
                foreach (UserGroup item in comboBox1.Items)
                {
                    if (item.GroupId != selected.UserGroup) continue;
                    comboBox1.SelectedItem = item;
                    break;
                }
                foreach (WorkerT item in comboBox2.Items)
                {
                    if (item.WorkerId != selected.UserWorker) continue;
                    comboBox2.SelectedItem = item;
                    break;
                }
            }
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox3.PasswordChar = '\0';
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox3.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!(listBox1.SelectedIndex > -1))
            {
                MessageBox.Show(@"Необходимо выбрать пользователя для удаления!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selectedUser = listBox1.SelectedItem as User;
            if (selectedUser.UserLogin == "admin")
            {
                MessageBox.Show(@"Пользователь admin является системной записью, удаление невозможно!", @"Ошибка",
                    MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format("Вы действительно хотите удалить пользователя '{0}'?", selectedUser.UserLogin),
                @"Ошибка", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                try
                {
                    conn.Open();
                    var sqlCommand = new SqlCommand("Delete From Users where UserID=" + selectedUser.UserId, conn);
                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selectedUser);
                    _dataBaseUsers.Remove(selectedUser);
                    textBox2.Text = "";
                    textBox3.Text = "";
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;

                    MessageBox.Show(string.Format("Удаление пользователя {0} завершено!", selectedUser.UserLogin),
                        "Успешно", MessageBoxButtons.OK);
                }
                catch (SqlException er)
                {
                    MessageBox.Show(
                        string.Format(
                            "Во время удаления произошла ошибка базы данных: {0}. Обратитесь к администратору!",
                            er.Message), "Ошибка", MessageBoxButtons.OK);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Новому пользователю должен быть присвоен логин!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Новому пользователю должен быть присвоен пароль!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Новый пользователь должен быть отнесен в группу!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            foreach (var user in _dataBaseUsers)
            {
                if (textBox2.Text != user.UserLogin) continue;
                MessageBox.Show(
                    string.Format(
                        "Пользователь с таким логином уже существует: {0}. Обратитесь к администратору!",
                        user.UserLogin), @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Следующий пользователь будет добавлен: \r Логин: {0} \r Пароль: {1} \r Группа: {2}",
                    textBox2.Text, textBox3.Text, comboBox1.SelectedItem), @"Подтверждение", MessageBoxButtons.YesNo) !=
                DialogResult.Yes) return;
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                try
                {
                    var newUserLogin = textBox2.Text;
                    var newUserPassword = textBox3.Text;
                    var newUserGroup = comboBox1.SelectedItem as UserGroup;
                    WorkerT newUserWorker = null;
                    if (comboBox2.SelectedIndex != -1)
                        newUserWorker = comboBox2.SelectedItem as WorkerT;

                    var newUser = newUserWorker != null
                        ? new User(0, newUserLogin, newUserPassword, newUserGroup.GroupId, newUserWorker.WorkerId)
                        : new User(0, newUserLogin, newUserPassword, newUserGroup.GroupId, null);
                    conn.Open();
                    var sqlCommand =
                        new SqlCommand(
                            "Insert into Users (UserLogin,UserPassword,UserGroup,UserWorker) values (@0,@1,@2,@3)", conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", newUserLogin));
                    sqlCommand.Parameters.Add(new SqlParameter("1", newUserPassword));
                    sqlCommand.Parameters.Add(new SqlParameter("2", newUserGroup.GroupId));
                    sqlCommand.Parameters.Add(new SqlParameter("3", newUser.UserWorker));
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand =
                        new SqlCommand("SELECT * FROM Users Where convert(varchar, UserLogin)='" + newUserLogin + "'",
                            conn);

                    using (var reader = sqlCommand.ExecuteReader())
                        while (reader.Read())
                            newUser.UserId = Convert.ToInt32(reader["UserID"]);

                    listBox1.Items.Add(newUser);
                    _dataBaseUsers.Add(newUser);
                    textBox2.Text = "";
                    textBox3.Text = "";
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;

                    MessageBox.Show(string.Format("Пользователь {0} успешно добавлен!", newUser.UserLogin),
                        @"Успешно", MessageBoxButtons.OK);
                }
                catch (SqlException er)
                {
                    MessageBox.Show(
                        string.Format(
                            "Во время добавлени произошла ошибка базы данных: {0}. Обратитесь к администратору!",
                            er.Message), @"Ошибка", MessageBoxButtons.OK);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Пользователю должен быть присвоен логин!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Пользователю должен быть присвоен пароль!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Пользователь должен быть отнесен в группу!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selectedUser = listBox1.SelectedItem as User;
            if (textBox2.Text != selectedUser.UserLogin)
                foreach (var user in _dataBaseUsers)
                {
                    if (textBox2.Text != user.UserLogin) continue;
                    MessageBox.Show(
                        string.Format(
                            "Пользователь с таким логином уже существует: {0}. Обратитесь к администратору!",
                            user.UserLogin), @"Ошибка", MessageBoxButtons.OK);
                    return;
                }
            if (MessageBox.Show(
                string.Format(
                    "Изменить данные пользователя {0}",
                    textBox2.Text), @"Подтверждение", MessageBoxButtons.YesNo) !=
                DialogResult.Yes) return;
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                try
                {
                    var editUserLogin = textBox2.Text;
                    var editUserPassword = textBox3.Text;
                    var editUserGroup = comboBox1.SelectedItem as UserGroup;
                    WorkerT editUserWorker = null;
                    if (comboBox2.SelectedIndex != -1)
                        editUserWorker = comboBox2.SelectedItem as WorkerT;

                    var editUser = selectedUser;
                    editUser.UserLogin = editUserLogin;
                    editUser.UserPassword = editUserPassword;
                    editUser.UserGroup = editUserGroup.GroupId;

                    if (editUserWorker != null) editUser.UserWorker = editUserWorker.WorkerId;

                    conn.Open();
                    var sqlCommand =
                        new SqlCommand(
                            "Update Users Set UserLogin = @0, UserPassword = @1, UserGroup = @2, UserWorker = @3 Where UserID = @4",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", editUser.UserLogin));
                    sqlCommand.Parameters.Add(new SqlParameter("1", editUser.UserPassword));
                    sqlCommand.Parameters.Add(new SqlParameter("2", editUser.UserGroup));
                    sqlCommand.Parameters.Add(editUserWorker != null
                        ? new SqlParameter("3", editUser.UserWorker)
                        : new SqlParameter("3", null));
                    sqlCommand.Parameters.Add(new SqlParameter("4", editUser.UserId));

                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selectedUser);
                    listBox1.Items.Add(editUser);
                    _dataBaseUsers.Remove(selectedUser);
                    _dataBaseUsers.Add(editUser);
                    textBox2.Text = "";
                    textBox3.Text = "";
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;

                    MessageBox.Show(string.Format("Пользователь {0} успешно изменен!", editUser.UserLogin),
                        @"Успешно", MessageBoxButtons.OK);
                }
                catch (SqlException er)
                {
                    MessageBox.Show(
                        string.Format(
                            "Во время изменения произошла ошибка базы данных: {0}. Обратитесь к администратору!",
                            er.Message), @"Ошибка", MessageBoxButtons.OK);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _programmaticallyExit = true;
            Owner.Show();
            Close();
        }

        private void UsersConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }
    }
}
