using System.Collections.Generic;
using System.Linq;

namespace MSJennings.SqlSchema
{
    public abstract class SqlTableViewBase : SqlObject
    {
        public SqlDatabase Database { get; set; }

        public SqlSchemaName Schema { get; set; }

        public IEnumerable<SqlColumnBase> Columns =>
            (this as SqlTable)?.Columns.Cast<SqlColumnBase>() ??
            (this as SqlView)?.Columns.Cast<SqlColumnBase>();

        protected SqlTableViewBase(string name) : base(name)
        {
        }
    }
}
