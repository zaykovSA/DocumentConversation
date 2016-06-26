namespace DocumentConversation
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientMail { get; set; }
        public string ClientAddress { get; set; }

        public Client(int id, string name, string phone, string mail, string address)
        {
            ClientId = id;
            ClientName = name;
            ClientPhone = phone;
            ClientMail = mail;
            ClientAddress = address;
        }

        public override string ToString()
        {
            return ClientName;
        }
    }
}
