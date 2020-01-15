using System;
using System.Data;
using System.Globalization;

namespace MSJennings.SqlSchema
{
    public class SqlDataType
    {
        public string SqlTypeName { get; set; }

        public int MaxLength { get; set; }

        public bool IsUnlimitedLength => MaxLength < 0;

        public int Precision { get; set; }

        public int Scale { get; set; }

        public SqlDbType SqlDbType
        {
            get
            {
                switch (SqlTypeName.ToUpperInvariant())
                {
                    case "BIGINT":
                        return SqlDbType.BigInt;

                    case "BINARY":
                        return SqlDbType.Binary;

                    case "BIT":
                        return SqlDbType.Bit;

                    case "CHAR":
                        return SqlDbType.Char;

                    case "DATE":
                        return SqlDbType.Date;

                    case "DATETIME":
                        return SqlDbType.DateTime;

                    case "DATETIME2":
                        return SqlDbType.DateTime2;

                    case "DATETIMEOFFSET":
                        return SqlDbType.DateTimeOffset;

                    case "DECIMAL":
                        return SqlDbType.Decimal;

                    case "FLOAT":
                        return SqlDbType.Float;

                    case "GEOGRAPHY":
                        return SqlDbType.Udt;

                    case "GEOMETRY":
                        return SqlDbType.Udt;

                    case "HIERARCHYID":
                        return SqlDbType.Udt;

                    case "IMAGE":
                        return SqlDbType.Image;

                    case "INT":
                        return SqlDbType.Int;

                    case "MONEY":
                        return SqlDbType.Money;

                    case "NCHAR":
                        return SqlDbType.NChar;

                    case "NTEXT":
                        return SqlDbType.NText;

                    case "NUMERIC":
                        return SqlDbType.Decimal;

                    case "NVARCHAR":
                        return SqlDbType.NVarChar;

                    case "REAL":
                        return SqlDbType.Real;

                    case "SMALLDATETIME":
                        return SqlDbType.SmallDateTime;

                    case "SMALLINT":
                        return SqlDbType.SmallInt;

                    case "SMALLMONEY":
                        return SqlDbType.SmallMoney;

                    case "SQL_VARIANT":
                        return SqlDbType.Variant;

                    case "SYSNAME":
                        return SqlDbType.VarChar;

                    case "TEXT":
                        return SqlDbType.Text;

                    case "TIME":
                        return SqlDbType.Time;

                    case "TIMESTAMP":
                        return SqlDbType.Timestamp;

                    case "TINYINT":
                        return SqlDbType.TinyInt;

                    case "UNIQUEIDENTIFIER":
                        return SqlDbType.UniqueIdentifier;

                    case "VARBINARY":
                        return SqlDbType.VarBinary;

                    case "VARCHAR":
                        return SqlDbType.VarChar;

                    case "XML":
                        return SqlDbType.Xml;

                    default:
                        var message = string.Format(CultureInfo.InvariantCulture, "The type name '{0}' has not been mapped to a {1}.", SqlTypeName, nameof(SqlDbType));
                        throw new InvalidOperationException(message);
                }
            }
        }
    }
}
