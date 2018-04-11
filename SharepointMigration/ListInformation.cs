using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace SharepointMigration
{
    public class ListInformation : ListCreationInformation
    {
        public ListItemCollection ListItems { get; set; }

        public List<FieldInfo> Fields { get; set; }

        public ListInformation()
        {
            Title = string.Empty;
            Fields = new List<FieldInfo>();
        }
    }
}