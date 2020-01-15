namespace MSJennings.SqlSchema
{
    public abstract class SqlColumnConstraint : SqlObject
    {
        public SqlTableColumn Column { get; set; }

        public string Defintion { get; set; }

        protected SqlColumnConstraint(string name) : base(name)
        {
        }
    }
}
