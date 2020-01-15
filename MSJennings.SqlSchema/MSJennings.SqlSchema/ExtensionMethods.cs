using System;
using System.Collections.Generic;
using System.Linq;

namespace MSJennings.SqlSchema
{
    public static class ExtensionMethods
    {
        public static SqlSchemaName Get(this IEnumerable<SqlSchemaName> schemas, string schemaName)
        {
            return schemas.SingleOrDefault(x => x.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase));
        }

        public static SqlTable Get(this IEnumerable<SqlTable> tables, string schemaName, string tableName)
        {
            return tables.SingleOrDefault(x =>
                x.Schema.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase) &&
                x.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));
        }

        public static SqlView Get(this IEnumerable<SqlView> views, string schemaName, string tableName)
        {
            return views.SingleOrDefault(x =>
                x.Schema.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase) &&
                x.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));
        }

        public static SqlTableViewBase Get(this IEnumerable<SqlTableViewBase> tablesAndViews, string schemaName, string tableName)
        {
            return tablesAndViews.SingleOrDefault(x =>
                x.Schema.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase) &&
                x.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));
        }

        public static SqlIndex Get(this IEnumerable<SqlIndex> indexes, string indexName)
        {
            return indexes.SingleOrDefault(x =>
                x.Name.Equals(indexName, StringComparison.OrdinalIgnoreCase));
        }

        public static SqlForeignKey Get(this IEnumerable<SqlForeignKey> foreignKeys, string foreignKeyName)
        {
            return foreignKeys.SingleOrDefault(x =>
                x.Name.Equals(foreignKeyName, StringComparison.OrdinalIgnoreCase));
        }

        public static SqlTableColumn Get(this IEnumerable<SqlTableColumn> columns, string columnName)
        {
            return columns.SingleOrDefault(x =>
                x.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }

        public static SqlViewColumn Get(this IEnumerable<SqlViewColumn> columns, string columnName)
        {
            return columns.SingleOrDefault(x =>
                x.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }
        public static SqlColumnBase Get(this IEnumerable<SqlColumnBase> columns, string columnName)
        {
            return columns.SingleOrDefault(x =>
                x.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
