using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.SqlConverter.XmlFormatter;
using DataAvail.SqlConverter;
using System.Data;

namespace DataAvail.SqlConverterTool
{
    class CustomOptionsB : XmlFormatterOptionsA
    {
        public CustomOptionsB()
            : base()
        {
            this.relationOptions = null;
            this.tableOptions.collectionTag = null;
            this.tableOptions.attrCreator = new TableAttributeCreator();
            this.fieldOptions.attrCreator = new FieldAttributeCreator();
        }

        class TableAttributeCreator : IAttributeCreator<ITable> 
        {

            #region IAttributeCreator<ITable> Members

            public string GetAttributeValue(ITable DbObject, string AttrName)
            {
                return null;
            }

            public IDictionary<string, string> GetCustomAttributes(ITable DbObject)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                
                dict.Add("caption", "#");
                dict.Add("itemCaption", "#");

                return dict;
            }

            #endregion
        }

        class FieldAttributeCreator : IAttributeCreator<IColumn>
        {

            #region IAttributeCreator<ITable> Members

            public string GetAttributeValue(IColumn DbObject, string AttrName)
            {
                return null;
            }

            public IDictionary<string, string> GetCustomAttributes(IColumn DbObject)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();

                dict.Add("caption", "#");

                return dict;
            }

            #endregion
        }

    }
}
