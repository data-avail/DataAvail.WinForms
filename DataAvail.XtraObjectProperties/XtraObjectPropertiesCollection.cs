using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraObjectPropertiesCollection : IEnumerable<XtraObjectProperties>
    {
        internal XtraObjectPropertiesCollection(IEnumerable<XtraObjectProperties> XtraObjectProperties, DataAvail.XOP.AppContext.AppContext AppContext)
        {
            _objectProperties = XtraObjectProperties;

            _appContext = AppContext;

            foreach (XtraObjectProperties xop in XtraObjectProperties) 
                xop.SetContainer(this);
        }

        private readonly IEnumerable<XtraObjectProperties> _objectProperties;

        private readonly DataAvail.XOP.AppContext.AppContext _appContext;


        #region IEnumerable<XtraObjectProperties> Members

        public IEnumerator<XtraObjectProperties> GetEnumerator()
        {
            return _objectProperties.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _objectProperties.GetEnumerator();
        }

        #endregion

        public DataAvail.XOP.AppContext.AppContext AppContext
        {
            get
            {
                return _appContext;
            }
        }
    }
}
