using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace SharepointMigration
{
    public partial class Context
    {
        public List<User> GetUsers(Group group)
        {
            List<User> users = new List<User>();
            try
            {
                UserCollection groupMembers = group.Users;
                Load(groupMembers);
                ExecuteQuery();

                foreach (User member in groupMembers)
                {
                    users.Add(member);
                    Log.Info(member.Title);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}\t{ex.StackTrace}");
            }
            return users;
        }
    }
}