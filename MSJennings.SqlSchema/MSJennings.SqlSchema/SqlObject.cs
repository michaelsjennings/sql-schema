namespace MSJennings.SqlSchema
{
    public abstract class SqlObject
    {
        public string Name { get; protected set; }

        protected SqlObject(string name)
        {
            Name = name;
        }
    }
}
