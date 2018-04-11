using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace SharepointMigration
{
    public class ListImporter : IImport
    {
        #region Private Fields

        private ListInformationCollection listInformationCollection = new ListInformationCollection();

        #endregion Private Fields

        #region Public Methods

        public void Verify(ImportContextBase context)
        {
            ListImportContext listImportContext = context as ListImportContext;
            listInformationCollection.Clear();
            Fetch(context.TargetContext, listImportContext.ListNames);
        }

        public void Import(ImportContextBase context)
        {
            Context sourceContext = context.SourceContext;

            ListImportContext listImportContext = context as ListImportContext;
            Fetch(sourceContext, listImportContext.ListNames);

            Context targetContext = context.TargetContext;
            foreach (var listTitle in listInformationCollection.Keys)
            {
                ListInformation listInformation = listInformationCollection[listTitle];

                targetContext.CreateList(listInformation);
                if (context.Include.Fields)
                {
                    targetContext.AddFields(listTitle, listInformation.Fields);
                }
                if (context.Include.ListItems)
                {
                    foreach (ListItem listItem in listInformation.ListItems)
                    {
                        Dictionary<string, string> sourceFieldValues = listItem.FieldValuesAsHtml.FieldValues;
                        Dictionary<string, string> targetFieldValues = new Dictionary<string, string>();
                        foreach (FieldInfo field in listInformation.Fields)
                        {
                            if (field.Type == FieldType.Counter || field.Type == FieldType.Computed || field.Type == FieldType.Attachments || field.Type == FieldType.Calculated || field.Type == FieldType.ModStat || field.Type == FieldType.User)
                            {
                                continue;
                            }
                            string key = field.InternalName;
                            string value = sourceFieldValues[key];
                            if (!string.IsNullOrEmpty(value))
                            {
                                targetFieldValues.Add(key, value);
                            }
                        }
                        targetContext.AddListItem(listTitle, targetFieldValues);
                        break;
                    }
                }
            };
        }

        #endregion Public Methods

        #region Private Methods

        private void Fetch(Context sourceContext, List<string> listNames)
        {
            Web sourceContextWeb = sourceContext.Web;
            sourceContext.Load(sourceContextWeb.Lists, lists => lists.IncludeWithDefaultProperties());

            sourceContext.ExecuteQuery();

            foreach (List list in sourceContextWeb.Lists)
            {
                if (list.BaseType != (int)BaseType.GenericList)
                {
                    continue;
                }

                string listTitle = list.Title;
                if (!listNames.Contains(listTitle))
                {
                    continue;
                }

                string listDescription = list.Description;
                int listBaseTemplate = list.BaseTemplate;

                ListInformation listInformation = new ListInformation()
                {
                    Title = listTitle,
                    Description = listDescription,
                    TemplateType = listBaseTemplate,
                    Fields = sourceContext.GetFields(list),
                    ListItems = sourceContext.GetListItems(list),
                };
                listInformationCollection[listTitle] = listInformation;
            }
        }

        #endregion Private Methods
    }
}