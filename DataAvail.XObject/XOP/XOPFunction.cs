using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
Тэг	Function
Аттрибуты	name, type
Вложенные тэги	Param[1..]
Класс	XOPFunction
Описание	Определяет механизм взаимодействия кэша данных программы для отдельной таблицы БД.


Аттрибуты
Имя 	Описание

 * name*	Имя объекта (хранимой процедуры или функции) в БД

 * Type*	Значения: select, insert, update, delete
Определяет для какой комманды будет вызывться эта функция (отбор, вставка, изменение или удаление)

Внимание если для таблицы было задано что необходимо использовать стандартную комманду (см defaultCommands), данная функция использована не будет!
 */

namespace DataAvail.XObject.XOP
{
    public class XOPFunction
    {
        public XOPFunction(XElement XElement)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _name = reader.ReadAttribute("name", true);

            _commandType = reader.ReadAttributeEnumFlags("type", XOPTableCommandType.All, XOPTableCommandType.None);

            _params = reader.GetChildren("Param", new Range(1, -1)).Select(p => new XOPFunctionParam(p)).ToArray();  

            if (!(CommandType == (XOPTableCommandType.Insert | XOPTableCommandType.Update)))
            {
                if (_commandType == XOPTableCommandType.None)
                {
                    XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, string.Format("Attribute commantType of command {0} is None. Command won't be used in project.", _name), false);
                }
                else if (EnumFlags.IsContainMixed(_commandType, XOPTableCommandType.Select))
                {
                    XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, string.Format("Attribute commantType of command {0} mixes Select and another command. Command of this type can't be mixed, will be used as Select.", _name), false);
                }
                else if (EnumFlags.IsContainMixed(_commandType, XOPTableCommandType.Delete))
                {
                    XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, string.Format("Attribute commantType of command {0} mixes Delete and another command. Command of this type can't be mixed, will be used as Delete.", _name), false);
                }
                else if (EnumFlags.IsContainMixed(_commandType, XOPTableCommandType.Insert))
                {
                    XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, string.Format("Attribute commantType of command {0} mixes Insert and command of type diffrent from Update. Command of this type can't be mixed, will be used as Insert.", _name), false);
                }
                else if (EnumFlags.IsContainMixed(_commandType, XOPTableCommandType.Update))
                {
                    XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, string.Format("Attribute commantType of command {0} mixes Update and command of type diffrent from Insert. Command of this type can't be mixed, will be used as Update.", _name), false);
                }
            }
        }

        private readonly string _name;

        private readonly XOPTableCommandType _commandType;

        private readonly XOPFunctionParam [] _params;

        public string Name
        {
            get { return _name; }
        }

        public XOPTableCommandType CommandType
        {
            get { return _commandType; }
        }

        public XOPFunctionParam [] Params
        {
            get { return _params; }
        } 

    }
}
