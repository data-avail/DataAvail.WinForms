using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DataAvail.Controllers.Binding
{
    /// <summary>
    /// Controller control properties
    /// </summary>
    public class CCProperties : INotifyPropertyChanged
    {
        bool _readOnly;

        System.Drawing.Color _backColor;

        object _dataSource;

        string _dataSourceFilter;

        string _mask;

        public bool ReadOnly
        {
            get { return _readOnly; }

            set 
            {
                if (ReadOnly != value)
                {
                    _readOnly = value;

                    OnPropertyChanged("ReadOnly");

                    OnReadOnlyChanged();
                }
            }
        }

        public System.Drawing.Color BackColor
        {
            get { return _backColor; }
            
            set 
            {
                if (BackColor != value)
                {
                    _backColor = value;

                    OnPropertyChanged("BackColor");

                    OnBackColorChanged();
                }
            }
        }

        public virtual object DataSource
        {
            get { return _dataSource; }
            
            set 
            {
                if (DataSource != value)
                {
                    _dataSource = value;

                    OnPropertyChanged("DataSource");

                    OnDataSourceChanged();
                }
            }
        }

        public virtual string DataSourceFilter
        {
            get { return _dataSourceFilter; }

            set 
            {
                if (DataSourceFilter != value)
                {
                    _dataSourceFilter = value;

                    OnPropertyChanged("DataSourceFilter");

                    OnDataSourceFilterChanged();
                }
            }
        }

        public virtual string Mask
        {
            get { return _mask; }

            set 
            {
                if (Mask != value)
                {
                    _mask = value;

                    OnPropertyChanged("Mask");

                    OnMaskChanged();
                }
            
            }
        }

        protected virtual void OnReadOnlyChanged()
        {
        }

        protected virtual void OnBackColorChanged()
        { }

        protected virtual void OnDataSourceChanged()
        { }

        protected virtual void OnDataSourceFilterChanged()
        { }

        protected virtual void OnMaskChanged()
        { }

        public void Assign(CCProperties CCProperties)
        {
            this.ReadOnly = CCProperties.ReadOnly;

            this.BackColor = CCProperties.BackColor;

            this.DataSource = CCProperties.DataSource;

            this.DataSourceFilter = CCProperties.DataSourceFilter;

            this.Mask = CCProperties.Mask;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(string PropertyName)
        { 
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
