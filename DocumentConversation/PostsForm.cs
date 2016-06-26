using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class PostsForm : Form
    {
        private List<Post> _dataBasePosts;
        public string DbServer;
        public string DbUser;
        public string DbPass;

        private bool _programmaticallyExit;
        private List<string> _existingPosts; 
        public PostsForm(string serv, string log, string pass)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
        }

        private void PostsForm_Load(object sender, EventArgs e)
        {
            _dataBasePosts = new List<Post>();
            _existingPosts = new List<string>();
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
                    {
                        _dataBasePosts.Add(new Post(
                            Convert.ToInt32(reader["PostId"]),
                            reader["PostTitle"].ToString()));
                        _existingPosts.Add(reader["PostTitle"].ToString());
                    }
            }
            listBox1.Items.Clear();
            foreach (var post in _dataBasePosts)
                listBox1.Items.Add(post);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            listBox1.Items.Clear();
            if (textBox1.Text == string.Empty)
                foreach (var user in _dataBasePosts)
                    listBox1.Items.Add(user);
            else
                foreach (
                    var post in
                        _dataBasePosts.Where(post => post.ToString().ToLower().Contains(textBox1.Text.ToLower())))
                    listBox1.Items.Add(post);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem as Post;
            textBox2.Text = selected == null ? string.Empty : selected.PostTitle;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _programmaticallyExit = true;
            Owner.Show();
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PostsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            listBox1.ClearSelected();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать должность для удаления!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as Post;
            if (MessageBox.Show(
                string.Format(
                    "Вы действительно хотите удалить должность '{0}'?\rДля удаления необходимо убрать работников, относящихся к удаляемой должности!",
                    selected.PostTitle),
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
                    var sqlCommand = new SqlCommand("Delete From Posts where PostId=" + selected.PostId, conn);
                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    _dataBasePosts.Remove(selected);
                    _existingPosts.Remove(selected.PostTitle);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    MessageBox.Show(string.Format("Удаление должности {0} завершено!", selected.PostTitle),
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
                MessageBox.Show(@"Необходимо выбрать должность для изменения!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as Post;
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Название должности должно быть задано!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (_existingPosts.Contains(textBox2.Text))
            {
                MessageBox.Show(@"Должность уже существует!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Изменить данные должности {0}",
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

                    var editPost = selected;
                    editPost.PostTitle = editTitle;

                    conn.Open();
                    var sqlCommand =
                        new SqlCommand(
                            "Update Posts Set PostTitle = @0 Where PostId = @1",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", editPost.PostTitle));
                    sqlCommand.Parameters.Add(new SqlParameter("1", editPost.PostId));

                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    listBox1.Items.Add(editPost);
                    _dataBasePosts.Remove(selected);
                    _dataBasePosts.Add(editPost);

                    _existingPosts.Remove(selected.PostTitle);
                    _existingPosts.Add(editPost.PostTitle);

                    textBox1.Text = "";
                    textBox2.Text = "";

                    MessageBox.Show(string.Format("Должность {0} успешно изменена!", editPost.PostTitle),
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
                MessageBox.Show(@"Должно быть задано название должности!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (_existingPosts.Contains(textBox2.Text))
            {
                MessageBox.Show(string.Format("Должность {0} уже существует!", textBox2.Text), @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Следующая должность будет добавлен: \r Название: {0}",
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
                    conn.Open();
                    var newName = textBox2.Text;

                    var newPost = new Post(0, newName);
                    var sqlCommand =
                        new SqlCommand(
                            "Insert into Posts (PostTitle) values (@0)", conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", newName));
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand =
                        new SqlCommand("SELECT * FROM Posts Where PostTitle='" + newName + "'",
                            conn);

                    using (var reader = sqlCommand.ExecuteReader())
                        while (reader.Read())
                            newPost.PostId = Convert.ToInt32(reader["PostId"]);

                    listBox1.Items.Add(newPost);
                    _dataBasePosts.Add(newPost);
                    _existingPosts.Add(newPost.PostTitle);
                    textBox1.Text = "";
                    textBox2.Text = "";

                    MessageBox.Show(string.Format("Должность {0} успешно добавлена!", newPost.PostTitle),
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
