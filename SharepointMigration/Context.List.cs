using Microsoft.SharePoint.Client;
using System;
using System.Linq;

namespace SharepointMigration
{
    public partial class Context
    {
        public List GetListByTitle(string title)
        {
            try
            {
                Load(Web.Lists, lists => lists.IncludeWithDefaultProperties());

                ExecuteQuery();
            }
            catch (Exception ex)
            {
                return null;
            }

            return Web.Lists.FirstOrDefault(li => li.Title == title);
        }

        public ListItemCollection GetListItemsByTitle(string title)
        {
            List list = Web.Lists.GetByTitle(title);

            CamlQuery query = CamlQuery.CreateAllItemsQuery();

            query.ViewXml = "<View Scope=\"RecursiveAll\"></View>";

            ListItemCollection items = list.GetItems(query);

            Load(items, itms => itms.Include(i => i.DisplayName, i => i.Id, i => i.ContentType, i => i.FieldValuesAsHtml));

            ExecuteQuery();

            return items;
        }

        public ListItemCollection GetListItems(List list)
        {
            CamlQuery query = CamlQuery.CreateAllItemsQuery();

            query.ViewXml = "<View Scope=\"RecursiveAll\"></View>";

            ListItemCollection items = list.GetItems(query);

            Load(items, itms => itms.Include(i => i.DisplayName, i => i.Id, i => i.ContentType, i => i.FieldValuesAsHtml));

            ExecuteQuery();

            return items;
        }

        public void CreateList(ListCreationInformation listCreationInformation)
        {
            List targetList = GetListByTitle(listCreationInformation.Title);
            if (targetList == null)
            {
                Web.Lists.Add(listCreationInformation);
            }
            ExecuteQuery();
        }
    }
}