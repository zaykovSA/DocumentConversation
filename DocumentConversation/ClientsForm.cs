using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class ClientsForm : Form
    {
        private List<Client> _dataBaseClients;
        public string DbServer;
        public string DbUser;
        public string DbPass;

        private bool _programmaticallyExit;

        public ClientsForm(string serv, string log, string pass)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
        }

        private void ClientsForm_Load(object sender, EventArgs e)
        {
            _dataBaseClients = new List<Client>();
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM Clients", conn);

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        _dataBaseClients.Add(new Client(
                            Convert.ToInt32(reader["ClientId"]),
                            reader["ClientName"].ToString(),
                            reader["ClientPhone"].ToString(),
                            reader["ClientMail"].ToString(),
                            reader["ClientAddress"].ToString()));
            }
            listBox1.Items.Clear();
            foreach (var client in _dataBaseClients)
                listBox1.Items.Add(client);
        }

        private void ClientsForm_FormClosed(object sender, FormClosedEventArgs e)
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
                foreach (var client in _dataBaseClients)
                    listBox1.Items.Add(client);
            else
                foreach (
                    var client in
                        _dataBaseClients.Where(client => client.ToString().ToLower().Contains(textBox1.Text.ToLower())))
                    listBox1.Items.Add(client);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem as Client;
            if (selected == null)
            {
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                textBox5.Text = string.Empty;
            }
            else
            {
                textBox2.Text = selected.ClientName;
                textBox3.Text = selected.ClientPhone;
                textBox4.Text = selected.ClientAddress;
                textBox5.Text = selected.ClientMail;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать заказчика для удаления!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as Client;
            if (selected == null)
            {
                MessageBox.Show(
                    @"Не удалось получить выбранного заказчика, попробуйте немного позже.",
                    @"Подтверждение", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Вы действительно хотите удалить клиента '{0}'?\rДля удаления необходимо убрать документы, относящиеся к удаляемому клиенту!",
                    selected.ClientName),
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
                    var sqlCommand = new SqlCommand("Delete From Clients where ClientId=" + selected.ClientId, conn);
                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    _dataBaseClients.Remove(selected);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    MessageBox.Show(string.Format("Удаление клиента {0} завершено!", selected.ClientName),
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
                MessageBox.Show(@"Необходимо выбрать заказчика для изменения!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as Client;
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Имя заказчика должно быть задано!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Телефон заказчика должен быть задан!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show(@"Электронная почта заказчика должна быть задана!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show(@"Адрес заказчика должен быть задан!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Изменить данные заказчика {0}",
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
                    var editAddress = textBox4.Text;

                    var editClient = selected;
                    if (editClient == null)
                    {
                        MessageBox.Show(
                            @"Не удалось получить выбранного заказчика, попробуйте немного позже.",
                            @"Подтверждение", MessageBoxButtons.OK);
                        return;
                    }
                    editClient.ClientName = editTitle;
                    editClient.ClientPhone = editPhone;
                    editClient.ClientMail = editMail;
                    editClient.ClientAddress = editAddress;

                    conn.Open();
                    var sqlCommand =
                        new SqlCommand(
                            "Update Clients Set ClientName = @0, ClientPhone = @1, ClientMail = @2, ClientAddress= @3 Where DepartmentId = @4",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", editClient.ClientName));
                    sqlCommand.Parameters.Add(new SqlParameter("1", editClient.ClientPhone));
                    sqlCommand.Parameters.Add(new SqlParameter("2", editClient.ClientMail));
                    sqlCommand.Parameters.Add(new SqlParameter("3", editClient.ClientAddress));
                    sqlCommand.Parameters.Add(new SqlParameter("4", editClient.ClientId));

                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    listBox1.Items.Add(editClient);
                    _dataBaseClients.Remove(selected);
                    _dataBaseClients.Add(editClient);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    MessageBox.Show(string.Format("Заказчик {0} успешно изменен!", editClient.ClientName),
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
                MessageBox.Show(@"Должно быть задано имя заказчика!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Должен быть задан телефон заказчика!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show(@"Должна быть задана электронная почта заказчика!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show(@"Должен быть задан адрес заказчика!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Следующий заказчик будет добавлен: \r Имя: {0} \r Телефон: {1} \r Электронная почта: {2} \r Адрес: {3} ",
                    textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text), @"Подтверждение",
                MessageBoxButtons.YesNo) !=
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
                    var newAddress = textBox4.Text;

                    var newClient = new Client(0, newName, newPhone, newMail, newAddress);
                    var sqlCommand =
                        new SqlCommand(
                            "Insert into Clients (ClientName, ClientPhone, ClientMail, ClientAddress) values (@0,@1,@2,@3)",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", newName));
                    sqlCommand.Parameters.Add(new SqlParameter("1", newPhone));
                    sqlCommand.Parameters.Add(new SqlParameter("2", newMail));
                    sqlCommand.Parameters.Add(new SqlParameter("3", newAddress));
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand =
                        new SqlCommand(
                            "SELECT * FROM Clients Where ClientName='" + newName + "' and ClientAddress='" + newAddress +
                            "'",
                            conn);

                    using (var reader = sqlCommand.ExecuteReader())
                        while (reader.Read())
                            newClient.ClientId = Convert.ToInt32(reader["ClientId"]);

                    listBox1.Items.Add(newClient);
                    _dataBaseClients.Add(newClient);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    MessageBox.Show(
                        string.Format("Заказчик {0} по адресу {1} успешно добавлен!", newClient.ClientName,
                            newClient.ClientAddress),
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