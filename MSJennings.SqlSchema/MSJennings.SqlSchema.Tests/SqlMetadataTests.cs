using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace MSJennings.SqlSchema.Tests
{
    public class SqlMetadataTests : SqlSchemaTestsBase
    {
        [Fact]
        public async Task LoadFromDatabaseAsync_WithValidConnectionString_ShouldLoadSuccessfully()
        {
            // arrange
            var metadata = new SqlMetadata();

            // act
            await metadata.LoadFromDatabaseAsync(_connectionString);

            // assert
            Assert.NotEmpty(metadata.TablesMetadata.Rows);
            Assert.NotEmpty(metadata.IndexesMetadata.Rows);
            Assert.NotEmpty(metadata.ForeignKeysMetadata.Rows);
            Assert.NotEmpty(metadata.CheckConstraintsMetadata.Rows);
            Assert.NotEmpty(metadata.DefaultConstraintsMetadata.Rows);
            Assert.NotEmpty(metadata.TriggersMetadata.Rows);
            Assert.NotEmpty(metadata.ViewsMetadata.Rows);
            Assert.NotEmpty(metadata.StoredProceduresMetadata.Rows);
            Assert.NotEmpty(metadata.StoredProcedureParametersMetadata.Rows);
            Assert.NotEmpty(metadata.FunctionsMetadata.Rows);
            Assert.NotEmpty(metadata.FunctionParametersMetadata.Rows);
        }

        [Fact]
        public async Task SaveToFileAsync_WithValidSchema_ShouldWriteReadableFile()
        {
            // arrange
            var metadata1 = new SqlMetadata();
            var metadata2 = new SqlMetadata();

            await metadata1.LoadFromDatabaseAsync(_connectionString);
            var fileName = Path.GetTempFileName();

            // act
            await metadata1.SaveToFileAsync(fileName);
            await metadata2.LoadFromFileAsync(fileName);

            // assert
            Assert.Equal(metadata1.TablesMetadata.Rows.Count, metadata2.TablesMetadata.Rows.Count);
            Assert.Equal(metadata1.IndexesMetadata.Rows.Count, metadata2.IndexesMetadata.Rows.Count);
            Assert.Equal(metadata1.ForeignKeysMetadata.Rows.Count, metadata2.ForeignKeysMetadata.Rows.Count);
            Assert.Equal(metadata1.CheckConstraintsMetadata.Rows.Count, metadata2.CheckConstraintsMetadata.Rows.Count);
            Assert.Equal(metadata1.DefaultConstraintsMetadata.Rows.Count, metadata2.DefaultConstraintsMetadata.Rows.Count);
            Assert.Equal(metadata1.TriggersMetadata.Rows.Count, metadata2.TriggersMetadata.Rows.Count);
            Assert.Equal(metadata1.ViewsMetadata.Rows.Count, metadata2.ViewsMetadata.Rows.Count);
            Assert.Equal(metadata1.StoredProceduresMetadata.Rows.Count, metadata2.StoredProceduresMetadata.Rows.Count);
            Assert.Equal(metadata1.StoredProcedureParametersMetadata.Rows.Count, metadata2.StoredProcedureParametersMetadata.Rows.Count);
            Assert.Equal(metadata1.FunctionsMetadata.Rows.Count, metadata2.FunctionsMetadata.Rows.Count);
            Assert.Equal(metadata1.FunctionParametersMetadata.Rows.Count, metadata2.FunctionParametersMetadata.Rows.Count);

        }
    }
}
