namespace DocumentConversation
{
    public class WorkerT
    {
        public int WorkerId { get; set; }
        public string WorkerFio { get; set; }
        public string WorkerPhone { get; set; }
        public string WorkerMail { get; set; }
        public int WorkerPost { get; set; }

        public WorkerT(int id, string fio, string phone, string mail, int post)
        {
            WorkerId = id;
            WorkerFio = fio;
            WorkerPhone = phone;
            WorkerMail = mail;
            WorkerPost = post;
        }

        public override string ToString()
        {
            return WorkerFio;
        }
    }
}
