using System.Collections.Generic;

namespace MSJennings.SqlSchema
{
    public class SqlIndex : SqlObject
    {
        public SqlTable Table { get; set; }

        public IList<SqlTableColumn> KeyColumns { get; private set; }

        public IList<SqlTableColumn> IncludedColumns { get; private set; }

        public bool IsClustered { get; set; }

        public bool IsUnique { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsDisabled { get; set; }

        public SqlIndex(string name) : base(name)
        {
            KeyColumns = new List<SqlTableColumn>();
            IncludedColumns = new List<SqlTableColumn>();
        }
    }
}
