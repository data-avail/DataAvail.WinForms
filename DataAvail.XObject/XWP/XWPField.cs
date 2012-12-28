using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;
using DataAvail.XObject;

/*
 * Тэг	Column
Аттрибуты	 caption, mask, readOnly, controlType, parentDisplayColumn,itemSelector, commands
Вложенные тэги	
Класс	XWPColumn
Описание	Описывает визульное представление и поведение поля данных на интерфейсе пользователя.


 * Имя 	Описание

 * Caption	Заголовок поля. Будет выведен как заголовок контролов о содиржащих значения данного поля. 

 * Mask	Маска значения. Определенны следующие значения.
Формат – тип маски:маска
Пример – d:G
1.Для поля типа Date (тип формата d:) :
G,g,D,d… и другие смотри полный список здесь 
http://www.csharp-examples.net/string-format-datetime/
2.Для поля типа цифровых значений (long, int, float), ( тип формата n:) :
n, n0, n2… и другие смотри полный список здесь 
http://www.csharp-examples.net/string-format-double/
http://www.csharp-examples.net/string-format-int/
3.Для типа string (тип формата r:) :
Любой формат типа RegExp, смотри описание здесь
http://www.regular-expressions.info/examples.html
Или одно из предопределенных значений:
phone, email


 * readOnly	Определяет будет ли поле доступно пользователю для редактирования.

 * controlType	Определяет какой именно контрол будет использован для редактирования значения данного поля. Кроме стандартных контролов для определенных типов полей (см type) приложение может переопределять данные типы контролов:

 * Для полей типа string : TextBoxMultiLine, TextBoxRichEdit

 * Для fioregin key полей : AutoRefCombo

 * parentDisplayColumn*	Определяет какое поле парент таблицы будет отображаться в списке выбора значений. 

 * Commands*	Определяет какие расширенные интерфейсы  для выбора  значения из списка будут доступны.
Значения:
selectItemKey, selectItemButton, addItemKey, addItemButton, selectItemKeySearch, selectButtonKeySerach

selectItemKey – если фокус пользователя будет находиться на данном контророле и пользователь нажмет горяую клавишу отурытия формы выбора дочерних значений, то появиться форма выбора дочерней записи связанная с данным полем.

selectItemButton – для контрола связанного с этим полем повиться кнопка ...
при нажатию на которую появиться форма выбора дочерней записи

addItemKey – если фокус пользователя будет находиться на данном контророле и пользователь нажмет горяую клавишу отурытия формы добавления дочерней записи, то появиться форма добавления  дочерней записи связанная с данным полем.

addItemButton – для контрола связанного с этим полем повиться кнопка +
при нажатию на которую появиться форма добавления  дочерней записи связанная с данным полем.

selectItemKeySearch – все тоже что и для selectItemKey тоолько для конролов расположенных на панели поиска.

selectItemButtonSearch – все тоже что и для selectItemButton только для конролов расположенных на панели поиска.

itemSelector*	Определяет доступные пользователю действия для формы выбора дочерних значений.
Значения: edit|add| delete (по умолчанию view доступно всегда)
edit – позволяет редактировать запись через  форму выбора дочерних значений.
add– позволяет добавлять  запись через  форму выбора дочерних значений (если определенно то edit также будет доступен).
delete – позволяет удалять  запись через  форму выбора дочерних значений.

* доступны только для родительских foreign key полей.
 */

namespace DataAvail.XObject.XWP
{
    public class XWPField
    {
        
        public XWPField(XWPFields XWPFields, XElement XElement)
        {
            _xWPFields = XWPFields;

            XmlLinqElementReader reader = new XmlLinqElementReader(XElement, XOApplication.xmlReaderLog);

            _fieldName = reader.ReadAttribute("name", true);

            _caption = reader.ReadAttribute("caption");

            _readOnly = reader.ReadAttributeBool("readOnly", false);

            _mask = new XWPFieldMask(XElement);

            _controlType = reader.ReadAttribute("controlType");

            _parentDisplayField = reader.ReadAttribute("parentDisplayField");

            _fkInterfaceType = reader.ReadAttributeEnumFlags("fkInterface", XWPFieldFkInterfaceType.All, XWPFieldFkInterfaceType.Default);

            _fkSelectItemMode = reader.ReadAttributeEnumFlags("fkSelectItemMode", XOMode.All, XOMode.View);

            _bindingField = reader.ReadAttribute("bindingProperty");

            _displayType = reader.ReadAttributeEnumFlags("displayType", XWPFieldDisplayType.Anywhere, XWPFieldDisplayType.Anywhere);
        }

        private readonly XWPFields _xWPFields;

        private readonly string _fieldName;

        private readonly string _caption;

        private readonly bool _readOnly;

        private readonly XWPFieldMask _mask;

        private readonly string _controlType;

        private readonly string _parentDisplayField;

        private readonly XWPFieldFkInterfaceType _fkInterfaceType;

        private readonly XOMode _fkSelectItemMode;

        private readonly string _bindingField;

        private readonly XWPFieldDisplayType _displayType;

        public XWPFields XWPFields
        {
            get { return _xWPFields; }
        } 

        public string FieldName
        {
            get { return _fieldName; }
        }

        public string Caption
        {
            get { return _caption; }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
        }

        public XWPFieldMask Mask
        {
            get { return _mask; }
        }

        public string ControlType
        {
            get { return _controlType; }
        }

        public string ParentDisplayField
        {
            get { return _parentDisplayField; }
        }

        public XWPFieldFkInterfaceType FkInterfaceType
        {
            get { return _fkInterfaceType; }
        }

        public XOMode FkSelectItemMode
        {
            get { return _fkSelectItemMode; }
        }

        public string BindingField 
        {
            get { return _bindingField; }
        }

        public XWPFieldDisplayType DisplayType
        {
            get { return _displayType; }
        }
    }

    [Flags]
    public enum XWPFieldFkInterfaceType
    {
        Default = -1,
        None = 0x0,
        SelectItemButton = 0x01,
        SelectItemKey = 0x02,
        AddItemButton = 0x04,
        AddItemKey = 0x08,
        SelectItemButtonSearch = 0x10,
        SelectItemKeySearch = 0x20,
        All = SelectItemButton | SelectItemKey | AddItemButton | AddItemKey | SelectItemButtonSearch | SelectItemKeySearch
    }

    [Flags]
    public enum XWPFieldDisplayType
    {
        Nowhere = 0x0,
        List = 0x01,
        Item = 0x02,
        Search = 0x04,
        Anywhere = List | Item | Search
    }

}
