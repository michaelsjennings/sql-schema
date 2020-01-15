namespace MSJennings.SqlSchema
{
    public class SqlStoredProcedureParameter : SqlParameter
    {
        public SqlStoredProcedure StoredProcedure { get; set; }

        public SqlStoredProcedureParameter(string name) : base(name)
        {
        }
    }
}
