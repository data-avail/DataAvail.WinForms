using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils.XmlLinq;

/*
 *Тэг	DataTable
Аттрибуты	 name, persistFill, autoFill, source, sourceUpdate, defaultCommands, schemeName
Вложенные тэги	Functions[1], Columns[1..]
Класс	XOPDataTable
Описание	Описывает структуру  в которой будут храниться кэш данные из таблицы БД и через какие интерфейсы эти данные будут взаимодействовать с БД.
Аналог 	

Аттрибуты

 * Имя 	Описание

 * name*	Имя таблицы, в общем случае соответствует имени таблицы или вью из базы данных. Также по этому аттрибуту другие элементы мета-данных приложения связываются с таблицей(см Relation, Table, MenuItem, TableSecurity)

 * source	Фактическое имя таблицы или вью к которым  привязана данная таблица (см что такое привязана) (если не заданно то будет инициализированно со значением name)

 * sourceUpdate	Фактическое имя элемента БД через который будет происходить сохранение данных в БД. 
Пример:  необходимо считывать данные из вью и сохранить измененные данные в таблицу (через стандартные команды). В этом случае все поля содержащиеся во вью но не пренедлежащие таблице должны быть readOnly также 
source = “View Name”  sourceUpdate=”Table Name”
(если не заданно то будет инициализированно со значением source)

 * schemeName	Имя схемы БД. 

 * autoFill	Если true - таблица заполняется при запуске приложения (** true)

 * persistFill	Если true – данные таблицы могут быть отобранны пользователем только полностью, без использования фильтров. В этом случае если пользователь будет отбирать данные через панель поиска фильтр будет выставлятся на грид данных , отбор данных из БД производится не будет. Если пользователь вызавет команду переотбора данных, все данные принадлежащие данной таблице будут переотобраны. (** true)

 * schemeName	Имя схемы БД для таблицы. Схема также может быть задана непосредственно в имени источника данных стандартным образом [SchemeName].[TableName]

 * defaultCommands	Задает какие стандартные комманды будут использованы при взаимодействии с БД 
Значения : select|insert|update|delete
Пример:  defaultCommands = “update|select”
В этом случае для отбора  данных из БД будет использоваться стандартная комманда SELECT, а для изменения записи стандартная UPDATE  (обе комманды будут сгенерены по правилам определенными в мета-данных для полей таблицы); для вставки и отбора записей будут использованы функции определенные для данной таблицы, (внимательно, каждой комманде соответствует свой тип функции) если такой функции не задано то комманда выполняться не будет и интерфейс пользователя для данной комманды будет заблокирован

 */

namespace DataAvail.XObject.XOP
{
    public class XOPTable
    {
        public XOPTable(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _name = reader.ReadAttribute("name", true);

            _source = reader.ReadAttribute("source");

            _sourceUpdate = reader.ReadAttribute("sourceUpdate");

            _schemeName = reader.ReadAttribute("schemeName");

            _autoFill = reader.ReadAttributeBool("autoFill", true);

            _persistFill = reader.ReadAttributeBool("persistFill", true);

            _stdCommandsType = reader.ReadAttributeEnumFlags<XOPTableCommandType>("stdCommandsType", XOPTableCommandType.All, XOPTableCommandType.Default);

            XElement xFields = reader.GetChildren("Fields", new DataAvail.Utils.Range(1)).FirstOrDefault();

            _fields = new XOPFields(xFields);

            XElement xFuncs = reader.GetChildren("Functions", new DataAvail.Utils.Range(0, 1)).FirstOrDefault();

            _functions = new XOPFunctions(xFuncs);

        }

        private readonly string _name;

        private readonly string _source;

        private readonly string _sourceUpdate;

        private readonly string _schemeName;

        private readonly bool _autoFill;

        private readonly bool _persistFill;

        private readonly XOPTableCommandType _stdCommandsType;

        private readonly XOPFields _fields;

        private readonly XOPFunctions _functions;

        public string Name
        {
            get { return _name; }
        }

        public string Source
        {
            get { return _source; }
        }

        public string SourceUpdate
        {
            get { return _sourceUpdate; }
        }

        public string SchemeName
        {
            get { return _schemeName; }
        }

        public bool AutoFill
        {
            get { return _autoFill; }
        }

        public bool PersistFill
        {
            get { return _persistFill; }
        }

        public XOPTableCommandType StdCommandsType
        {
            get { return _stdCommandsType; }
        }

        public XOPFields Fields
        {
            get { return _fields; }
        }

        public XOPFunctions Functions
        {
            get { return _functions; }
        } 
    }

    [Flags]
    public enum XOPTableCommandType
    { 
        Default = -1,
        None = 0x0,
        Select = 0x01,
        Insert = 0x02,
        Update = 0x04,
        Delete = 0x08,
        All = Select | Insert | Update | Delete
    }

}
