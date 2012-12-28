using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 *Тэг	Relation
Аттрибуты	 serializationName, showChildren
Вложенные тэги	
Класс	XWPRelation
Описание	Описывает поведение связанных данных  (Child-Parent) на интерфейсе пользователя. 
 */


namespace DataAvail.XObject.XWP
{
    public class XWPRelation
    {
        public XWPRelation(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _relationName = reader.ReadAttribute("name", true);

            _isShown = reader.ReadAttributeBool("showChildren", false);

            _serializationName = reader.ReadAttribute("serializationName");
        }

        private readonly string _relationName;

        private readonly bool _isShown;

        private readonly string _serializationName;

        public string RelationName
        {
            get { return _relationName; }
        }

        public bool IsShown
        {
            get { return _isShown; }
        }

        public string SerializationName
        {
            get { return _serializationName; }
        } 

    }
}
