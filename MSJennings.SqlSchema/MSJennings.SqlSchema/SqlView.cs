using System.Collections.Generic;

namespace MSJennings.SqlSchema
{
    public class SqlView : SqlTableViewBase
    {
        public new IList<SqlViewColumn> Columns { get; private set; }

        public SqlView(string name) : base(name)
        {
            Columns = new List<SqlViewColumn>();
        }
    }
}
