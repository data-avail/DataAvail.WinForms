using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 * Тэг	Functions
Аттрибуты	 
Вложенные тэги	Function[1..4]
Класс	XOPFunctions
Описание	Определяет набор функций через которые данные будут считываться  -сохраняться из/в  БД.
Всего может содержать 4 функции для создания, сохранения, удаления и заполнения данных из/в базу данных для отдельной таблицы (см Table).
 */

namespace DataAvail.XObject.XOP
{
    public class XOPFunctions
    {
        public XOPFunctions(XElement XElement)
        {
            if (XElement != null)
                _functions = XmlLinq.GetChildren(XElement, "Function", new DataAvail.Utils.Range(1, 4), XOApplication.xmlReaderLog).Select(p => new XOPFunction(p)).ToArray();
            else
                _functions = new XOPFunction[] { };
        }

        private readonly XOPFunction[] _functions;

        public XOPFunction[] Functions
        {
            get { return _functions; }
        } 

    }
}
