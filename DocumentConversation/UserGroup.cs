namespace DocumentConversation
{
    public class UserGroup
    {
        public int GroupId { get; set; }
        public string GroupTitle { get; set; }

        public UserGroup(int id, string title)
        {
            GroupId = id;
            GroupTitle = title;
        }

        public override string ToString()
        {
            return GroupTitle;
        }
    }
}
