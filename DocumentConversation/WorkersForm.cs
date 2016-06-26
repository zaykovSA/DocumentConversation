using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class WorkersForm : Form
    {
        private List<WorkerT> _dataBaseWorkers;
        private List<Post> _dataBasePosts;

        public string DbServer;
        public string DbUser;
        public string DbPass;

        private bool _programmaticallyExit;
        private List<string> _wokerFioList;

        public WorkersForm(string serv, string log, string pass)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
        }

        private void WorkersForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _programmaticallyExit = true;
            Owner.Show();
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            listBox1.Items.Clear();
            if (textBox1.Text == string.Empty)
                foreach (var worker in _dataBaseWorkers)
                    listBox1.Items.Add(worker);
            else
                foreach (
                    var worker in
                        _dataBaseWorkers.Where(worker => worker.ToString().ToLower().Contains(textBox1.Text.ToLower())))
                    listBox1.Items.Add(worker);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem as WorkerT;
            if (selected == null)
            {
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                textBox2.Text = selected.WorkerFio;
                textBox3.Text = selected.WorkerPhone;
                textBox5.Text = selected.WorkerMail;
                foreach (var item in comboBox1.Items.Cast<Post>().Where(item => item.PostId == selected.WorkerPost))
                {
                    comboBox1.SelectedItem = item;
                    break;
                }
            }
        }

        private void WorkersForm_Load(object sender, EventArgs e)
        {
            _dataBaseWorkers = new List<WorkerT>();
            _dataBasePosts = new List<Post>();
            _wokerFioList = new List<string>();
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Posts", conn);

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        _dataBasePosts.Add(new Post(
                            Convert.ToInt32(reader["PostId"]),
                            reader["PostTitle"].ToString()));

                command = new SqlCommand("SELECT * FROM Workers", conn);

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                    {
                        _dataBaseWorkers.Add(new WorkerT(
                            Convert.ToInt32(reader["WorkerId"]),
                            reader["WorkerFIO"].ToString(),
                            reader["WorkerPhone"].ToString(),
                            reader["WorkerMail"].ToString(),
                            Convert.ToInt32(reader["WorkerPost"])));
                        _wokerFioList.Add(reader["WorkerFIO"].ToString());
                    }
            }
            listBox1.Items.Clear();
            comboBox1.Items.Clear();
            foreach (var worker in _dataBaseWorkers)
                listBox1.Items.Add(worker);
            foreach (var post in _dataBasePosts)
                comboBox1.Items.Add(post);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox5.Text = string.Empty;
            comboBox1.SelectedIndex = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать работника для удаления!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as WorkerT;
            if (MessageBox.Show(
                string.Format(
                    "Вы действительно хотите удалить работника '{0}'?\rДля удаления необходимо убрать пользователей, относящиеся к удаляемому работнику!",
                    selected.WorkerFio),
                @"Подтверждение", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
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
                    var sqlCommand = new SqlCommand("Delete From Workers where WorkerId=" + selected.WorkerId, conn);
                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    _dataBaseWorkers.Remove(selected);
                    _wokerFioList.Remove(selected.WorkerFio);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox5.Text = "";
                    comboBox1.SelectedIndex = -1;
                    MessageBox.Show(string.Format("Удаление работника {0} завершено!", selected.WorkerFio),
                        @"Успешно", MessageBoxButtons.OK);
                }
                catch (SqlException er)
                {
                    MessageBox.Show(
                        string.Format(
                            "Во время удаления произошла ошибка базы данных: {0}. Обратитесь к администратору!",
                            er.Message), @"Ошибка", MessageBoxButtons.OK);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать работника для изменения!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as WorkerT;
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Имя работника должно быть задано!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (_wokerFioList.Contains(textBox2.Text) && selected.WorkerFio != textBox2.Text)
            {
                MessageBox.Show(@"Такие ФИО работника уже существуют!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Телефон работника должен быть задан!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show(@"Электронная почта работника должна быть задана!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Должность работника должна быть выбрана!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Изменить данные работника {0}",
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
                    var editTitle = textBox2.Text;
                    var editPhone = textBox3.Text;
                    var editMail = textBox5.Text;
                    var editPost = (comboBox1.SelectedItem as Post).PostId;

                    var editWorker = selected;
                    editWorker.WorkerFio = editTitle;
                    editWorker.WorkerPhone = editPhone;
                    editWorker.WorkerMail = editMail;
                    editWorker.WorkerPost = editPost;

                    conn.Open();
                    var sqlCommand =
                        new SqlCommand(
                            "Update Workers Set WorkerFIO = @0, WorkerPhone = @1, WorkerMail = @2, WorkerPost= @3 Where WorkerId = @4",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", editWorker.WorkerFio));
                    sqlCommand.Parameters.Add(new SqlParameter("1", editWorker.WorkerPhone));
                    sqlCommand.Parameters.Add(new SqlParameter("2", editWorker.WorkerMail));
                    sqlCommand.Parameters.Add(new SqlParameter("3", editWorker.WorkerPost));
                    sqlCommand.Parameters.Add(new SqlParameter("4", editWorker.WorkerId));

                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    listBox1.Items.Add(editWorker);
                    _wokerFioList.Remove(selected.WorkerFio);
                    _wokerFioList.Add(editWorker.WorkerFio);
                    _dataBaseWorkers.Remove(selected);
                    _dataBaseWorkers.Add(editWorker);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox5.Text = "";
                    comboBox1.SelectedIndex = -1;
                    MessageBox.Show(string.Format("Работник {0} успешно изменен!", editWorker.WorkerFio),
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Должно быть задано имя работника!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (_wokerFioList.Contains(textBox2.Text))
            {
                MessageBox.Show(@"Такие ФИО работника уже существуют!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Должен быть задан телефон работника!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show(@"Должна быть задана электронная почта работника!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Должна быть задана должность работника!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Следующий работник будет добавлен: \r Имя: {0} \r Телефон: {1} \r Электронная почта: {2} \r Должность: {3} ",
                    textBox2.Text, textBox3.Text, textBox5.Text, (comboBox1.SelectedItem as Post).PostTitle),
                @"Подтверждение", MessageBoxButtons.YesNo) !=
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
                    conn.Open();
                    var newName = textBox2.Text;
                    var newPhone = textBox3.Text;
                    var newMail = textBox5.Text;
                    var newPost = (comboBox1.SelectedItem as Post).PostId;

                    var newWorker = new WorkerT(0, newName, newPhone, newMail, newPost);
                    var sqlCommand =
                        new SqlCommand(
                            "Insert into Workers (WorkerFIO, WorkerPhone, WorkerMail, WorkerPost) values (@0,@1,@2,@3)",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", newName));
                    sqlCommand.Parameters.Add(new SqlParameter("1", newPhone));
                    sqlCommand.Parameters.Add(new SqlParameter("2", newMail));
                    sqlCommand.Parameters.Add(new SqlParameter("3", newPost));
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand =
                        new SqlCommand(
                            "SELECT * FROM Workers Where WorkerFIO='" + newName + "' and WorkerPhone='" + newPhone + "'",
                            conn);

                    using (var reader = sqlCommand.ExecuteReader())
                        while (reader.Read())
                            newWorker.WorkerId = Convert.ToInt32(reader["WorkerId"]);

                    listBox1.Items.Add(newWorker);
                    _dataBaseWorkers.Add(newWorker);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox5.Text = "";
                    comboBox1.SelectedIndex = -1;
                    MessageBox.Show(string.Format("Работник {0} успешно добавлен!", newWorker.WorkerFio),
                        @"Успешно", MessageBoxButtons.OK);
                }
                catch (SqlException er)
                {
                    MessageBox.Show(
                        string.Format(
                            "Во время добавления произошла ошибка базы данных: {0}. Обратитесь к администратору!",
                            er.Message), @"Ошибка", MessageBoxButtons.OK);
                }
            }
        }
    }
}