namespace MSJennings.SqlSchema
{
    public class SqlFunctionParameter : SqlParameter
    {
        public SqlFunction Function { get; set; }

        public SqlFunctionParameter(string name) : base(name)
        {
        }
    }
}
