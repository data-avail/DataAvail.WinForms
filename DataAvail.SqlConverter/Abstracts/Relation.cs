using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAvail.SqlConverter
{
    public abstract class Relation : IRelation
    {
        public Relation(ITable Table, IDataReader DataReader)
        {
            _table = Table;

            _parentColumn = (string)DataReader["ForeignColumnName"];

            _parentTable = (string)DataReader["ForeignTableName"];

            _childColumn = (string)DataReader["ColumnName"];

            _childTable = Table.Name;
        }

        private readonly ITable _table;

        private readonly string _childColumn;

        private readonly string _childTable;

        private readonly string _parentColumn;

        private readonly string _parentTable;

        #region IRelation Members

        public string ChildColumn
        {
            get { return _childColumn; }
        }

        public string ChildTable
        {
            get { return _childTable; }
        }

        public string ParentColumn
        {
            get { return _parentColumn; }
        }

        public string ParentTable
        {
            get { return _parentTable; }
        }

        public ITable Table
        {
            get { return _table; }
        }

        #endregion

        #region Abstracts

        protected abstract string ReaderChildColumnField { get; }

        protected abstract string ReaderChildTableField { get; }

        protected abstract string ReaderParentColumnField { get; }

        protected abstract string ReaderParentTableField { get; }

        #endregion
    }
}
