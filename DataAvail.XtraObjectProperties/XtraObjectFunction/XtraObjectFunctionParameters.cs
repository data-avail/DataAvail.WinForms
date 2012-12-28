using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XtraObjectProperties
{
    public class XtraObjectFunctionParameters : IEnumerable<XtraObjectFunctionParameter>
    {
        internal void AddParam(XtraObjectFunctionParameter Param)
        {
            _params.Add(Param);
        }

        private List<XtraObjectFunctionParameter> _params = new List<XtraObjectFunctionParameter>();

        #region IEnumerable<XtraObjectFunctionParameter> Members

        public IEnumerator<XtraObjectFunctionParameter> GetEnumerator()
        {
            return _params.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _params.GetEnumerator();
        }

        #endregion
    }
}
