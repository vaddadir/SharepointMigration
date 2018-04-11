using System.Collections.Generic;

namespace SharepointMigration
{
    public class ListImportContext : ImportContextBase
    {
        public List<string> ListNames { get; set; }

        public ListImportContext(Context sourceContext, Context targetContext) : base(sourceContext, targetContext)
        {
            ListNames = new List<string>();
        }
    }
}