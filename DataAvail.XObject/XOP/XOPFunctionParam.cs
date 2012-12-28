using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;



/*
 * Тэг	Param
Аттрибуты	name, type, mappedColumn, , direction, value
Вложенные тэги	
Класс	XOPFunctionParam
Описание	Связывает параметр функции и поле таблицы, также может использоваться для передачи контантного значения в функию либо получения возвращаемого значения.

Аттрибуты
Имя 	Описание

 * name*	Имя параметра функции.

 * type	Тип параметра.
Формат: тип[,размер]
Приеры: type=”int”  type=”string,50”

 * mappedColumn	Имя поля таблицы ассоциированного с параметром. 

 * direction	Направление параметра.
Значения: In, Out, ReturnValue (** In)

 * value	Контстантное значение (может быть использованно если в функцию необходимо передать константу)

 */

namespace DataAvail.XObject.XOP
{
    public class XOPFunctionParam
    {
        public XOPFunctionParam(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _name = reader.ReadAttribute("name");

            _paramType = new XOPFieldType(XElement, false);

            _mappedColumn = reader.ReadAttribute("mappedColumn");

            _direction = reader.ReadAttributeEnum("direction", System.Data.ParameterDirection.Input);

            _value = reader.ReadAttributeValue("value", _paramType.Type);
        }

        private readonly string _name;

        private readonly XOPFieldType _paramType;

        private readonly string _mappedColumn;

        private readonly System.Data.ParameterDirection _direction;

        private readonly object _value;

        public string Name
        {
            get { return _name; }
        }

        public XOPFieldType ParamType
        {
            get { return _paramType; }
        }

        public string MappedColumn
        {
            get { return _mappedColumn; }
        }


        public System.Data.ParameterDirection Direction
        {
            get { return _direction; }
        }

        public object Value
        {
            get { return _value; }
        } 

    }
}
