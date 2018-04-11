using Microsoft.SharePoint.Client;
using System;
using System.Text;
using System.Xml.Linq;

namespace SharepointMigration
{
    //Did not inherit from Field as Field class has a constructor which needs more information.
    public class FieldInfo : IComparable<FieldInfo>
    {
        public int Index { get; set; }
        public string InternalName { get; set; }
        public FieldType Type { get; set; }
        public XElement Element { get; set; }
        public bool Hidden { get; set; }
        public bool Readonly { get; set; }

        public FieldInfo(Field field)
        {
            InternalName = field.InternalName;
            Type = field.FieldTypeKind;
            Hidden = field.Hidden;
            Readonly = field.ReadOnlyField;
            Element = XElement.Parse(field.SchemaXml);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"Index: {Index}\t");
            builder.Append($"InternalName: {InternalName}\t");
            builder.Append($"Type: {Type}\t");
            builder.Append($"IsHidden: {Hidden}\t");
            builder.Append($"IsReadonly: {Readonly}");
            return builder.ToString();
        }

        public int CompareTo(FieldInfo other)
        {
            return Index.CompareTo(other.Index);
        }
    }
}