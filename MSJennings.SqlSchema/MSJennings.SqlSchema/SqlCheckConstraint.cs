namespace MSJennings.SqlSchema
{
    public class SqlCheckConstraint : SqlColumnConstraint
    {
        public bool IsDisabled { get; set; }

        public SqlCheckConstraint(string name) : base(name)
        {
        }
    }
}
