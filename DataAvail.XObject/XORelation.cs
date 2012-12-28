using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Utils;
using DataAvail.XObject.XOP;
using DataAvail.XObject.XWP;
using DataAvail.XObject.XSP;

namespace DataAvail.XObject
{
    public class XORelation
    {
        internal XORelation(XOTable XOTable, XOPRelation XOPRelation, XWPRelation XWPRelation)
        {
            _xoTable = XOTable;

            _xopRelation = XOPRelation;

            _xwpRelation = XWPRelation;
        }

        private readonly XOTable _xoTable;

        private readonly XOPRelation _xopRelation;

        private readonly XWPRelation _xwpRelation;

        IDictionary<string, object> _defaultValues;

        internal void EndInit()
        {

            if (!string.IsNullOrEmpty(this.XopRelation.DefaultValues))
            {
                var r = (from p in this.XopRelation.DefaultValues.Split(',')
                         let pair = p.Split('=').ToArray()
                         select new { key = pair[0].Trim(), value = pair[1].Trim() }).Select(
                   p => new { k = p.key, v = Reflection.Parse(this.XoTable.Fields.Single(s => s.Name == p.key).FieldType, p.value) });

                _defaultValues = r.ToDictionary(k => k.k, v => v.v);
            }
        }

        public XOTable XoTable
        {
            get { return _xoTable; }
        }

        public XOPRelation XopRelation
        {
            get { return _xopRelation; }
        }

        public XWPRelation XwpRelation
        {
            get { return _xwpRelation; }
        }

        public IDictionary<string, object> DefaultValues
        {
            get { return _defaultValues; }
        }

        public XOTable ParentTable
        {
            get { return XoTable.XoApplication.Tables.Single(p => p.Name == XopRelation.ParentTable); }
        }

        public XOField ParentField
        {
            get { return ParentTable.Fields.Single(p => p.Name == XopRelation.ParentField); }
        }

        public XOTable ChildTable
        {
            get { return XoTable.XoApplication.Tables.Single(p => p.Name == XopRelation.ChildTable); }
        }

        public XOField ChildField
        {
            get { return ChildTable.Fields.Single(p => p.Name == XopRelation.ChildField); }
        }

        public string RelationName
        {
            get { return XopRelation.Name; }
        }

        public string SerializationName
        {
            get { return XwpRelation != null ? XwpRelation.SerializationName : null; }
        }

        public bool IsShown
        {
            get { return XwpRelation != null ? XwpRelation.IsShown : false; }
        }

        public string DisplayedField
        {
            get
            {
                if (ChildField.XwpField != null)
                {
                    if (!string.IsNullOrEmpty(ChildField.XwpField.ParentDisplayField))
                        return ChildField.XwpField.ParentDisplayField;

                    if (!string.IsNullOrEmpty(ChildField.XwpField.XWPFields.ParentDisplayField))
                        return ChildField.XwpField.XWPFields.ParentDisplayField;

                    if (!string.IsNullOrEmpty(ChildField.XwpField.XWPFields.XWPTable.XWPDataView.ParentDisplayField))
                        return ChildField.XwpField.XWPFields.XWPTable.XWPDataView.ParentDisplayField;

                }

                XOField field = ParentTable.Fields.FirstOrDefault(p => p.FieldType == typeof(string));

                if (field != null)
                    return field.Name;

                return ParentTable.Fields.First().Name;
            }
        }

        public string Filter
        {
            get { return XopRelation.Filter; }
        }
    }
}
