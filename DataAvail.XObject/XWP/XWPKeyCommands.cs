using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 * Тэг	Keys
Аттрибуты	
Вложенные тэги	Key
Класс	XWPKeys
Описание	Определяет набор горячих клавиш для работы с различными командами приложения.

 */

namespace DataAvail.XObject.XWP
{
    public class XWPKeyCommands
    {
        public XWPKeyCommands(XElement XElement)
        {
            _keyCommands = XmlLinq.GetChildren(XElement, "Key", new Range(1, -1), XOApplication.xmlReaderLog).Select(p => new XWPKeyCommand(p, true)).ToArray();
        }

        private readonly XWPKeyCommand [] _keyCommands;

        public XWPKeyCommand[] KeyCommands
        {
            get { return _keyCommands; }
        } 

    }
}
