using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class TablesViewForm : Form
    {
        public User UserLogin;
        public string DbServer;
        public string DbUser;
        public string DbPass;
        public string DefStartFolder;

        private List<Client> _clientList;
        private List<Department> _departmentList;
        private List<Post> _postList;
        private List<Worker> _workerList;

        private bool _programExit;

        private Dictionary<int, int> _postDictionary;
        public TablesViewForm(string serv, string log, string pass, string folder)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
            DefStartFolder = folder;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            switch (comboBox1.SelectedItem.ToString())
            {
                case "Заказчики":
                    dataGridView1.Columns.Add("ClientId", "ID Клиента");
                    dataGridView1.Columns.Add("ClientName", "Имя Клиента");
                    dataGridView1.Columns.Add("ClientPhone", "Телефон Клиента");
                    dataGridView1.Columns.Add("ClientMail", "Электронный Адрес Клиента");
                    dataGridView1.Columns.Add("ClientAddress", "Адрес Клиента");

                    foreach (var client in _clientList)
                    {
                        dataGridView1.Rows.Add(client.ClientId, client.ClientName, client.ClientPhone, client.ClientMail, client.ClientAddress);
                    }
                    break;
                case "Отделы":
                    dataGridView1.Columns.Add("DepartmentId", "ID Отдела");
                    dataGridView1.Columns.Add("DepartmentName", "Название Отдела");
                    dataGridView1.Columns.Add("DepartmentAddress", "Адрес Отдела");

                    foreach (var department in _departmentList)
                    {
                        dataGridView1.Rows.Add(department.DepartmentId, department.DepartmentName, department.DepartmentAddress);
                    }
                    break;
                case "Работники":
                    dataGridView1.Columns.Add("WorkerId", "ID Работника");
                    dataGridView1.Columns.Add("WorkerFIO", "ФИО Работника");
                    dataGridView1.Columns.Add("WorkerPhone", "Телефон Работника");
                    dataGridView1.Columns.Add("WorkerMail", "Электронная Почта Работника");
                    dataGridView1.Columns.Add("WorkerPost", "Должность Работника");

                    foreach (var worker in _workerList)
                    {
                        dataGridView1.Rows.Add(
                            worker.WorkerId,
                            worker.WorkerFio,
                            worker.WorkerPhone,
                            worker.WorkerMail,
                            worker.WorkerPost);
                    }
                    break;
            }
        }

        private void TablesViewForm_Load(object sender, EventArgs e)
        {
            _clientList = new List<Client>();
            _departmentList = new List<Department>();
            _postList = new List<Post>();
            _workerList = new List<Worker>();

            _postDictionary = new Dictionary<int, int>();

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
                {
                    while (reader.Read())
                    {
                        _clientList.Add(new Client(
                            Convert.ToInt32(reader["ClientId"]),
                            reader["ClientName"].ToString(),
                            reader["ClientPhone"].ToString(),
                            reader["ClientMail"].ToString(),
                            reader["ClientAddress"].ToString()));
                    }
                }

                command = new SqlCommand("SELECT * FROM Departments", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _departmentList.Add(new Department(
                            Convert.ToInt32(reader["DepartmentId"]),
                            reader["DepartmentName"].ToString(),
                            reader["DepartmentAddress"].ToString()));
                    }
                }

                command = new SqlCommand("SELECT * FROM Posts", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _postList.Add(new Post(
                            Convert.ToInt32(reader["PostId"]),
                            reader["PostTitle"].ToString()));
                        _postDictionary.Add(Convert.ToInt32(reader["PostId"]), _postList.Count - 1);
                    }
                }

                command = new SqlCommand("SELECT * FROM Workers", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var postId = Convert.ToInt32(reader["WorkerPost"]);
                        var postIdInList = _postDictionary[postId];
                        var post = _postList[postIdInList];
                        _workerList.Add(new Worker(
                            Convert.ToInt32(reader["WorkerId"]),
                            reader["WorkerFIO"].ToString(),
                            reader["WorkerPhone"].ToString(),
                            reader["WorkerMail"].ToString(),
                            post.PostTitle));
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Owner.Show();
            _programExit = true;
            Close();
        }

        private void TablesViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programExit)
                Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
