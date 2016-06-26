namespace DocumentConversation
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }

        public Department(int id, string name, string address)
        {
            DepartmentId = id;
            DepartmentName = name;
            DepartmentAddress = address;
        }

        public override string ToString()
        {
            return DepartmentName;
        }
    }
}
