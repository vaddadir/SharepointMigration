using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace SharepointMigration
{
    public partial class Context
    {
        public bool AddListItem(string listTitle, Dictionary<string, string> fieldValues)
        {
            try
            {
                List<string> ignoreFields = new List<string>()
                {
                    "ID", "Attachments"
                };
                List targetList = Web.Lists.GetByTitle(listTitle);

                ListItemCreationInformation listItemCreateInfo = new ListItemCreationInformation();
                ListItem targetListItem = targetList.AddItem(listItemCreateInfo);
                foreach (var pair in fieldValues)
                {
                    Log.Info($"updating {pair.Key}");
                    targetListItem[pair.Key] = pair.Value;
                    targetListItem.Update();
                    ExecuteQuery();
                    Log.Info($"{pair.Key} updated with {pair.Value}");
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}