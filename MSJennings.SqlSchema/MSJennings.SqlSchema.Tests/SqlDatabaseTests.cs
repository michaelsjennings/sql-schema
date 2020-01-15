using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MSJennings.SqlSchema.Tests
{
    public class SqlDatabaseTests : SqlSchemaTestsBase
    {
        [Fact]
        public async Task LoadFromMetadata_WithPopulatedMetadata_ShouldLoadSuccessfully()
        {
            // arrange
            var database = new SqlDatabase();

            var metadata = new SqlMetadata();
            await metadata.LoadFromDatabaseAsync(_connectionString);

            // act
            database.LoadFromMetadata(metadata);

            // assert
            Assert.Equal(new SqlConnectionStringBuilder(_connectionString).InitialCatalog, database.Name);
            Assert.NotEmpty(database.Schemas);

            Assert.NotEmpty(database.Tables);
            Assert.True(database.Tables.All(x => x.Columns.Any()));

            Assert.NotEmpty(database.Views);
            Assert.True(database.Views.All(x => x.Columns.Any()));

            Assert.NotEmpty(database.Indexes);
            Assert.True(database.Indexes.All(x => x.KeyColumns.Any()));
            Assert.Contains(database.Indexes, x => x.IncludedColumns.Any());

            Assert.NotEmpty(database.ForeignKeys);
            Assert.True(database.ForeignKeys.All(x => x.Columns.Any()));
            Assert.True(database.ForeignKeys.All(x => x.PrimaryKey != null));

            Assert.NotEmpty(database.CheckConstraints);
            Assert.True(database.CheckConstraints.All(x => x.Column != null));

            Assert.NotEmpty(database.DefaultConstraints);
            Assert.True(database.DefaultConstraints.All(x => x.Column != null));

            Assert.NotEmpty(database.Triggers);
            Assert.NotEmpty(database.StoredProcedures);
            Assert.NotEmpty(database.Functions);
        }
    }
}
