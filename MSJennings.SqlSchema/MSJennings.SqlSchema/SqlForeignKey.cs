using System.Collections.Generic;

namespace MSJennings.SqlSchema
{
    public class SqlForeignKey : SqlObject
    {
        public SqlTable Table { get; set; }

        public IList<SqlTableColumn> Columns { get; private set; }

        public SqlIndex PrimaryKey { get; set; }

        public bool IsDisabled { get; set; }

        public SqlForeignKey(string name) : base(name)
        {
            Columns = new List<SqlTableColumn>();
        }
    }
}
