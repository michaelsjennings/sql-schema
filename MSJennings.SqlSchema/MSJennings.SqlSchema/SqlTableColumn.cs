using System.Collections.Generic;
using System.Linq;

namespace MSJennings.SqlSchema
{
    public class SqlTableColumn : SqlColumnBase
    {
        public SqlTable Table
        {
            get => Parent as SqlTable;
            set => Parent = value;
        }

        public bool IsComputed { get; set; }

        public bool IsInPrimaryKey => Table.PrimaryKey?.KeyColumns.Any(x => x == this) ?? false;

        public bool IsInForeignKey => Table.ForeignKeys.Any(x => x.Columns.Any(y => y == this));

        public IEnumerable<SqlCheckConstraint> CheckConstraints => Table.Database.CheckConstraints.Where(x => x.Column == this);

        public IEnumerable<SqlDefaultConstraint> DefaultConstraints => Table.Database.DefaultConstraints.Where(x => x.Column == this);

        public SqlTableColumn(string name) : base(name)
        {
        }
    }
}
