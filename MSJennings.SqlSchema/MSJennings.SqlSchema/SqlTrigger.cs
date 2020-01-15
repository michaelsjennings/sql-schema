namespace MSJennings.SqlSchema
{
    public class SqlTrigger : SqlObject
    {
        public SqlTable Table { get; set; }

        public string Definition { get; set; }

        public bool IsDisabled { get; set; }

        public SqlTrigger(string name) : base(name)
        {
        }
    }
}
