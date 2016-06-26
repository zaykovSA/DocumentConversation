using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class DocTypesForm : Form
    {
        private List<DocumentType> _dataBaseDocTypes;
        public string DbServer;
        public string DbUser;
        public string DbPass;

        private bool _programmaticallyExit;
        private List<string> _existingTypes; 
        public DocTypesForm(string serv, string log, string pass)
        {
            InitializeComponent();
            DbServer = serv;
            DbUser = log;
            DbPass = pass;
        }

        private void DocTypesForm_Load(object sender, EventArgs e)
        {
            _existingTypes = new List<string>();
            _dataBaseDocTypes = new List<DocumentType>();
            using (
                var conn =
                    new SqlConnection(
                        string.Format(
                            "Data Source={0};Persist Security Info=True;User ID={1};Password={2};Initial Catalog=DocumentMainContent",
                            DbServer, DbUser, DbPass)))
            {
                conn.Open();
                var command = new SqlCommand("SELECT * FROM DocumentTypes", conn);

                using (var reader = command.ExecuteReader())
                    while (reader.Read())
                        _dataBaseDocTypes.Add(new DocumentType(
                            Convert.ToInt32(reader["DocumentTypeId"]),
                            reader["DocumentTypeName"].ToString()));
            }
            listBox1.Items.Clear();
            foreach (var documentType in _dataBaseDocTypes)
            {
                listBox1.Items.Add(documentType);
                _existingTypes.Add(documentType.DocumentTypeName);
            }
        }

        private void DocTypesForm_FormClosed(object sender, FormClosedEventArgs e)
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
                foreach (var user in _dataBaseDocTypes)
                    listBox1.Items.Add(user);
            else
                foreach (
                    var docType in
                        _dataBaseDocTypes.Where(docType => docType.ToString().ToLower().Contains(textBox1.Text.ToLower())))
                    listBox1.Items.Add(docType);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem as DocumentType;
            textBox2.Text = selected == null ? string.Empty : selected.DocumentTypeName;
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
                MessageBox.Show(@"Необходимо выбрать тип документа для удаления!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as DocumentType;
            if (MessageBox.Show(
                string.Format(
                    "Вы действительно хотите удалить тип документа '{0}'?\rДля удаления необходимо убрать документы, относящихся к удаляемому типу документа!",
                    selected.DocumentTypeName),
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
                    var sqlCommand = new SqlCommand("Delete From DocumentTypes where DocumentTypeId=" + selected.DocumentTypeId, conn);
                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    _dataBaseDocTypes.Remove(selected);
                    _existingTypes.Remove(selected.DocumentTypeName);
                    textBox1.Text = "";
                    textBox2.Text = "";
                    MessageBox.Show(string.Format("Удаление типа документа {0} завершено!", selected.DocumentTypeName),
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
                MessageBox.Show(@"Необходимо выбрать тип документа для изменения!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            var selected = listBox1.SelectedItem as DocumentType;
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(@"Название импа документа должно быть задано!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (_existingTypes.Contains(textBox2.Text))
            {
                MessageBox.Show(@"Тип документа уже существует!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Изменить данные типа документа {0}",
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

                    var editDocType = selected;
                    editDocType.DocumentTypeName = editTitle;

                    conn.Open();
                    var sqlCommand =
                        new SqlCommand(
                            "Update DocumentTypes Set DocumentTypeName= @0 Where DocumentTypeId = @1",
                            conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", editDocType.DocumentTypeName));
                    sqlCommand.Parameters.Add(new SqlParameter("1", editDocType.DocumentTypeId));

                    sqlCommand.ExecuteNonQuery();

                    listBox1.Items.Remove(selected);
                    listBox1.Items.Add(editDocType);
                    _dataBaseDocTypes.Remove(selected);
                    _dataBaseDocTypes.Add(editDocType);

                    _existingTypes.Remove(selected.DocumentTypeName);
                    _existingTypes.Add(editDocType.DocumentTypeName);

                    textBox1.Text = "";
                    textBox2.Text = "";

                    MessageBox.Show(string.Format("Тип документа {0} успешно изменен!", editDocType.DocumentTypeName),
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
                MessageBox.Show(@"Должно быть задано название типа документа!", @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (_existingTypes.Contains(textBox2.Text))
            {
                MessageBox.Show(string.Format("Тип документа {0} уже существует!", textBox2.Text), @"Ошибка", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show(
                string.Format(
                    "Следующий тип документа будет добавлен: \r Название: {0}",
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

                    var newDocType = new DocumentType(0, newName);
                    var sqlCommand =
                        new SqlCommand(
                            "Insert into DocumentTypes (DocumentTypeName) values (@0)", conn);
                    sqlCommand.Parameters.Add(new SqlParameter("0", newName));
                    sqlCommand.ExecuteNonQuery();

                    sqlCommand =
                        new SqlCommand("SELECT * FROM DocumentTypes Where DocumentTypeName='" + newName + "'",
                            conn);

                    using (var reader = sqlCommand.ExecuteReader())
                        while (reader.Read())
                            newDocType.DocumentTypeId = Convert.ToInt32(reader["DocumentTypeId"]);

                    listBox1.Items.Add(newDocType);
                    _dataBaseDocTypes.Add(newDocType);
                    _existingTypes.Add(newDocType.DocumentTypeName);
                    textBox1.Text = "";
                    textBox2.Text = "";

                    MessageBox.Show(string.Format("Тип документа {0} успешно добавлен!", newDocType.DocumentTypeName),
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
