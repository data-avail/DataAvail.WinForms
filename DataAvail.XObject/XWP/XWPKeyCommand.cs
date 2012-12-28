using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

/*
 Тэг	Key
Аттрибуты	 commandName, key
Вложенные тэги	
Класс	XWPKey
Описание	Определяет горячие клавиши для определенной команды прилодения.

Имя 	Описание

  commandName *	Имя комманды.

  Key*	Клавиши комманды

  Формат : [alt|shift|ctl]имя клавишы
Пример: key=”ctl|a” key=”ctl|shif|b” key=”alt|down” key=”return”

Комманды
showItem	Открыть форму редатирования записи (форма списка записей должна быть активна и фокус должен быть на гриде)
selectItem	Аналогична showItem в контексте по умолчанию. В контексте выбора дочерней записи выбирает из грида списка текущую запись и закрывает окно
addItem	Добавть запись в список (форма списка записей должна быть выбрана и фокус должен быть на гриде) 
removeItem	Удалить запись из списка (форма списка записей должна быть выбрана и фокус должен быть на гриде)
closeForm    	Закрыть форму. Закрывает любую открытую, активную форму.
saveChanges	Сохраняет изменения в БД, для текущей активной формы.
rejectChanges	Отменяет не сохраненные изменения для текущей активной формы.
movePerv	Перейти к предыдущей записи (Форма редактирования записи должна быть активна)
moveNext    	Перейти к следующей записи (Форма редактирования записи должна быть активна)
fkAddItem	Добавить дочернюю запись через для сфокусированного поля (см Column.Commands = “addFkItem”)
fkSelectItem    	Открывает форму выбора дочерней записи для сфокусированного поля (см Column.Commands = “selectFkItem”)
refreshList	Переотбирает данные таблицы из БД (форма списка записей должна быть активна)
uploadToExcel    	Выгружает данные грида в XLS файл. (форма списка записей должна быть активна)
focusSearch	Переместить фокус на панель поиска (форма списка записей должна быть активна)
focusList    	Переместить фокус на грид записей форма списка записей (должна быть активна)
endEdit    	Сохранить изменения в кэш (форма редоктирования записи должна быть активна)
cancelEdit	Отменить изменения не сохраненные в кэш (форма редоктирования записи должна быть активна)


 */

namespace DataAvail.XObject.XWP
{
    public class XWPKeyCommand
    {
        public XWPKeyCommand(XElement XElement, bool Obrigatory)
        {
            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            string keyAttr = reader.ReadAttribute("key", Obrigatory);

            if (!string.IsNullOrEmpty(keyAttr))
            {
                _keys = keyAttr.Split(';').Select(p => new XWPKey(this, p)).ToArray();
            }
            else
            {
                _keys = new XWPKey[] { };
            }
            
            _command = reader.ReadAttributeEnum("command", XWPKeyCommandType.none);

            if (_command == XWPKeyCommandType.none)
                XmlLinqReaderLog.WriteToLog(XOApplication.xmlReaderLog, XElement, "command", "Command can't have None type. It will be omitted.", false);

            _keyContexts = reader.GetChildren("KeyContext", Range.NotBound).Select(p => new XWPKeyCommandContext(this, p)).ToArray();

        }

        private readonly XWPKey[] _keys;

        private readonly XWPKeyCommandType _command;

        private readonly XWPKeyCommandContext[] _keyContexts;

        public XWPKey[] Keys
        {
            get { return _keys; }
        } 

        public XWPKeyCommandType CommandType
        {
            get { return _command; }
        }

        public XWPKeyCommandContext[] KeyContexts
        {
            get { return _keyContexts; }
        }
    }

    public enum XWPKeyCommandType
    { 
        none,
        showItem,	
        selectItem,	
        addItem,	
        removeItem,	
        closeForm,    	
        saveChanges,	
        rejectChanges,	
        movePerv,	
        moveNext,    	
        fkAddItem,	
        fkSelectItem,    	
        refreshList,	
        uploadToExcel,    	
        focusSearch,	
        focusList,    	
        endEdit,    	
        cancelEdit,
	    clone
    }
}
