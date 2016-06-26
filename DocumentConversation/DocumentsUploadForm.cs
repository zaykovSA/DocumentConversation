using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class DocumentsUploadForm : Form
    {
        public User UserLogin;
        public string DbServer;
        public string DbUser;
        public string DbPass;
        public string DefStartFolder;

        private bool _programmaticallyExit;
        public DocumentsUploadForm(string serv, string log, string pass, string folder)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
            DefStartFolder = folder;
        }

        private void DocumentsUploadForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            using (var conn = new SqlConnection(string.Format("Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent", DbServer, DbUser, DbPass)))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM DocumentTypes", conn);

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        comboBox1.Items.Add(
                            new DocumentType(
                                Convert.ToInt32(reader["DocumentTypeId"].ToString()),
                                reader["DocumentTypeName"].ToString()));
                if (comboBox1.Items.Count == 0)
                {
                    MessageBox.Show(
                        @"В базе нет ни одного типа документа! Обратитесь к администратору!",
                        @"Ошибка",
                        MessageBoxButtons.OK);
                    button1.Enabled = false;
                    button2.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    return;
                }

                command = new SqlCommand("SELECT * FROM Departments", conn);
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        comboBox2.Items.Add(
                            new Department(
                                Convert.ToInt32(reader["DepartmentId"].ToString()),
                                reader["DepartmentName"].ToString(),
                                reader["DepartmentAddress"].ToString()));
                if (comboBox2.Items.Count == 0)
                {
                    MessageBox.Show(
                        @"В базе нет ни одного отдела! Обратитесь к администратору!",
                        @"Ошибка",
                        MessageBoxButtons.OK);
                    button1.Enabled = false;
                    button2.Enabled = false;
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    return;
                }

                command = new SqlCommand("SELECT * FROM Clients", conn);
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        comboBox3.Items.Add(
                            new Client(
                                Convert.ToInt32(reader["ClientId"].ToString()),
                                reader["ClientName"].ToString(),
                                reader["ClientPhone"].ToString(),
                                reader["ClientMail"].ToString(),
                                reader["ClientAddress"].ToString()));
                if (comboBox3.Items.Count != 0) return;
                MessageBox.Show(
                    @"В базе нет ни одного заказчика! Обратитесь к администратору!",
                    @"Ошибка",
                    MessageBoxButtons.OK);
                button1.Enabled = false;
                button2.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                dateTimePicker1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowReadOnly = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show(@"Необходимо выбрать файл для загрузки!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Необходимо задать название документа!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show(@"Необходимо задать номер документа!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать тип документа!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать отдел, из которого направлен документ!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show(@"Необходимо выбрать заказчика!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show(@"Выбранный файл перемещен или удален! Проверьте его местоположение!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }

            var pathFormLogic = string.Empty;
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();
                var command = new SqlCommand("Select * From Configuration Where ConfigParameterTitle = @0", conn);
                command.Parameters.AddWithValue("0", "PathFormLogic");
                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        pathFormLogic = reader["ConfigParameterValue"].ToString();
            }
            if (string.IsNullOrEmpty(pathFormLogic))
            {
                MessageBox.Show(@"В базе данных не настроена логика формирования пути!Обратитесь к администратору!", @"Ошибка", MessageBoxButtons.OK);
                button1.Enabled = false;
                button2.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                dateTimePicker1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                return;
            }
            var splitedPathLogic = pathFormLogic.Split(new[] {@"\\"}, StringSplitOptions.RemoveEmptyEntries);
            
            var selectedDepartment = comboBox2.SelectedItem as Department;
            var selectedClient = comboBox3.SelectedItem as Client;
            var selectedType = comboBox1.SelectedItem as DocumentType;

            var docTitle = textBox2.Text;
            var docNumber = textBox3.Text;
            var docDate = dateTimePicker1.Value;
            var docDescription = textBox4.Text;
            var docUploader = UserLogin.UserId;
            var docDepartment = selectedDepartment.DepartmentId;
            var docClient = selectedClient.ClientId;
            var docType = selectedType.DocumentTypeId;
            var docPath = string.Empty;
            switch (splitedPathLogic[0])
            {
                case "Type":
                    docPath = selectedType.DocumentTypeName;
                    break;
                case "Department":
                    docPath = selectedDepartment.DepartmentName;
                    break;
                case "User":
                    docPath = UserLogin.UserLogin;
                    break;
                case "Client":
                    docPath = selectedClient.ClientName;
                    break;
            }
            switch (splitedPathLogic[1])
            {
                case "Type":
                    docPath += @"\" + selectedType.DocumentTypeName;
                    break;
                case "Department":
                    docPath += @"\" + selectedDepartment.DepartmentName;
                    break;
                case "User":
                    docPath += @"\" + UserLogin.UserLogin;
                    break;
                case "Client":
                    docPath += @"\" + selectedClient.ClientName;
                    break;
            }
            switch (splitedPathLogic[2])
            {
                case "Type":
                    docPath += @"\" + selectedType.DocumentTypeName;
                    break;
                case "Department":
                    docPath += @"\" + selectedDepartment.DepartmentName;
                    break;
                case "User":
                    docPath += @"\" + UserLogin.UserLogin;
                    break;
                case "Client":
                    docPath += @"\" + selectedClient.ClientName;
                    break;
            }
            switch (splitedPathLogic[3])
            {
                case "Type":
                    docPath += @"\" + selectedType.DocumentTypeName;
                    break;
                case "Department":
                    docPath += @"\" + selectedDepartment.DepartmentName;
                    break;
                case "User":
                    docPath += @"\" + UserLogin.UserLogin;
                    break;
                case "Client":
                    docPath += @"\" + selectedClient.ClientName;
                    break;
            }
            var splitedFilePath = textBox1.Text.Split(new[] {@"\"}, StringSplitOptions.RemoveEmptyEntries);
            var fileName = splitedFilePath[splitedFilePath.Length - 1];
            var fileExtension = fileName.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries)[1];

            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();
                var command = new SqlCommand(
                    "Insert Into Documents " +
                    "(DocumentTitle, DocumentNumber, DocumentDate, DocumentDescription, DocumentUploader, DocumentDepartment, DocumentClient, DocumentType, DocumentPath) " +
                    "values " +
                    "(@0,@1,@2,@3,@4,@5,@6,@7,@8)", conn);
                command.Parameters.AddWithValue("0", docTitle);
                command.Parameters.AddWithValue("1", docNumber);
                command.Parameters.AddWithValue("2", docDate);
                command.Parameters.AddWithValue("3", docDescription);
                command.Parameters.AddWithValue("4", docUploader);
                command.Parameters.AddWithValue("5", docDepartment);
                command.Parameters.AddWithValue("6", docClient);
                command.Parameters.AddWithValue("7", docType);
                command.Parameters.AddWithValue("8", docPath + @"\" + docTitle + "." + fileExtension);

                command.ExecuteNonQuery();
            }

            var destFolder = Path.Combine(DefStartFolder, docPath);
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            File.Move(textBox1.Text, destFolder + @"\" + docTitle + "." + fileExtension);

            MessageBox.Show(string.Format("Карточка документа была заполнена!\rФайл был перемещен по пути:{0}", destFolder), @"Успешно", MessageBoxButtons.OK);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _programmaticallyExit = true;
            Owner.Show();
            Close();
        }

        private void DocumentsUploadForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_programmaticallyExit)
                Application.Exit();
        }
    }
}
