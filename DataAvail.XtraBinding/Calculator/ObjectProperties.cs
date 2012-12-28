using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;

namespace DataAvail.XtraBindings.Calculator
{
    public class ObjectProperties : DataAvail.Utils.Collections.KeyDictDict
    {
        internal ObjectProperties(XOTableContext TableContext, IDictionary<string, object> FieldValues)
        {
            tableContext = TableContext;

            /*
            objectName = XOTableContext.Name;

            pkField = XOTableContext.PkFieldName;// AppItemContext.Container.PKField.FieldName;

            Caption = XOTableContext.Caption;

            ReadOnly = XOTableContext.IsCanEdit;//!AppItemContext.IsCanEdit;

            _fields = XOTableContext.Fields.Select(p => p.Name).ToArray(); ;// AppItemContext.Fields.Select(p => p.FieldName).ToArray();
             */

            foreach (string fieldName in Fields)
            {
                SetValue(fieldName, FieldValues[fieldName]);
            }
        }

        public readonly XOTableContext tableContext;

        public const string VALUE_PROP_NAME = "value";

        public const string READONLY_PROP_NAME = "readOnly";

        public const string STATE_PROP_NAME = "state";

        public const string ERROR_PROP_NAME = "error";

        public const string FILTER_PROP_NAME = "filter";

        public const string MASK_PROP_NAME = "mask";

        private string _caption = null;

        private bool _readOnly = false;

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public IEnumerable<string> Fields
        {
            get { return tableContext.Fields.Select(p=>p.Name); }
        }

        public void SetError(string FieldName, string Error)
        {
            this[ERROR_PROP_NAME, FieldName] = Error;
        }

        public object GetError(string FieldName)
        {
            return this[ERROR_PROP_NAME, FieldName];
        }

        public void SetValue(string FieldName, object Value)
        {
            this[VALUE_PROP_NAME, FieldName] = Value;
        }

        public object GetValue(string FieldName)
        {
            return this[VALUE_PROP_NAME, FieldName];
        }

        public void SetReadOnly(string FieldName, bool ReadOnly)
        {
            this[READONLY_PROP_NAME, FieldName] = ReadOnly;
        }

        public object GetReadOnly(string FieldName)
        {
            return this[READONLY_PROP_NAME, FieldName];
        }

        public void SetState(string FieldName, string StateName)
        {
            this[STATE_PROP_NAME, FieldName] = StateName;
        }

        public string GetState(string FieldName)
        {
            return (string)this[STATE_PROP_NAME, FieldName];
        }

        public void SetStateTyped(string FieldName, CalulatorFieldState FieldState)
        {
            SetState(FieldName, FieldState.ToString());
        }

        public CalulatorFieldState GetStateTyped(string FieldName)
        {
            string state = GetState(FieldName);

            if (state != null)
                return (CalulatorFieldState)System.Enum.Parse(typeof(CalulatorFieldState), state);
            else
                return CalulatorFieldState.None;
        }

        public void SetMask(string FieldName, string StateName)
        {
            this[MASK_PROP_NAME, FieldName] = StateName;
        }

        public string GetMask(string FieldName)
        {
            return (string)this[MASK_PROP_NAME, FieldName];
        }

        public void SetFilter(string FieldName, string StateName)
        {
            this[FILTER_PROP_NAME, FieldName] = StateName;
        }

        public string GetFilter(string FieldName)
        {
            return (string)this[FILTER_PROP_NAME, FieldName];
        }

        public void MergeValues(IDictionary<string, object> KeyValuePairs)
        {
            foreach (KeyValuePair<string, object> kvp in KeyValuePairs)
            {
                this[VALUE_PROP_NAME].value[kvp.Key] = kvp.Value;
            }
        }

        public void Merge(ObjectProperties ObjectProperties)
        {
            foreach (var r in ObjectProperties)
            {
                foreach (var i in r.value.value)
                {
                    if (i.value != null)
                        this[r.key, i.key] = i.value;
                }
            }
        }


        #region Helpers

        /// <summary>
        /// GetValue
        /// </summary>
        public object GV(string FieldName)
        {
            return this.GetValue(FieldName);
        }

        /// <summary>
        /// SetValue
        /// </summary>
        public void SV(string FieldName, object Value)
        {    
            this.SetValue(FieldName, Value);
        }

        /// <summary>
        /// SetReadOnly
        /// </summary>
        public void SR(string FieldName, bool ReadOnly)
        {
            this.SetReadOnly(FieldName, ReadOnly);
        }

        /// <summary>
        /// SetValue + change state to Calculated
        /// </summary>
        public void SVC(string FieldName, object Value)
        {
            this.SetValue(FieldName, Value);
            this.SS(FieldName, CalulatorFieldState.Calculated);
        }


        /// <summary>
        /// SetStateTyped
        /// </summary>
        public void SS(string FieldName, CalulatorFieldState FieldState)
        {
            this.SetStateTyped(FieldName, FieldState);
        }

        /// <summary>
        /// GetStateTyped
        /// </summary>
        public CalulatorFieldState GS(string FieldName)
        {
            return this.GetStateTyped(FieldName);
        }

        /// <summary>
        /// SetFilter
        /// </summary>
        public void SF(string FieldName, string Value)
        {
            this.SetFilter(FieldName, Value);
        }

        /// <summary>
        /// GetFilter
        /// </summary>
        public string GF(string FieldName)
        {
            return this.GetFilter(FieldName);
        }


        /// <summary>
        /// SetMask
        /// </summary>
        public void SM(string FieldName, string Value)
        {
            this.SetMask(FieldName, Value);
        }


        public double AsDbl(string FieldName)
        {
            object val = GetValue(FieldName);

            return val != System.DBNull.Value ? Convert.ToDouble(val) : 0;
        }

        public int AsInt(string FieldName)
        {
            object val = GetValue(FieldName);

            return val != System.DBNull.Value ? Convert.ToInt32(val) : 0;
        }


        #endregion
    }
}
