using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Controllers.Binding
{
    public interface IControllerDataSourceControl
    {
        ControllerDataSourceProperties DataSourceProperties { get; set; }
    }

    public class ControllerDataSourceProperties
    {
        private object _dataSource;

        private string _valueMember;

        private string _displayMember;

        private string _filter;
            
        public object DataSource 
        {
            set 
            {
                if (DataSource != value)
                {
                    _dataSource = value;

                    OnDataSourceChanged();
                }
            
            }

            get { return _dataSource; }
        }

        public string ValueMember 
        {
            set 
            {
                if (ValueMember != value)
                {
                    _valueMember = value;

                    OnValueMemberChanged();
                }
            
            }

            get { return _valueMember; }
        }

        public string DisplayMember 
        {
            set 
            {
                if (DisplayMember != value)
                {
                    _displayMember = value;

                    OnDisplayMemberChanged();
                }
            
            }

            get { return _displayMember; }
        }

        public string Filter 
        {
            set 
            {
                if (Filter != value)
                {
                    _filter = value;

                    OnFilterChanged();
                }
            }

            get { return _filter; }
        }

        public void Assign(ControllerDataSourceProperties Src)
        {
            this.DataSource = Src.DataSource;

            this.DisplayMember = Src.DisplayMember;

            this.ValueMember = Src.ValueMember;

            this.Filter = Src.Filter;
        }

        protected virtual void OnDataSourceChanged()
        { }

        protected virtual void OnValueMemberChanged()
        { }

        protected virtual void OnDisplayMemberChanged()
        { }

        protected virtual void OnFilterChanged()
        { }
        
    }
}
