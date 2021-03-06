﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.SqlConverter.XmlFormatter;
using DataAvail.SqlConverter;
using System.Data;

namespace DataAvail.SqlConverterTool
{
    public class CustomOptionsA : XmlFormatterOptions
    {
        public CustomOptionsA()
            : base()
        {
            this.tableOptions.collectionTag = null;

            this.fieldOptions.attrCreator = new FieldAttributeCreator();

            this.relationOptions.attrCreator = new RelationAttributeCreator();
        }

        private class FieldAttributeCreator : IAttributeCreator<IColumn>
        {
            #region IAttributeCreator<IColumn> Members

            public string GetAttributeValue(IColumn DbObject, string AttrName)
            {
                if (AttrName == DbExtensions.Options.fieldOptions.typeAttr)
                {
                    return string.Format("{0}{1}", FormatType(DbObject.DbType), DbObject.Size > 1 ? "," + DbObject.Size.ToString() : null);
                }
                else if (AttrName == DbExtensions.Options.fieldOptions.sizeAttr)
                {
                    return "";
                }
                else if (AttrName == DbExtensions.Options.fieldOptions.isPkAttr)
                {
                    return DbObject.IsPk ? null : "";
                }
                else if (AttrName == DbExtensions.Options.fieldOptions.isPkAutoGeneratedAttr)
                {
                    return DbObject.IsPk && !DbObject.IsPkAutoGenerated ? null : "";
                }
                else if (AttrName == DbExtensions.Options.fieldOptions.isNullableAttr)
                {
                    return DbObject.IsNullable ? null : "";
                }
                else if (AttrName == DbExtensions.Options.fieldOptions.defaultValueAttr)
                {
                    return !string.IsNullOrEmpty(DbObject.DefaultValue) && DbObject.DefaultValue  != "getdate" ? null : "";
                }
                else
                {
                    return null;
                }
            }

            public IDictionary<string, string> GetCustomAttributes(IColumn DbObject)
            {
                return null;
            }

            #endregion

            private string FormatType(DbType DbType)
            {
                switch (DbType)
                {
                    case DbType.Int32:
                        return "int";
                    case DbType.Int64:
                        return "long";
                    case DbType.String:
                        return "string";
                    case DbType.DateTime:
                        return "date";
                    case DbType.Decimal:
                        return "decimal";
                    case DbType.Double:
                        return "double";

                }

                throw new ArgumentException(string.Format("Can't parse this type {0}", DbType));
            }
        }

        private class RelationAttributeCreator : IAttributeCreator<IRelation>
        {
            #region IAttributeCreator<IRelation> Members

            public string GetAttributeValue(IRelation DbObject, string AttrName)
            {
                if (AttrName == DbExtensions.Options.relationOptions.nameAttr)
                {
                    return string.Format("{0}_{1}_{2}", DbObject.ChildTable, DbObject.ChildColumn, DbObject.ParentTable);
                }
                else
                {
                    return null;
                }
            }

            public IDictionary<string, string> GetCustomAttributes(IRelation DbObject)
            {
                return null;
            }

            #endregion
        }
    }
}
