using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace MSJennings.SqlSchema
{
    public class SqlDatabase : SqlObject
    {
        public IList<SqlSchemaName> Schemas { get; private set; }

        public IList<SqlTable> Tables { get; private set; }

        public IList<SqlView> Views { get; private set; }

        public IList<SqlIndex> Indexes { get; private set; }

        public IList<SqlForeignKey> ForeignKeys { get; private set; }

        public IList<SqlCheckConstraint> CheckConstraints { get; private set; }

        public IList<SqlDefaultConstraint> DefaultConstraints { get; private set; }

        public IList<SqlTrigger> Triggers { get; private set; }

        public IList<SqlStoredProcedure> StoredProcedures { get; private set; }

        public IList<SqlFunction> Functions { get; private set; }

        public IEnumerable<SqlTableViewBase> TablesAndViews => Tables.Cast<SqlTableViewBase>().Union(Views.Cast<SqlTableViewBase>()).OrderBy(x => x.Name);

        public SqlDatabase() : this(string.Empty)
        {
        }

        public SqlDatabase(string name) : base(name)
        {
            Tables = new List<SqlTable>();
            Views = new List<SqlView>();
            Indexes = new List<SqlIndex>();
            ForeignKeys = new List<SqlForeignKey>();
            CheckConstraints = new List<SqlCheckConstraint>();
            DefaultConstraints = new List<SqlDefaultConstraint>();
            Triggers = new List<SqlTrigger>();
            StoredProcedures = new List<SqlStoredProcedure>();
            Functions = new List<SqlFunction>();
        }

        public void LoadFromMetadata(SqlMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            Name = metadata.DatabaseName;
            Schemas = new List<SqlSchemaName>();

            LoadTables(metadata);
            LoadViews(metadata);
            LoadIndexes(metadata);
            LoadForeignKeys(metadata);
            LoadCheckConstraints(metadata);
            LoadDefaultConstraints(metadata);
            LoadTriggers(metadata);
            LoadViews(metadata);
            LoadStoredProcedures(metadata);
            LoadFunctions(metadata);
        }

        private SqlSchemaName AddSchemaIfNotExists(string schemaName)
        {
            if (!Schemas.Any(x => x.Name.Equals(schemaName, StringComparison.OrdinalIgnoreCase)))
            {
                Schemas.Add(new SqlSchemaName(schemaName)
                {
                    Database = this
                });
            }

            return Schemas.Get(schemaName);
        }

        private void LoadTables(SqlMetadata metadata)
        {
            Tables = new List<SqlTable>();

            foreach (DataRow row in metadata.TablesMetadata.Rows)
            {
                var schemaName = row["table_schema_name"].ToString();
                var tableName = row["table_name"].ToString();

                var table = Tables.Get(schemaName, tableName);
                if (table == null)
                {
                    table = new SqlTable(tableName)
                    {
                        Database = this,
                        Schema = AddSchemaIfNotExists(schemaName)
                    };

                    Tables.Add(table);
                }

                var column = new SqlTableColumn(row["column_name"].ToString())
                {
                    Table = table,
                    DataType = new SqlDataType
                    {
                        SqlTypeName = row["type_name"].ToString().ToUpperInvariant(),
                        MaxLength = Convert.ToInt32(row["max_length"], CultureInfo.InvariantCulture),
                        Precision = Convert.ToInt32(row["precision"], CultureInfo.InvariantCulture),
                        Scale = Convert.ToInt32(row["scale"], CultureInfo.InvariantCulture)
                    },
                    IsNullable = (bool)row["is_nullable"],
                    IsIdentity = (bool)row["is_identity"],
                    IsComputed = (bool)row["is_computed"]
                };

                table.Columns.Add(column);
            }
        }

        private void LoadViews(SqlMetadata metadata)
        {
            Views = new List<SqlView>();

            foreach (DataRow row in metadata.ViewsMetadata.Rows)
            {
                var schemaName = row["view_schema_name"].ToString();
                var viewName = row["view_name"].ToString();

                var view = Views.Get(schemaName, viewName);
                if (view == null)
                {
                    view = new SqlView(viewName)
                    {
                        Database = this,
                        Schema = AddSchemaIfNotExists(schemaName)
                    };

                    Views.Add(view);
                }

                var column = new SqlViewColumn(row["column_name"].ToString())
                {
                    View = view,
                    DataType = new SqlDataType
                    {
                        SqlTypeName = row["type_name"].ToString().ToUpperInvariant(),
                        MaxLength = Convert.ToInt32(row["max_length"], CultureInfo.InvariantCulture),
                        Precision = Convert.ToInt32(row["precision"], CultureInfo.InvariantCulture),
                        Scale = Convert.ToInt32(row["scale"], CultureInfo.InvariantCulture)
                    },
                    IsNullable = (bool)row["is_nullable"],
                    IsIdentity = (bool)row["is_identity"]
                };

                view.Columns.Add(column);
            }
        }

        private void LoadIndexes(SqlMetadata metadata)
        {
            Indexes = new List<SqlIndex>();

            foreach (DataRow row in metadata.IndexesMetadata.Rows)
            {
                var schemaName = row["index_schema_name"].ToString();
                var tableName = row["index_table_name"].ToString();
                var indexName = row["index_name"].ToString();

                var table = Tables.Get(schemaName, tableName);
                var index = table.Indexes.Get(indexName);
                if (index == null)
                {
                    index = new SqlIndex(indexName)
                    {
                        Table = table,
                        IsClustered = (bool)row["is_clustered"],
                        IsUnique = (bool)row["is_unique"],
                        IsPrimaryKey = (bool)row["is_primary_key"],
                        IsDisabled = (bool)row["is_disabled"]
                    };

                    Indexes.Add(index);
                }

                var column = index.Table.Columns.Get(row["index_column_name"].ToString());
                var isIncluded = (bool)row["is_included_column"];
                if (isIncluded)
                {
                    index.IncludedColumns.Add(column);
                }
                else
                {
                    index.KeyColumns.Add(column);
                }
            }
        }

        private void LoadForeignKeys(SqlMetadata metadata)
        {
            ForeignKeys = new List<SqlForeignKey>();

            foreach (DataRow row in metadata.ForeignKeysMetadata.Rows)
            {
                var schemaName = row["foreign_key_schema_name"].ToString();
                var tableName = row["foreign_key_table_name"].ToString();
                var foreignKeyName = row["foreign_key_name"].ToString();

                var table = Tables.Get(schemaName, tableName);
                var foreignKey = table.ForeignKeys.Get(foreignKeyName);
                if (foreignKey == null)
                {
                    var primaryKeySchemaName = row["primary_key_schema_name"].ToString();
                    var primaryKeyTableName = row["primary_key_table_name"].ToString();
                    var primaryKeyTable = Tables.Get(primaryKeySchemaName, primaryKeyTableName);

                    foreignKey = new SqlForeignKey(foreignKeyName)
                    {
                        Table = table,
                        PrimaryKey = primaryKeyTable.PrimaryKey,
                        IsDisabled = (bool)row["is_disabled"]
                    };

                    ForeignKeys.Add(foreignKey);
                }

                var column = foreignKey.Table.Columns.Get(row["foreign_key_column_name"].ToString());
                foreignKey.Columns.Add(column);
            }
        }

        private void LoadCheckConstraints(SqlMetadata metadata)
        {
            CheckConstraints = new List<SqlCheckConstraint>();

            foreach (DataRow row in metadata.CheckConstraintsMetadata.Rows)
            {
                var schemaName = row["check_constraint_schema_name"].ToString();
                var tableName = row["check_constraint_table_name"].ToString();

                var table = Tables.Get(schemaName, tableName);
                var checkConstraint = new SqlCheckConstraint(row["check_constraint_name"].ToString())
                {
                    Column = table.Columns.Get(row["check_constraint_column_name"].ToString()),
                    Defintion = row["check_constraint_definition"].ToString()
                };

                CheckConstraints.Add(checkConstraint);
            }
        }

        private void LoadDefaultConstraints(SqlMetadata metadata)
        {
            DefaultConstraints = new List<SqlDefaultConstraint>();

            foreach (DataRow row in metadata.DefaultConstraintsMetadata.Rows)
            {
                var schemaName = row["default_constraint_schema_name"].ToString();
                var tableName = row["default_constraint_table_name"].ToString();

                var table = Tables.Get(schemaName, tableName);
                var defaultConstraint = new SqlDefaultConstraint(row["default_constraint_name"].ToString())
                {
                    Column = table.Columns.Get(row["default_constraint_column_name"].ToString()),
                    Defintion = row["default_constraint_definition"].ToString()
                };

                DefaultConstraints.Add(defaultConstraint);
            }
        }

        private void LoadTriggers(SqlMetadata metadata)
        {
            Triggers = new List<SqlTrigger>();

            foreach (DataRow row in metadata.TriggersMetadata.Rows)
            {
                var schemaName = row["trigger_schema_name"].ToString();
                var tableName = row["trigger_table_name"].ToString();

                var trigger = new SqlTrigger(row["trigger_name"].ToString())
                {
                    Table = Tables.Get(schemaName, tableName),
                    Definition = row["trigger_definition"].ToString(),
                    IsDisabled = (bool)row["is_disabled"]
                };

                Triggers.Add(trigger);
            }
        }

        private void LoadStoredProcedures(SqlMetadata metadata)
        {
            StoredProcedures = new List<SqlStoredProcedure>();

            foreach (DataRow row in metadata.StoredProceduresMetadata.Rows)
            {
                var storedProcedure = new SqlStoredProcedure(row["stored_procedure_name"].ToString())
                {
                    Schema = AddSchemaIfNotExists(row["stored_procedure_schema_name"].ToString()),
                    Database = this,
                    Definition = row["stored_procedure_definition"].ToString()
                };

                var parameterRows = metadata.StoredProcedureParametersMetadata.Select().AsEnumerable().Where(x =>
                    x["stored_procedure_schema_name"].ToString().Equals(storedProcedure.Schema.Name, StringComparison.OrdinalIgnoreCase) &&
                    x["stored_procedure_name"].ToString().Equals(storedProcedure.Name, StringComparison.OrdinalIgnoreCase));

                foreach (var parameterRow in parameterRows)
                {
                    var parameter = new SqlStoredProcedureParameter(parameterRow["parameter_name"].ToString())
                    {
                        StoredProcedure = storedProcedure,
                        DataType = new SqlDataType
                        {
                            SqlTypeName = parameterRow["type_name"].ToString(),
                            MaxLength = Convert.ToInt32(parameterRow["max_length"], CultureInfo.InvariantCulture),
                            Precision = Convert.ToInt32(parameterRow["precision"], CultureInfo.InvariantCulture),
                            Scale = Convert.ToInt32(parameterRow["scale"], CultureInfo.InvariantCulture)
                        },
                        DefaultValue = parameterRow["default_value"] != DBNull.Value ? parameterRow["default_value"].ToString() : null as string,
                        IsOutput = (bool)parameterRow["is_output"]
                    };

                    storedProcedure.Parameters.Add(parameter);
                }

                StoredProcedures.Add(storedProcedure);
            }
        }

        private void LoadFunctions(SqlMetadata metadata)
        {
            Functions = new List<SqlFunction>();

            foreach (DataRow row in metadata.FunctionsMetadata.Rows)
            {
                var function = new SqlFunction(row["function_name"].ToString())
                {
                    Schema = AddSchemaIfNotExists(row["function_schema_name"].ToString()),
                    Database = this,
                    Definition = row["function_definition"].ToString()
                };

                var parameterRows = metadata.FunctionParametersMetadata.Select().AsEnumerable().Where(x =>
                    x["function_schema_name"].ToString().Equals(function.Schema.Name, StringComparison.OrdinalIgnoreCase) &&
                    x["function_name"].ToString().Equals(function.Name, StringComparison.OrdinalIgnoreCase));

                foreach (var parameterRow in parameterRows)
                {
                    var parameter = new SqlFunctionParameter(parameterRow["parameter_name"].ToString())
                    {
                        Function = function,
                        DataType = new SqlDataType
                        {
                            SqlTypeName = parameterRow["type_name"].ToString(),
                            MaxLength = Convert.ToInt32(parameterRow["max_length"], CultureInfo.InvariantCulture),
                            Precision = Convert.ToInt32(parameterRow["precision"], CultureInfo.InvariantCulture),
                            Scale = Convert.ToInt32(parameterRow["scale"], CultureInfo.InvariantCulture)
                        },
                        DefaultValue = parameterRow["default_value"] != DBNull.Value ? parameterRow["default_value"].ToString() : null as string,
                        IsOutput = (bool)parameterRow["is_output"]
                    };

                    function.Parameters.Add(parameter);
                }

                Functions.Add(function);
            }
        }
    }
}
