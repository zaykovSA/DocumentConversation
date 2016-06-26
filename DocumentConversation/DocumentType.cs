namespace DocumentConversation
{
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }

        public DocumentType(int id, string name)
        {
            DocumentTypeId = id;
            DocumentTypeName = name;
        }

        public override string ToString()
        {
            return DocumentTypeName;
        }
    }
}
