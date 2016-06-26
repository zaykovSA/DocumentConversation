using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class DepartmentForm : Form
    {
        private List<Department> _dataBaseDepartments;
        public string DbServer;
        public string DbUser;
        public string DbPass;

        private bool _programmaticallyExit;
        public DepartmentForm(string serv, string log, string pass)
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
                foreach (var user in _dataBaseDepartments)
                    listBox1.Items.Add(user);
            else
                foreach (
                    var department in
                        _dataBaseDepartments.Where(department => department.ToString().ToLower().Contains(textBox1.Text.ToLower())))
                    listBox1.Items.Add(department);
        }

        private void DepartmentForm_Load(object sender, EventArgs e)
        {
            _dataBaseDepartments = new List<Department>();
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Departments", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _dataBaseDepartments.Add(new Department(
                            Convert.ToInt32(reader["DepartmentId"]),
                            reader["DepartmentName"].ToString(),
                            reader["DepartmentAddress"].ToString()));
                    }
                }
            }
            listBox1.Items.Clear();
            foreach (var department in _dataBaseDepartments)
            {
                listBox1.Items.Add(department);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem as Department;
            if (selected == null)
            {
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
            }
            else
            {
                textBox2.Text = selected.DepartmentName;
                textBox3.Text = selected.DepartmentAddress;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать отдел для удаления!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as Department;
            if (MessageBox.Show(
                string.Format(
                    "Вы действительно хотите удалить отдел '{0}'?\rДля удаления необходимо убрать документы, относящиеся к удаляемому отделу!",
                    selected.DepartmentName),
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
                    var sqlCommand = new SqlCommand("Delete From Departments where DepartmentId=" + selected.DepartmentId, conn);
                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    _dataBaseDepartments.Remove(selected);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    MessageBox.Show(string.Format("Удаление отдела {0} завершено!", selected.DepartmentName),
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
                MessageBox.Show(@"Необходимо выбрать отдел для изменения!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as Department;
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Название отдела должно быть задано!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Адрес отдела должен быть задан!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Изменить данные отдела {0}",
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
                    var editAddress = textBox3.Text;

                    var editDepartment = selected;
                    editDepartment.DepartmentName = editTitle;
                    editDepartment.DepartmentAddress = editAddress;

                    conn.Open();
                    var sqlCommand =
                        new SqlCommand(
                            "Update Departments Set DepartmentName = @0, DepartmentAddress = @1 Where DepartmentId = @2",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", editDepartment.DepartmentName));
                    sqlCommand.Parameters.Add(new SqlParameter("1", editDepartment.DepartmentAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("2", editDepartment.DepartmentId));

                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    listBox1.Items.Add(editDepartment);
                    _dataBaseDepartments.Remove(selected);
                    _dataBaseDepartments.Add(editDepartment);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";

                    MessageBox.Show(string.Format("Отдел {0} успешно изменен!", editDepartment.DepartmentName),
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
                MessageBox.Show(@"Должно быть задано название отдела!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Должен быть задан адрес отдела!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Следующий отдел будет добавлен: \r Название: {0} \r Адрес: {1}",
                    textBox2.Text, textBox3.Text), @"Подтверждение", MessageBoxButtons.YesNo) !=
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
                    var newAddress = textBox3.Text;

                    var newDepartment = new Department(0, newName, newAddress);
                    var sqlCommand =
                        new SqlCommand(
                            "Insert into Departments (DepartmentName, DepartmentAddress) values (@0,@1)", conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", newName));
                    sqlCommand.Parameters.Add(new SqlParameter("1", newAddress));
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand =
                        new SqlCommand("SELECT * FROM Departments Where DepartmentName='" + newName + "' and DepartmentAddress='" + newAddress + "'",
                            conn);

                    using (var reader = sqlCommand.ExecuteReader())
                        while (reader.Read())
                            newDepartment.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);

                    listBox1.Items.Add(newDepartment);
                    _dataBaseDepartments.Add(newDepartment);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";

                    MessageBox.Show(string.Format("Отдел {0} по адресу {1} успешно добавлен!", newDepartment.DepartmentName, newDepartment.DepartmentAddress),
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

        private void DepartmentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }
    }
}
