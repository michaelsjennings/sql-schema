namespace MSJennings.SqlSchema
{
    public abstract class SqlField : SqlObject
    {
        public SqlDataType DataType { get; set; }

        protected SqlField(string name) : base(name)
        {
        }
    }
}
