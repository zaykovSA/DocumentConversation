namespace DocumentConversation
{
    public class User
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public int UserGroup { get; set; }
        public int? UserWorker { get; set; }

        public User(int id, string login, string pass, int group, int? worker)
        {
            UserId = id;
            UserLogin = login;
            UserPassword = pass;
            UserGroup = group;
            UserWorker = worker;
        }

        public override string ToString()
        {
            return UserLogin;
        }
    }
}
