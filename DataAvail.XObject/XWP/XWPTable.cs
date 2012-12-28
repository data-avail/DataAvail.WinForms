using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 *Тэг	Table
Аттрибуты	tableName, caption, itemCaption, separateSerialization, saveMode, childSaveMode
Вложенные тэги	Table
Класс	XWPDataView
Описание	Описывает поведение и визуальное представление таблицы на интерфейсе пользователя.


Влияние аттрибутов на интерфейс пользователя. 
Имя 	Описание

 * tableName	Имя таблицы для которой определяются парметры. 

 * сaption	Заголовок таблицы. Будет отображен как заголовок формы списка записей.

 * itemCaption	Заголовок записи таблицы. Будет отображен как заголовок формы записи.

 * saveMode	Режим сохранения. 

 * Значения: cache | repository.
 Определяет режим записи данных  приложения.
Если cache определен то возможна запись в кэш приложения это значит что пользователь может сохранять измения нескольких записей в память программы без их апдейта в базу данных, в последствии эти отложенные изменения могут быть сохранены в БД все за раз.
Если repository определен то запись данных возможна только непосредственно в БД.  (*repository)

 * saveChildMode	Режим сохранения дочерних записей для таблицы.  
Значения:
cache | repository.
Работает так же как и параметр saveMode. Определяется сразу для всех дочерних таблиц данной таблицы. Возможно переопределить  режим сохранения для отдельно взятой дочерней таблицы при помощи вложенного тэга TABLE c заданным именем дочерне таблицы и аттрибутом параметра сохранения. Пример:
<Table name=”parentTableName” childSaveMode=”cache”>
<Table name=” childTableName”  saveMode=”repository”/>
</Table>
(*cahe|repository)

 * separateSerialization	Настройки лэйаута формы в режиме контекста по умолчанию будут сохраняться в файл отличный от других контекстов.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPTable
    {
        public XWPTable(XWPDataView XWPDataView, XElement XElement)
        {
            _xWPDataView = XWPDataView;

            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _tableName = reader.ReadAttribute("name", true);

            _caption = reader.ReadAttribute("caption");

            _itemCaption = reader.ReadAttribute("itemCaption");

            _separateSerialization = reader.ReadAttributeBool("separateSerialization", true);

            _saveMode = reader.ReadAttributeEnumFlags("saveMode", XWPTableSaveMode.All, XWPTableSaveMode.Default);

            _childSaveMode = reader.ReadAttributeEnumFlags("childSaveMode", XWPTableSaveMode.All, XWPTableSaveMode.Default);

            _fields = reader.GetChildren("Fields", new Range(0, 1)).Select(p => new XWPFields(this, p)).FirstOrDefault();
        }

        private readonly XWPDataView _xWPDataView;

        private readonly string _tableName;

        private readonly string _caption;

        private readonly string _itemCaption;

        private readonly bool _separateSerialization;

        private readonly XWPTableSaveMode _saveMode;

        private readonly XWPTableSaveMode _childSaveMode;

        private readonly XWPFields _fields;

        public XWPDataView XWPDataView
        {
            get { return _xWPDataView; }
        } 

        public string TableName
        {
            get { return _tableName; }
        }

        public string Caption
        {
            get { return _caption; }
        }

        public string ItemCaption
        {
            get { return _itemCaption; }
        }

        public XWPTableSaveMode SaveMode
        {
            get { return _saveMode; }
        }

        public XWPTableSaveMode ChildSaveMode
        {
            get { return _childSaveMode; }
        }

        public XWPFields Fields
        {
            get { return _fields; }
        }

        public bool SeparateSerialization
        {
            get { return _separateSerialization; }
        } 
       
    }

    [Flags]
    public enum XWPTableSaveMode
    { 
        Default = -1,
        Cache = 0x01,
        Repository = 0x02,
        All = Cache | Repository
    }

}
