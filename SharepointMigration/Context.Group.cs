using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace SharepointMigration
{
    public partial class Context
    {
        public List<Group> GetGroups()
        {
            List<Group> groups = new List<Group>();
            try
            {
                GroupCollection siteGroups = Web.SiteGroups;
                Load(siteGroups, grps => grps.IncludeWithDefaultProperties());
                ExecuteQuery();

                foreach (Group group in siteGroups)
                {
                    groups.Add(group);
                    Log.Info($"{group.Title}");
                    GetUsers(group);
                }
                return groups;
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}\t{ex.StackTrace}");
            }
            return groups;
        }
    }
}