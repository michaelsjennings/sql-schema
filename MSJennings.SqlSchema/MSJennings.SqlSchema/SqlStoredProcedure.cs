using System.Collections.Generic;

namespace MSJennings.SqlSchema
{
    public class SqlStoredProcedure : SqlObject
    {
        public SqlDatabase Database { get; set; }

        public SqlSchemaName Schema { get; set; }

        public IList<SqlStoredProcedureParameter> Parameters { get; private set; }

        public string Definition { get; set; }

        public SqlStoredProcedure(string name) : base(name)
        {
            Parameters = new List<SqlStoredProcedureParameter>();
        }
    }
}
