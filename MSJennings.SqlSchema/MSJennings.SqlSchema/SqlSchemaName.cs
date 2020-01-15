using System.Collections.Generic;
using System.Linq;

namespace MSJennings.SqlSchema
{
    public class SqlSchemaName : SqlObject
    {
        public SqlDatabase Database { get; set; }

        public IEnumerable<SqlTable> Tables => Database.Tables.Where(x => x.Schema == this);

        public IEnumerable<SqlView> Views => Database.Views.Where(x => x.Schema == this);

        public IEnumerable<SqlTableViewBase> TablesAndViews => Database.TablesAndViews.Where(x => x.Schema == this);

        public IEnumerable<SqlStoredProcedure> StoredProcedures => Database.StoredProcedures.Where(x => x.Schema == this);

        public IEnumerable<SqlFunction> Functions => Database.Functions.Where(x => x.Schema == this);

        public SqlSchemaName(string name) : base(name)
        {
        }
    }
}
