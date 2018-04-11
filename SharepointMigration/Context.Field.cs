using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace SharepointMigration
{
    public partial class Context
    {
        public List<FieldInfo> GetFields(List list)
        {
            FieldCollection fields = list.Fields;
            Load(fields, flds => flds.IncludeWithDefaultProperties(f => f.SchemaXml));
            ExecuteQuery();

            Log.Info("***** Field Names [BEGIN] *****");
            List<FieldInfo> fieldInfos = new List<FieldInfo>();

            for (int i = 0; i < fields.Count; i++)
            {
                Field field = fields[i];
                FieldInfo fieldInfo = new FieldInfo(field) { Index = i };
                Log.Info(fieldInfo.ToString());
                fieldInfos.Add(fieldInfo);
            }
            Log.Info("***** Field Names [END] *****");
            return fieldInfos;
        }

        public bool AddFields(string listTitle, List<FieldInfo> fields)
        {
            List targetList = Web.Lists.GetByTitle(listTitle);
            FieldCollection collField = targetList.Fields;

            fields.Sort();
            try
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    FieldInfo fieldInfo = fields[i];
                    string fieldSchema = fieldInfo.Element.ToString();

                    Field field;
                    if (TryGetField(targetList, fieldInfo.InternalName, out field))
                    {
                        field.SchemaXml = fieldSchema;
                    }
                    else
                    {
                        field = collField.AddFieldAsXml(fieldSchema, !fieldInfo.Hidden, AddFieldOptions.AddFieldInternalNameHint);
                    }
                    field.Update();
                }

                ExecuteQuery();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}\n{ex.StackTrace}");
                return false;
            }

            return true;
        }

        public bool DeleteFieldByTitle(string listTitle, string fieldTitle)
        {
            try
            {
                List targetList = Web.Lists.GetByTitle(listTitle);
                Field fld = targetList.Fields.GetByTitle(fieldTitle);
                Load(fld);
                fld.DeleteObject();
                fld.Update();
                ExecuteQuery();
            }
            catch (Exception ex)
            {
                Log.Error($"DeleteFieldByTitle: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
            return true;
        }

        public bool TryGetField(List targetList, string fieldInternalName, out Field field)
        {
            try
            {
                FieldCollection collField = targetList.Fields;
                field = collField.GetByInternalNameOrTitle(fieldInternalName);
                Load(field, f => f.SchemaXml);
                ExecuteQuery();
                if (field.ServerObjectIsNull.HasValue && field.ServerObjectIsNull.Value == false)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            field = null;
            return false;
        }
    }
}