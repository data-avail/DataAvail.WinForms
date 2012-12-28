using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraBinding.Controllers.Binding
{
    internal class ControllerDataSourceControl : IControllerDataSourceControl
    {
        internal ControllerDataSourceControl()
        {
        }

        private object _dataSource;

        private string _valueMember;

        private string _displayMember;

        private string _filter;

        #region IControllerDataSourceControl Members

        public object DataSource
        {
            get
            {
                return _dataSource;
            }

            set
            {
                _dataSource = value;
            }
        }

        public string ValueMember
        {
            get
            {
                return _valueMember;
            }
            set
            {
                _valueMember = value;
            }
        }

        public string DisplayMember
        {
            get
            {
                return _displayMember;
            }
            set
            {
                _displayMember = value;
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
            }
        }

        #endregion
    }
}
