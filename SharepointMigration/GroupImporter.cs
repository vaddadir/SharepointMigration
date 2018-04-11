using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace SharepointMigration
{
    public class GroupImporter : IImport
    {
        public void Import(ImportContextBase context)
        {
            Context sourceContext = context.SourceContext;
            List<Group> groups = sourceContext.GetGroups();
        }
    }
}