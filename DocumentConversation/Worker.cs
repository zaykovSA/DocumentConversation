namespace DocumentConversation
{
    public class Worker
    {
        public int WorkerId { get; set; }
        public string WorkerFio { get; set; }
        public string WorkerPhone { get; set; }
        public string WorkerMail { get; set; }
        public string WorkerPost { get; set; }

        public Worker(int id, string fio, string phone, string mail, string post)
        {
            WorkerId = id;
            WorkerFio = fio;
            WorkerPhone = phone;
            WorkerMail = mail;
            WorkerPost = post;
        }
    }
}
