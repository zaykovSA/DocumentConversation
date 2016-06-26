using System;

namespace DocumentConversation
{
    public class DocumentCard
    {
        public int DocumentId { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentDescription { get; set; }
        public int DocumentUploader { get; set; }
        public int DocumentDepartment { get; set; }
        public int DocumentClient { get; set; }
        public int DocumentType { get; set; }
        public string DocumentPath { get; set; }

        public DocumentCard(int id, string name, string number, DateTime date, string descr, int uploader, int department, int client, int type, string path)
        {
            DocumentId = id;
            DocumentTitle = name;
            DocumentNumber = number;
            DocumentDate = date;
            DocumentDescription = descr;
            DocumentUploader = uploader;
            DocumentDepartment = department;
            DocumentClient = client;
            DocumentType = type;
            DocumentPath = path;
        }

        public override string ToString()
        {
            return DocumentNumber;
        }
    }
}
