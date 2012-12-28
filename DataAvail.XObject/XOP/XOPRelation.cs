using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;


/*
 * Тэг	Relation
Аттрибуты	 parentTable, parentColumn, childTable, childColumn, filter, defaultValues
Вложенные тэги	
Класс	XOPRelation
Описание	Определяет связь Child – Parent между двумя таблицами в БД.

Если для определенного столбца задана foreign key связь и он является парент столбцом, то при построении интерфейса пользователя для такого поля будет создан контрол типа комбо бокс (по умолчанию), содержащий список дочерних значений.
Аттрибуты

 * Имя 	Описание

 * parentTable*	Имя парент таблицы.

 * parentColumn*	Имя парент столбца.

 * childTable*	Имя дочерней таблицы.

 * childColumn*	Имя дочернего столбца.

 * filter	Фильтр определенный для связи таблиц. Для правильной работы связей необходимо только чтобы значения парент столбцов было уникально для высталенного фильтра.

 * defaultValues	Если для данной связи выставлен фильтр, необходимо задать значения по умолчанию при добавлении записи, для того чтобы она гарантированно попала в текущий фильтр.
Формат: “ИмяСтолбца=Значение,[…]”

Влияние аттрибутов на интерфейс пользователя. 
Имя 	Описание
filter	Если фильтр задан то в интерфейсе пользователя будут показываться только те записи которые соответствуют текущему фильтру.

См также View…Realtion

 * 
 */

namespace DataAvail.XObject.XOP
{
    public class XOPRelation
    {
        public XOPRelation(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _name = reader.ReadAttribute("name", true);

            _parentTable = reader.ReadAttribute("parentTable", true);

            _parentField = reader.ReadAttribute("parentField", true);

            _childTable = reader.ReadAttribute("childTable", true);

            _childField = reader.ReadAttribute("childField", true);

            _filter = reader.ReadAttribute("filter");

            _defaultValues = reader.ReadAttribute("defaultValues");

            if (_defaultValues == null && !string.IsNullOrEmpty(Filter))
            {
                _defaultValues = Filter.Replace("'", "").Replace(" AND ", ","); 
            }

        }

        private readonly string _name;

        private readonly string _parentTable;

        private readonly string _parentField;

        private readonly string _childTable;

        private readonly string _childField;

        private readonly string _filter;

        private readonly string _defaultValues;

        public string Name
        {
            get { return _name; }
        }

        public string ParentTable
        {
            get { return _parentTable; }
        }

        public string ParentField
        {
            get { return _parentField; }
        }

        public string ChildTable
        {
            get { return _childTable; }
        }

        public string ChildField
        {
            get { return _childField; }
        } 

        public string Filter
        {
            get { return _filter; }
        }

        public string DefaultValues
        {
            get { return _defaultValues; }
        } 




        
    }
}
