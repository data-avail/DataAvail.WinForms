using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.XObject
{
    public class XOFieldCalculatorRouteRelation
    {
        internal XOFieldCalculatorRouteRelation(XOTable TableForFill,
            XOField KeyFieldForFill,
            XOField FieldForRetrieve,
            string Filter)
        {
            this._tableForFill = TableForFill;

            this._keyFieldForFill = KeyFieldForFill;

            this._fieldForRetrieve = FieldForRetrieve;

            this._filter = Filter;
        }

        private readonly XOTable _tableForFill;

        private readonly XOField _keyFieldForFill;

        private readonly XOField _fieldForRetrieve;

        private readonly string _filter;

        public XOField KeyFieldForFill
        {
            get { return _keyFieldForFill; }
        }

        public XOField FieldForRetrieve
        {
            get { return _fieldForRetrieve; }
        }

        public XOTable TableForFill
        {
            get { return _tableForFill; }
        }

        public string Filter
        {
            get { return _filter; }
        }
    }
}
