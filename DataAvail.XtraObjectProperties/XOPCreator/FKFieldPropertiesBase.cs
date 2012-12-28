using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties.XOPCreator
{
    public abstract class FKFieldPropertiesBase : XtraTextFieldProperties, IFKFieldProperties
    {
        public FKFieldPropertiesBase(XtraFieldProperties XtraFieldProperties)
            : base(XtraFieldProperties)
        {
        }

        private string _valueMember;

        private string _displayMember;

        private string _filter;

        private object _dataSource;

        public string ValueMember
        {
            get { return _valueMember; }

            set 
            {
                if (_valueMember != value)
                {
                    _valueMember = value;

                    OnNotifyPropertyChanged("ValueMember");
                }
            
            }
        }

        public string DisplayMember
        {
            get { return _displayMember; }

            set 
            {
                if (_displayMember != value)
                {
                    _displayMember = value;

                    OnNotifyPropertyChanged("DisplayMember");
                }
            }
        }

        public virtual object DataSource
        {
            get { return _dataSource; }
        }

        protected void SetDataSource(object DataSource, string Filter)
        {
            if (_dataSource != DataSource || _filter != Filter)
            {
                _dataSource = DataSource;

                _filter = Filter;

                OnNotifyPropertyChanged("DataSource");
            }
        }


        public string Filter
        {
            get { return _filter; }
        }
    }
}
