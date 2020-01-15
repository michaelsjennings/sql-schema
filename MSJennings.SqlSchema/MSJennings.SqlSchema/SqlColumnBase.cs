namespace MSJennings.SqlSchema
{
    public abstract class SqlColumnBase : SqlField
    {
        public SqlTableViewBase Parent { get; set; }

        public bool IsNullable { get; set; }

        public bool IsIdentity { get; set; }

        protected SqlColumnBase(string name) : base(name)
        {
        }
    }
}
