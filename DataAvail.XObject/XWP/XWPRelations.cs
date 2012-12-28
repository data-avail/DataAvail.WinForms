using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 Тэг	Relations
Аттрибуты	 
Вложенные тэги	Relation
Класс	XWPRelations
Описание	Содержит описание набора связей данных интерфейса пользователя.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPRelations
    {
        public XWPRelations(XElement XElement)
        {
            _relations = XmlLinq.GetChildren(XElement, "Relation", new Range(1, -1), XOApplication.xmlReaderLog).Select(p=>new XWPRelation(p)).ToArray();
        }


        private readonly XWPRelation[] _relations;

        public XWPRelation[] Relations
        {
            get { return _relations; }
        } 

    }
}
