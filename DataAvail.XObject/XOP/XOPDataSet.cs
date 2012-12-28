using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;
/*
 Тэг	DataSet
Аттрибуты	provider, connectionString, providerPath, shemeName, useDefaultCommands, defaultTablePrefix, typesConverter
Вложенные тэги	Tables[1], Relations[1]
Класс	XOPDataSet
Описание	Определяет по каким правилам будет сформирован кэш данных программы, через какие интерфейсы эти данные будут взаимодействовать с БД.
Аналог БД	Database
В [] для вложенных тэгов задается их минимальное-максимальное колличество
Аттрибуты
Имя 	Описание
adapter	Имя стандартного адаптера для работы с БД. 
Значения: SQLite, MSSQL, Oracle
 * 
connectionString	Строка для коннекта с БД должна быть заданна в специфичном для adapter
формате. Если необходимо при запуске приложения вывести форму соединения с базой данных, необходимо использовать  строку соединения в стандартном для данной формы формате.

 * adapterPath	Путь до библиотеки (.dll) имплементирующий adapter.
Должен быть задан либо аттрибут adapter либо adapterPath, если заданы обы будет использован adapter.

 * schemeName	Имя схемы БД которая будет автоматически применена для каждой таблицы и функции если явно не задан специфичный. 

 * useDefaultCommands	Данные для каждой таблицы могут сохраняться либо через стандартные комманды БД : SELECT, INSERT, UPDATE, DELETE либо через специально определенные для данной таблицы функции*.  Данный параметр определяет каким механизмом пользоваться - если true, то используются стандартные комманды для всех таблиц где явно не определенно обратное (даже если функции определены для таблицы) (см defaultCommands); если false то используются функции, при этом если функция не задана то комманда выполнятся не будет. (**true)

 * typesConverter	Формат исходныйТип=конвертируемыйТип
Пример: typesConverter =”int=long” при создании струтуры данных в кэше программы все столбцы описаные с типом int, будут сгенерены с типом long
Для чего нужно: В некоторых случаях можно использовать один и тотже файл описывающие мета-данные 1-го типа, для различных типов БД, при этом очевидно что структура данных в БД должна быть одинакова, за исключением некоторых типов полей. Например в базе SQL можно использовать тип Int для первичных ключей в то время как в SQLite они могут иметь только тип long. В отсальном структура БД может совпадать для решения такого случая может быть использован данный аттрибут.
 */

namespace DataAvail.XObject.XOP
{
    public class XOPDataSet
    {
        public XOPDataSet(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _adapterPath = reader.ReadAttribute("adapterPath");

            _adapterType = reader.ReadAttributeEnum<XOPDataSetAdapterType>("adapterType", XOPDataSetAdapterType.Custom);

            if (string.IsNullOrEmpty(_adapterPath) && _adapterType == XOPDataSetAdapterType.Custom)
            {
                XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, "Either adapterType or adapterPath should be set for DataSet", true);
            }

            _connectionString = reader.ReadAttribute("connectionString", true);

            _schemeName = reader.ReadAttribute("schemeName");

            _useStdCommands = reader.ReadAttributeBool("useStdCommands", true);

            _tables = reader.GetChildren("Table", new Range(1, -1)).Select(p => new XOPTable(p)).ToArray();

            _relations = reader.GetChildren("Relations", new Range(-1, 1)).Select(p=>new XOPRelations(p)).FirstOrDefault();

        }

        private readonly string _adapterPath;

        private readonly XOPDataSetAdapterType _adapterType;

        private readonly string _connectionString;

        private readonly string _schemeName;

        private readonly bool _useStdCommands;

        private readonly XOPTable[] _tables;

        private readonly XOPRelations _relations;

        public string AdapterPath
        {
            get { return _adapterPath; }
        }

        public XOPDataSetAdapterType AdapterType
        {
            get { return _adapterType; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public string SchemeName
        {
            get { return _schemeName; }
        }

        public bool UseStdCommands
        {
            get { return _useStdCommands; }
        }

        public XOPTable[] Tables
        {
            get { return _tables; }
        }

        public XOPRelations Relations
        {
            get { return _relations; }
        } 

    }

    public enum XOPDataSetAdapterType
    {
        Custom,
        SqlServer,
        SQLite,
        Oracle
    }
}
