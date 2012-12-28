using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.DataSetParser
{
    public static class DataSetBuilder
    {
        public static System.Data.DataSet Build(string XMLFile)
        {
            System.Data.DataSet dataSet = DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateDataSet();

            XDocument xdoc = XDocument.Load(XMLFile);

            string typeConverters = XmlLinq.GetAttribute(xdoc.Root, "typesConverter");

            if (!string.IsNullOrEmpty(typeConverters))
            {
                DataSetParser.TypesConverters = typeConverters.Split(',').ToDictionary(k=>k.Substring(0, k.IndexOf('=')), v=>v.Substring(v.IndexOf('=') + 1));
            }

            foreach (XElement xTab in xdoc.Root.Elements().Where(p => p.Name == "Table"))
            {
                string tabName = XmlLinq.GetAttribute(xTab, "name");

                IEnumerable<XElement> xCols = xTab.Elements().First(p => p.Name == "Fields").Elements().Where(p => p.Name == "Field");

                ////Create select command

                string sourceName = XmlLinq.GetAttribute(xTab, "source");

                if (sourceName == null)
                {
                    sourceName = tabName;
                }

                if (!string.IsNullOrEmpty(XmlLinq.GetAttribute(xdoc.Root, "defaultTablePrefix")))
                {
                    sourceName = string.Format("{0}{1}", XmlLinq.GetAttribute(xdoc.Root, "defaultTablePrefix"), sourceName);
                }

                var r = xCols.Where(p => string.IsNullOrEmpty(XmlLinq.GetAttribute(p, "calculator"))).
                    Select(p => XmlLinq.GetAttribute(p, "name"));

                string selectCommandText = DataAvail.Data.DataTable.GetSelectCommandText(XmlLinq.GetAttribute(xdoc.Root, "defaultSchemeName"), sourceName, r);

                System.Data.IDbCommand selectCommand = DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand().Command;

                selectCommand.Connection = DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.Connection;
                selectCommand.CommandText = selectCommandText;
                selectCommand.CommandType = System.Data.CommandType.Text;

                //////////////////

                System.Data.DataTable dataTable = DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateDataTable(selectCommand);

                dataTable.TableName = tabName;

                dataSet.Tables.Add(dataTable);

                foreach (XElement xCol in xCols)
                {
                    System.Data.DataColumn dataCol = new System.Data.DataColumn();

                    dataTable.Columns.Add(dataCol);

                    dataCol.ColumnName = XmlLinq.GetAttribute(xCol, "name");

                    KeyValuePair<Type, int> kvp = DataSetParser.GetFieldTypeSize(XmlLinq.GetAttribute(xCol, "type"));

                    dataCol.DataType = kvp.Key;

                    if (dataCol.DataType == typeof(string))
                        dataCol.MaxLength = kvp.Value;

                    if (XmlLinq.GetAttribute(xCol, "pk") == "true")
                    {
                        dataCol.AutoIncrement = true;
                        dataCol.AllowDBNull = false;

                        if (XmlLinq.GetAttribute(xCol, "readOnly") == "false")
                        {
                            dataCol.AutoIncrementSeed = 0;
                            dataCol.AutoIncrementStep = 1;
                        }
                        else
                        {
                            dataCol.AutoIncrementSeed = -1;
                            dataCol.AutoIncrementStep = -1;
                        }

                        dataTable.Constraints.Add(new System.Data.UniqueConstraint("ConstraintPK", new System.Data.DataColumn[] { dataCol }, true));
                    }


                }

                dataTable.EndInit();
            }

            dataSet.EndInit();

            return dataSet;
        }
    }
}
