using System.Collections.Generic;

namespace MSJennings.SqlSchema
{
    public class SqlFunction : SqlObject
    {
        public SqlDatabase Database { get; set; }

        public SqlSchemaName Schema { get; set; }

        public IList<SqlFunctionParameter> Parameters { get; private set; }

        public string Definition { get; set; }

        public SqlDataType ResultType { get; set; }

        public SqlFunction(string name) : base(name)
        {
            Parameters = new List<SqlFunctionParameter>();
        }
    }
}
