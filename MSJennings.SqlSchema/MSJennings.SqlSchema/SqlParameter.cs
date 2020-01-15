namespace MSJennings.SqlSchema
{
    public abstract class SqlParameter : SqlField
    {
        public string DefaultValue { get; set; }

        public bool IsOutput { get; set; }

        protected SqlParameter(string name) : base(name)
        {
        }
    }
}
