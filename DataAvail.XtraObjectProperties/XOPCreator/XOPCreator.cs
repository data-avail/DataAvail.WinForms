using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.XtraObjectProperties;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XtraObjectProperties.XOPCreator
{
    public static class XOPCreator
    {
        public static XtraObjectPropertiesCollection Create(IXOPCreatorProvider XOPCreatorProvider, string ModelFileName, string ModelViewFileName, string ModelSecurityFileName)
        {
            IEnumerable<XtraObjectProperties> objs = Create(XOPCreatorProvider).ToArray();

            if (!string.IsNullOrEmpty(ModelFileName))
            {
                Initialize(objs, ModelFileName, XOPCreatorProvider);
            }

            XOPCreatorProvider.FinalizeCreation(objs);

            XOP.ModelView.ModelViewApp mvApp = null;

            if (!string.IsNullOrEmpty(ModelViewFileName))
            {
                mvApp = XOP.ModelView.ModelViewApp.Load(ModelViewFileName);
            }

            XOP.ModelSecurity.ModelSecApp msApp = null;

            if (!string.IsNullOrEmpty(ModelSecurityFileName))
            {
                msApp = XOP.ModelSecurity.ModelSecApp.Load(ModelSecurityFileName);
            }

            XOP.AppContext.AppContext appItems = new DataAvail.XOP.AppContext.AppContext(objs, mvApp, msApp);

            return new XtraObjectPropertiesCollection(objs, appItems);
        }

        public static IEnumerable<XtraObjectProperties> Create(IXOPCreatorProvider XOPCreatorProvider)
        {
            XtraObjectProperties objectProperties = null;

            XtraFieldProperties fieldProperties = null;

            while ((objectProperties = XOPCreatorProvider.NextObject()) != null)
            {
                while ((fieldProperties = XOPCreatorProvider.NextField()) != null)
                {
                    objectProperties.Fields.AddFieldProperties(fieldProperties);
                }

                yield return objectProperties;
            }
        }

        public static void Initialize(IEnumerable<XtraObjectProperties> ObjectProperties, string FileName, IXOPFieldCreator XOPFieldCreator)
        {
            XDocument xdoc = System.Xml.Linq.XDocument.Load(FileName);

            string defaultParentDisplayColumn = XmlLinq.GetAttribute(xdoc.Root, "DefaultParentDisplayColumn");

            string defaultTablePrefix = XmlLinq.GetAttribute(xdoc.Root, "defaultTablePrefix");

            bool defUseSeparSz = true;

            bool.TryParse(XmlLinq.GetAttribute(xdoc.Root, "separateSerialization"), out defUseSeparSz);

            bool useDefaultCommands = false;

            if (!string.IsNullOrEmpty(XmlLinq.GetAttribute(xdoc.Root, "useDefaultCommands")))
            {
                useDefaultCommands = bool.Parse(XmlLinq.GetAttribute(xdoc.Root, "useDefaultCommands"));
            }

            foreach (XtraObjectProperties tabProp in ObjectProperties)
            {
                XElement xTab = GetElement(xdoc.Root, "Table", tabProp.ObjectName);

                IEnumerable<XElement> fkRelations = xdoc.Root.Descendants("Relations").Descendants().Where(p => XmlLinq.GetAttribute(p, "ChildTable") == tabProp.ObjectName);

                if (xTab != null)
                {
                    #region Table

                    tabProp.separateSerialization = defUseSeparSz;

                    string tablePrefix = defaultTablePrefix;

                    foreach (XAttribute attr in xTab.Attributes().Where(p => !string.IsNullOrEmpty(p.Value)))
                    {
                        switch (attr.Name.LocalName)
                        {
                            case "caption":
                                tabProp.Caption = attr.Value;
                                break;
                            case "itemCaption":
                                tabProp.ItemCaption = attr.Value;
                                break;
                            case "autoFill":
                                tabProp.AutoFill = bool.Parse(attr.Value);
                                break;
                            case "source":
                                tabProp.ObjectSource = attr.Value;
                                break;
                            case "persistFill":
                                tabProp.PersistFill = bool.Parse(attr.Value);
                                break;
                            case "useCommands":
                                tabProp.UseCommands = DataAvail.Utils.EditMode.Parse(attr.Value);
                                break;
                            case "tablePrefix":
                                tablePrefix = attr.Value;
                                break;
                            case "sourceUpdate":
                                tabProp.ObjectSourceUpdate = attr.Value;
                                break;
                            case "separateSerialization":
                                tabProp.separateSerialization = bool.Parse("separateSerialization");
                                break;
                        }
                    }

                    if (tabProp.UseCommands == DataAvail.Utils.EditModeType.Default && useDefaultCommands)
                        tabProp.UseCommands = DataAvail.Utils.EditModeType.All;

                    if (!string.IsNullOrEmpty(tablePrefix))
                        tabProp.ObjectSource = string.Format("{0}{1}", tablePrefix, tabProp.ObjectSource);

                    if (string.IsNullOrEmpty(tabProp.ObjectSourceUpdate))
                        tabProp.ObjectSourceUpdate = tabProp.ObjectSource;
                    else
                        tabProp.ObjectSourceUpdate = string.Format("{0}{1}", tablePrefix, tabProp.ObjectSourceUpdate);

                    #endregion

                    #region Columns

                    XElement xCols = xTab.Descendants("Columns").SingleOrDefault();

                    string tabDefParentDisplayColumn = XmlLinq.GetAttribute(xCols, "parentDisplayColumn");

                    if (tabDefParentDisplayColumn == null)
                        tabDefParentDisplayColumn = defaultParentDisplayColumn;

                    foreach (XtraFieldProperties colProp in tabProp.Fields.Cast<XtraFieldProperties>().ToArray())
                    {

                        XElement xCol = GetElement(xCols ?? xTab, "Column", colProp.FieldName);

                        XtraFieldProperties cProp = colProp;

                        #region FK relations

                        XElement xRel = fkRelations.FirstOrDefault(p => XmlLinq.GetAttribute(p, "ChildColumn") == colProp.FieldName);

                        FKFieldDescriptor fieldDescr = null;

                        if (xRel != null)
                        {
                            fieldDescr = GetFKFieldDescriptor(xRel);

                            if (fieldDescr != null)
                            {
                                cProp = XOPFieldCreator.CreateFkField(cProp, fieldDescr);

                                tabProp.Fields.Swap(colProp, cProp);
                            }
                        }

                        if (cProp is DataAvail.XtraObjectProperties.IFKFieldProperties
                           && !string.IsNullOrEmpty(defaultParentDisplayColumn))
                        {
                            ((DataAvail.XtraObjectProperties.IFKFieldProperties)cProp).DisplayMember = tabDefParentDisplayColumn;
                        }

                        #endregion

                        if (xCol != null)
                        {
                            foreach (XAttribute attr in xCol.Attributes())
                            {
                                bool f;
                                XtraFieldProperties newProp = null;

                                switch (attr.Name.LocalName)
                                {
                                    case "pk":
                                        bool.TryParse(attr.Value, out f);
                                        cProp.IsPK = f;
                                        break;
                                    case "caption":
                                        cProp.Caption = attr.Value;
                                        break;
                                    case "mask":
                                        DataAvail.XtraObjectProperties.XtraFieldMask mask = DataAvail.XtraObjectProperties.XtraFieldMask.Parse(attr.Value);
                                        if (mask != null)
                                        {
                                            if (cProp is DataAvail.XtraObjectProperties.XtraTextFieldProperties)
                                            {
                                                ((DataAvail.XtraObjectProperties.XtraTextFieldProperties)cProp).XtraFieldMask = mask;
                                            }
                                            else
                                            {
                                                newProp = new XtraTextFieldProperties(cProp) { XtraFieldMask = mask };
                                            }
                                        }
                                        break;
                                    case "readOnly":
                                        bool.TryParse(attr.Value, out f);
                                        cProp.AllowUserEdit = !f;
                                        break;
                                    case "nullInput":
                                        cProp.AllowUserNull = false;
                                        break;
                                    case "isMapped":
                                        cProp.IsMapped = bool.Parse(attr.Value);
                                        if (!cProp.IsMapped)
                                            cProp.AllowUserEdit = false;
                                        break;
                                    case "mappedParamName":
                                        cProp.MappedParamName = attr.Value;
                                        break;
                                    case "parentDisplayColumn":
                                        if (cProp is DataAvail.XtraObjectProperties.IFKFieldProperties)
                                        {
                                            ((DataAvail.XtraObjectProperties.IFKFieldProperties)cProp).DisplayMember = attr.Value;
                                        }
                                        break;
                                    case "defaultValue":
                                        cProp.DefaultValue = DataAvail.Utils.Reflection.Parse(cProp.FieldType, attr.Value);//cProp.FieldType.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { attr.Value });
                                        break;
                                    case "itemSelector":
                                        cProp.SelectFkItemType = DataAvail.Utils.EditMode.Parse(attr.Value);
                                        break;
                                    case "calculator":
                                        cProp.CalculatorExpression = attr.Value;
                                        break;
                                    case "controlType":
                                        cProp.SpecifiedControlType = attr.Value;
                                        break;
                                    case "commands":
                                        cProp.FKCommands = DataAvail.Utils.EnumFlags.Parse(attr.Value, DataAvail.XtraObjectProperties.XOPFieldFkCommands.all);
                                        break;
                                }

                                if (newProp != null)
                                {
                                    tabProp.Fields.Swap(cProp, newProp);

                                    cProp = newProp;
                                }
                            }

                            if (!string.IsNullOrEmpty(cProp.CalculatorExpression))
                            {
                                cProp.IsMapped = false;
                                cProp.AllowUserEdit = false;
                            }

                            if (!cProp.IsMapped)
                                cProp.AllowUserEdit = false;
                        }
                    }

                    #endregion

                    #region Functions


                    foreach (XElement xFunc in xTab.Descendants("Functions").Descendants())
                    {
                        XtraObjectFunction func = new XtraObjectFunction(XmlLinq.GetAttribute(xFunc, "name"));

                        string funcType = XmlLinq.GetAttribute(xFunc, "type");

                        if (funcType == "create | update" )
                        {
                            if(!DataAvail.Utils.EnumFlags.IsContain(tabProp.UseCommands, DataAvail.Utils.EditModeType.Add))
                                tabProp.Functions.createFunction = func;

                            if (!DataAvail.Utils.EnumFlags.IsContain(tabProp.UseCommands, DataAvail.Utils.EditModeType.Edit))
                                tabProp.Functions.updateFunction = func;

                        }
                        else if (funcType == "create")
                        {
                            if (!DataAvail.Utils.EnumFlags.IsContain(tabProp.UseCommands, DataAvail.Utils.EditModeType.Add))
                                tabProp.Functions.createFunction = func;

                        }
                        else if (funcType == "update")
                        {
                            if (!DataAvail.Utils.EnumFlags.IsContain(tabProp.UseCommands, DataAvail.Utils.EditModeType.Edit)) 
                                tabProp.Functions.updateFunction = func;
                        }
                        else if (funcType == "delete")
                        {
                            if (!DataAvail.Utils.EnumFlags.IsContain(tabProp.UseCommands, DataAvail.Utils.EditModeType.Delete)) 
                                tabProp.Functions.deleteFunction = func;
                        }

                        #region Function parameters

                        string attr = null;

                        foreach (XElement xFuncParam in xFunc.Descendants())
                        {
                            XtraObjectFunctionParameter param = new XtraObjectFunctionParameter();

                            param.name = XmlLinq.GetAttribute(xFuncParam, "name");

                            attr = XmlLinq.GetAttribute(xFuncParam, "mappedColumn");

                            if (attr != null)
                                param.mappedField = tabProp.Fields.Cast<XtraFieldProperties>().SingleOrDefault(p => p.FieldName == attr);

                            attr = XmlLinq.GetAttribute(xFuncParam, "direction");

                            if (attr != null)
                                param.direction = (System.Data.ParameterDirection)System.Enum.Parse(typeof(System.Data.ParameterDirection), attr);

                            attr = XmlLinq.GetAttribute(xFuncParam, "type");

                            if (attr != null)
                            {
                                KeyValuePair<Type, int> kvp = DataAvail.DataSetParser.DataSetParser.GetFieldTypeSize(attr);

                                param.type = kvp.Key;
                                param.size = kvp.Value;
                            }

                            func.funcParams.AddParam(param);

                            attr = XmlLinq.GetAttribute(xFuncParam, "value");

                            if (attr != null)
                                param.val = param.type.GetMethod("Parse", new Type[] { typeof(string) }).Invoke(null, new object[] { attr });

                        }

                        #endregion

                    }

                    #region Mapped columns

                    foreach (XtraFieldProperties prop in tabProp.Fields.Cast<XtraFieldProperties>().Where(p => p.IsMapped == true))
                    {
                        if (tabProp.Functions.createFunction != null)
                        {
                            if (tabProp.Functions.createFunction.funcParams.SingleOrDefault(p => p.mappedField == prop) == null)
                            {
                                XtraObjectFunctionParameter param = new XtraObjectFunctionParameter();

                                tabProp.Functions.createFunction.funcParams.AddParam(param);

                                param.mappedField = prop;
                            }
                        }
                    }

                    #endregion

                    #endregion
                }
            }

            #region relations

            foreach (XElement xRel in xdoc.Root.Descendants("Relations").Descendants())
            {
                XtraObjectProperties parTab = ObjectProperties.First(p => p.ObjectName == XmlLinq.GetAttribute(xRel, "ParentTable"));
                XtraObjectProperties childTab = ObjectProperties.First(p => p.ObjectName == XmlLinq.GetAttribute(xRel, "ChildTable"));
                XtraFieldProperties parCol = parTab.Fields.First(p => p.FieldName == XmlLinq.GetAttribute(xRel, "ParentColumn"));
                XtraFieldProperties childCol = childTab.Fields.First(p => p.FieldName == XmlLinq.GetAttribute(xRel, "ChildColumn"));

                bool isShown = false;

                string relSzName = XmlLinq.GetAttribute(xRel, "szName");

                bool.TryParse(XmlLinq.GetAttribute(xRel, "ShowChildren"), out isShown);

                XOP.XOPFieldValueCollection fvc = XOP.XOPFieldValueCollection.Parse(parTab, xRel);

                parTab.ChildrenRelations.Add(new XtraObjectRelation(parTab, parCol, childTab, childCol, isShown, fvc, relSzName));

                if (isShown)
                    childCol.AllowUserEdit = false;
            }

            #endregion

        }

        private static XElement GetElement(XElement Root, string ElementName, string AttrName)
        {
            return GetElements(Root, ElementName, AttrName).SingleOrDefault();
        }

        private static IEnumerable<XElement> GetElements(XElement Root, string ElementName, string AttrName)
        {
            return Root.Elements().Where(p => p.Name == ElementName && p.Attributes().FirstOrDefault(a => a.Name == "name" && a.Value == AttrName) != null);
        }

        private static FKFieldDescriptor GetFKFieldDescriptor(XElement XElement)
        {
            if (XmlLinq.GetAttribute(XElement, "ParentTable") != null)
            {
                return new FKFieldDescriptorTable(XmlLinq.GetAttribute(XElement, "ParentTable"),
                    XmlLinq.GetAttribute(XElement, "ParentColumn"),
                    XmlLinq.GetAttribute(XElement, "ChildTable"),
                    XmlLinq.GetAttribute(XElement, "ChildColumn"),
                    XmlLinq.GetAttribute(XElement, "Filter"));
            }
            else
            {
                return new FKFieldDescriptorDictionary(XElement.Descendants("Item").ToDictionary(k => XmlLinq.GetAttribute(k, "Value"), v => XmlLinq.GetAttribute(v, "Value")),
                    XmlLinq.GetAttribute(XElement, "Filter")
                    );
            }
        }
    }
}
