using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DocumentConversation
{
    public partial class FilterForm : Form
    {
        public List<User> DbUsers;
        public List<Client> DbClients;
        public List<DocumentType> DbDocTypes;
        public List<Department> DbDepartments;
        public List<DocumentCard> DbDocs;

        public FilterForm()
        {
            InitializeComponent();
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            docUploaderCB.Items.Clear();
            foreach (var user in DbUsers)
                docUploaderCB.Items.Add(user);

            docDepCB.Items.Clear();
            foreach (var dep in DbDepartments)
                docDepCB.Items.Add(dep);

            docClientCB.Items.Clear();
            foreach (var client in DbClients)
                docClientCB.Items.Add(client);

            docTypeCB.Items.Clear();
            foreach (var type in DbDocTypes)
                docTypeCB.Items.Add(type);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var parent = Owner as DocumentsViewForm;
            var filterTB = parent.Controls.Find("textBox1", true)[0] as TextBox;
            filterTB.Text = string.Empty;
            var listBox = parent.Controls.Find("listBox1", true)[0] as ListBox;
            listBox.ClearSelected();
            listBox.Items.Clear();
            foreach (var doc in DbDocs)
            {
                var filterName = docNameTB.Text;
                var filterNumber = docNumberTB.Text;
                var filterUploader = new User(1, "", "", 1, 1);
                if (docUploaderCB.SelectedIndex != -1)
                    filterUploader = docUploaderCB.SelectedItem as User;
                var filterDepartment = new Department(1, "", "");
                if (docDepCB.SelectedIndex != -1)
                    filterDepartment = docDepCB.SelectedItem as Department;
                var filterClient = new Client(1, "", "", "", "");
                if (docClientCB.SelectedIndex != -1)
                    filterClient = docClientCB.SelectedItem as Client;
                var filterDocType = new DocumentType(1, "");
                if (docTypeCB.SelectedIndex != -1)
                    filterDocType = docTypeCB.SelectedItem as DocumentType;
                var startDate = new DateTime();
                var endDate = new DateTime();
                if (UseDateCheckBox.Checked)
                {
                    startDate = dateStart.Value;
                    startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);

                    endDate = dateEnd.Value;
                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                }
                if (!string.IsNullOrEmpty(filterName) && !doc.DocumentTitle.Contains(filterName))
                    continue;
                if (!string.IsNullOrEmpty(filterNumber) && !doc.DocumentNumber.Contains(filterNumber))
                    continue;
                if (UseDateCheckBox.Checked && (doc.DocumentDate < startDate || doc.DocumentDate > endDate))
                    continue;
                if (docUploaderCB.SelectedIndex != -1 && doc.DocumentUploader != filterUploader.UserId)
                    continue;
                if (docDepCB.SelectedIndex != -1 && doc.DocumentDepartment != filterDepartment.DepartmentId)
                    continue;
                if (docClientCB.SelectedIndex != -1 && doc.DocumentClient != filterClient.ClientId)
                    continue;
                if (docTypeCB.SelectedIndex != -1 && doc.DocumentType != filterDocType.DocumentTypeId)
                    continue;
                listBox.Items.Add(doc);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
