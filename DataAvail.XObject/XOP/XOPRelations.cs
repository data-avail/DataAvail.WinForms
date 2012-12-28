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
Класс	XOPRelations
Описание	Определяет набор связей между данными в проекте
 */

namespace DataAvail.XObject.XOP
{
    public class XOPRelations
    {
        public XOPRelations(XElement XElement)
        {
            _relations = XmlLinq.GetChildren(XElement, "Relation", new Range(1, -1), XOApplication.xmlReaderLog).Select(p=>new XOPRelation(p)).ToArray();
        }

        private readonly XOPRelation[] _relations;

        public XOPRelation[] Relations
        {
            get { return _relations; }
        } 

    }
}
