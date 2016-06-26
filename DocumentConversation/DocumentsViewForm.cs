using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class DocumentsViewForm : Form
    {
        public User UserLogin;
        public string DbServer;
        public string DbUser;
        public string DbPass;
        public string DefStartFolder;

        private List<DocumentCard> _dbDocuments;
        private List<User> _dbUsers;
        private List<Department> _dbDepartments;
        private List<Client> _dbClients;
        private List<DocumentType> _dbDocTypes;

        private bool _programmaticallyExit;
        public DocumentsViewForm(string serv, string log, string pass, string folder)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
            DefStartFolder = folder;
        }

        private void DocumentsViewForm_Load(object sender, EventArgs e)
        {
            _dbDocuments = new List<DocumentCard>();
            _dbUsers = new List<User>();
            _dbClients = new List<Client>();
            _dbDocTypes = new List<DocumentType>();
            _dbDepartments = new List<Department>();

            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM Documents", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var newDocument = new DocumentCard(
                            Convert.ToInt32(reader["DocumentId"]),
                            reader["DocumentTitle"].ToString(),
                            reader["DocumentNumber"].ToString(),
                            DateTime.Parse(reader["DocumentDate"].ToString()),
                            reader["DocumentDescription"].ToString(),
                            Convert.ToInt32(reader["DocumentUploader"]),
                            Convert.ToInt32(reader["DocumentDepartment"]),
                            Convert.ToInt32(reader["DocumentClient"]),
                            Convert.ToInt32(reader["DocumentType"]),
                            reader["DocumentPath"].ToString());
                        listBox1.Items.Add(newDocument);
                        _dbDocuments.Add(newDocument);
                    }
                }

                command = new SqlCommand("SELECT * FROM Users", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User(
                            Convert.ToInt32(reader["UserID"]),
                            reader["UserLogin"].ToString(),
                            reader["UserPassword"].ToString(),
                            Convert.ToInt32(reader["UserGroup"]),
                            Convert.ToInt32(reader["UserWorker"]));
                        _dbUsers.Add(user);
                    }
                }

                command = new SqlCommand("SELECT * FROM Clients", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var client = new Client(
                            Convert.ToInt32(reader["ClientId"]),
                            reader["ClientName"].ToString(),
                            reader["ClientPhone"].ToString(),
                            reader["ClientMail"].ToString(),
                            reader["ClientAddress"].ToString());
                        _dbClients.Add(client);
                    }
                }
                command = new SqlCommand("SELECT * FROM Departments", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var department = new Department(
                            Convert.ToInt32(reader["DepartmentId"]),
                            reader["DepartmentName"].ToString(),
                            reader["DepartmentAddress"].ToString());
                        _dbDepartments.Add(department);
                    }
                }
                command = new SqlCommand("SELECT * FROM DocumentTypes", conn);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var docType = new DocumentType(
                            Convert.ToInt32(reader["DocumentTypeId"]),
                            reader["DocumentTypeName"].ToString());
                        _dbDocTypes.Add(docType);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            listBox1.Items.Clear();
            if (textBox1.Text == string.Empty)
                foreach (var user in _dbDocuments)
                    listBox1.Items.Add(user);
            else
                foreach (
                    var document in
                        _dbDocuments.Where(document => document.ToString().ToLower().Contains(textBox1.Text.ToLower())))
                    listBox1.Items.Add(document);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
                textBox4.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox6.Text = string.Empty;
                textBox7.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox9.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                return;
            }
            var selected = listBox1.SelectedItem as DocumentCard;
            textBox2.Text = selected.DocumentId.ToString();
            textBox3.Text = selected.DocumentTitle;
            textBox4.Text = selected.DocumentNumber;
            textBox5.Text = selected.DocumentDate.ToString(CultureInfo.CurrentCulture);
            textBox7.Text = selected.DocumentPath;
            textBox10.Text = selected.DocumentDescription;

            var selectedUserId = selected.DocumentUploader;
            foreach (var user in _dbUsers.Where(user => user.UserId == selectedUserId))
            {
                textBox6.Text = user.UserLogin;
                break;
            }

            var selectedDepartmentId = selected.DocumentDepartment;
            foreach (var department in _dbDepartments.Where(department => department.DepartmentId == selectedDepartmentId))
            {
                textBox8.Text = department.DepartmentName;
                break;
            }

            var selectedClientId = selected.DocumentClient;
            foreach (var client in _dbClients.Where(client => client.ClientId == selectedClientId))
            {
                textBox9.Text = client.ClientName;
                break;
            }

            var selectedTypeId = selected.DocumentType;
            foreach (var docType in _dbDocTypes.Where(docType => docType.DocumentTypeId == selectedTypeId))
            {
                textBox11.Text = docType.DocumentTypeName;
                break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _programmaticallyExit = true;
            Owner.Show();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DocumentsViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var filterForm = new FilterForm()
            {
                DbUsers = _dbUsers,
                DbClients = _dbClients,
                DbDocTypes = _dbDocTypes,
                DbDepartments = _dbDepartments,
                DbDocs = _dbDocuments
            };
            filterForm.Show(this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            listBox1.ClearSelected();
            listBox1.Items.Clear();
            foreach (var user in _dbDocuments)
                listBox1.Items.Add(user);
        }
    }
}
