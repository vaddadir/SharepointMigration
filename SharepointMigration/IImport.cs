namespace SharepointMigration
{
    internal interface IImport
    {
        void Import(ImportContextBase context);
    }

    public abstract class ImportContextBase
    {
        public struct IncludeItems
        {
            public bool Fields { get; set; }
            public bool ListItems { get; set; }
        }

        public Context SourceContext { get; }

        public Context TargetContext { get; }

        public IncludeItems Include;

        protected ImportContextBase(Context sourceContext, Context targetContext)
        {
            SourceContext = sourceContext;
            TargetContext = targetContext;
            Include = new IncludeItems
            {
                Fields = true,
                ListItems = true
            };
        }
    }
}