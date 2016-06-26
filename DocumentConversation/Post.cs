namespace DocumentConversation
{
    public class Post
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }

        public Post(int id, string title)
        {
            PostId = id;
            PostTitle = title;
        }

        public override string ToString()
        {
            return PostTitle;
        }
    }
}
