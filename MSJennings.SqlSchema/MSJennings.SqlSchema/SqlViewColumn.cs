namespace MSJennings.SqlSchema
{
    public class SqlViewColumn : SqlColumnBase
    {
        public SqlView View
        {
            get => Parent as SqlView;
            set => Parent = value;
        }

        public SqlViewColumn(string name) : base(name)
        {
        }
    }
}
