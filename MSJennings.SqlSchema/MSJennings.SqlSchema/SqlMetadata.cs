using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MSJennings.SqlSchema
{
    public class SqlMetadata
    {
        private DataSet _metadata;

        private enum MetadataTables
        {
            Tables = 0,
            Indexes = 1,
            ForeignKeys = 2,
            CheckConstraints = 3,
            DefaultConstraints = 4,
            Triggers = 5,
            Views = 6,
            StoredProcedures = 7,
            StoredProcedureParameters = 8,
            Functions = 9,
            FunctionParameters = 10
        }

        public string DatabaseName => _metadata?.DataSetName;

        public DataTable TablesMetadata => _metadata?.Tables[(int)MetadataTables.Tables];

        public DataTable IndexesMetadata => _metadata?.Tables[(int)MetadataTables.Indexes];

        public DataTable ForeignKeysMetadata => _metadata?.Tables[(int)MetadataTables.ForeignKeys];

        public DataTable CheckConstraintsMetadata => _metadata?.Tables[(int)MetadataTables.CheckConstraints];

        public DataTable DefaultConstraintsMetadata => _metadata?.Tables[(int)MetadataTables.DefaultConstraints];

        public DataTable TriggersMetadata => _metadata?.Tables[(int)MetadataTables.Triggers];

        public DataTable ViewsMetadata => _metadata?.Tables[(int)MetadataTables.Views];

        public DataTable StoredProceduresMetadata => _metadata?.Tables[(int)MetadataTables.StoredProcedures];

        public DataTable StoredProcedureParametersMetadata => _metadata?.Tables[(int)MetadataTables.StoredProcedureParameters];

        public DataTable FunctionsMetadata => _metadata?.Tables[(int)MetadataTables.Functions];

        public DataTable FunctionParametersMetadata => _metadata?.Tables[(int)MetadataTables.FunctionParameters];

        public void LoadFromDatabase(string connectionString)
        {
            var resourceFileName = $"{GetType().Namespace}.SqlMetadataQueries.sql";
            var sqlSchemaQueries = ReadResourceFile(resourceFileName);
            _metadata = ExecuteDataSet(connectionString, sqlSchemaQueries);
            _metadata.DataSetName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }

        public async Task LoadFromDatabaseAsync(string connectionString)
        {
            var resourceFileName = $"{GetType().Namespace}.SqlMetadataQueries.sql";
            var sqlSchemaQueries = await ReadResourceFileAsync(resourceFileName);
            _metadata = await ExecuteDataSetAsync(connectionString, sqlSchemaQueries);
            _metadata.DataSetName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }

        public void LoadFromFile(string fileName)
        {
            var json = File.ReadAllText(fileName);
            _metadata = JsonConvert.DeserializeObject<DataSet>(json);
        }

        public async Task LoadFromFileAsync(string fileName)
        {
            // File.ReadAllTextAsync is not supported (yet?) in .Net Standard
            // see: http://jonesie.kiwi/2018/05/17/lemony-snippet-async-file-read-write-in-net-standard-2/
            // var json = await File.ReadAllTextAsync(fileName);

            string json;
            using (var streamReader = File.OpenText(fileName))
            {
                json = await streamReader.ReadToEndAsync();
            }

            _metadata = JsonConvert.DeserializeObject<DataSet>(json);
        }

        public void SaveToFile(string fileName)
        {
            var json = JsonConvert.SerializeObject(_metadata, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        public async Task SaveToFileAsync(string fileName)
        {
            var json = JsonConvert.SerializeObject(_metadata, Formatting.Indented);

            // File.WriteAllTextAsync is not supported (yet?) in .Net Standard
            // see: http://jonesie.kiwi/2018/05/17/lemony-snippet-async-file-read-write-in-net-standard-2/
            // await File.WriteAllTextAsync(fileName, json);

            using (var streamWriter = File.CreateText(fileName))
            {
                await streamWriter.WriteAsync(json);
            }
        }

        private static string ReadResourceFile(string resourceFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceFileName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        private static async Task<string> ReadResourceFileAsync(string resourceFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceFileName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    return await streamReader.ReadToEndAsync();
                }
            }
        }

        private static DataSet ExecuteDataSet(string connectionString, string sql)
        {
            using (var dataSet = new DataSet())
            {
                dataSet.Locale = CultureInfo.InvariantCulture;

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            _ = dataAdapter.Fill(dataSet);
                        }
                    }
                }

                return dataSet;
            }
        }

        private static async Task<DataSet> ExecuteDataSetAsync(string connectionString, string sql)
        {
            using (var dataSet = new DataSet())
            {
                dataSet.Locale = CultureInfo.InvariantCulture;

                using (var connection = new SqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;

                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            _ = await Task.Run(() => dataAdapter.Fill(dataSet));
                        }
                    }
                }

                return dataSet;
            }
        }
    }
}
