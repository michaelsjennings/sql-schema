using System.Collections.Generic;
using System.Linq;

namespace MSJennings.SqlSchema
{
    public class SqlTable : SqlTableViewBase
    {
        public new IList<SqlTableColumn> Columns { get; private set; }

        public SqlIndex PrimaryKey => Indexes.SingleOrDefault(x => x.IsPrimaryKey);

        public IEnumerable<SqlIndex> Indexes => Database.Indexes.Where(x => x.Table == this);

        public IEnumerable<SqlForeignKey> ForeignKeys => Database.ForeignKeys.Where(x => x.Table == this);

        public IEnumerable<SqlTrigger> Triggers => Database.Triggers.Where(x => x.Table == this);

        public SqlTable(string name) : base(name)
        {
            Columns = new List<SqlTableColumn>();
        }
    }
}
