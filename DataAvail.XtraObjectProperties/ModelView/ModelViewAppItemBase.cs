using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DataAvail.Utils;
using DataAvail.Utils.XmlLinq;

namespace DataAvail.XOP.ModelView
{
    public class ModelViewAppItemBase
    {
        internal ModelViewAppItemBase()
        { 
        }

        internal ModelViewAppItemBase(ModelViewAppItemBase Parent, XElement XAction)
        {
            _parent = Parent;

            Parse(XAction);
        }

        private readonly ModelViewAppItemBase _parent;

        private DataAvail.Utils.EditModeType _mode = DataAvail.Utils.EditModeType.Default;

        private SaveMode _saveMode = SaveMode.Default;

        private SaveMode _childSaveMode = SaveMode.Default;

        internal virtual void Parse(XElement XAction)
        {
            _mode = EditMode.Parse(XmlLinq.GetAttribute(XAction, "mode"));

            string str = XmlLinq.GetAttribute(XAction, "saveMode");

            if (!string.IsNullOrEmpty(str))
            {
                _saveMode = DataAvail.Utils.EnumFlags.ComposeEnum(str.Split('|').Select(p => ParseSaveMode(p.Trim())));
            }

            str = XmlLinq.GetAttribute(XAction, "childSaveMode");

            if (!string.IsNullOrEmpty(str))
            {
                _childSaveMode = DataAvail.Utils.EnumFlags.ComposeEnum(str.Split('|').Select(p => ParseSaveMode(p.Trim())));
            }
        }
        private SaveMode ParseSaveMode(string Mode)
        {
            switch (Mode)
            {
                case "cache":
                    return SaveMode.Cache;
                case "repository":
                    return SaveMode.Repository;
                case "all":
                    return SaveMode.All;
            }

            return SaveMode.Default;
        }

        public ModelViewAppItemBase Parent
        {
            get { return _parent; }
        }

        public EditModeType Mode
        {
            get { return _mode; }
        }

        public SaveMode SaveMode
        {
            get { return _saveMode; }
        }

        public SaveMode ChildSaveMode
        {
            get { return _childSaveMode; }
        }
    }

    [Flags]
    public enum SaveMode
    {
        Default = 0x00,
        Repository = 0x01,
        Cache = 0x02,
        All = Repository | Cache
    }

}
