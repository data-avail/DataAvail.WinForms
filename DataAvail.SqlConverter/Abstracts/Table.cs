using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DataAvail.SqlConverter
{
    public abstract class Table : ITable
    {
        internal Table(IDb Db, IDataReader DataReader)
        {
            _db = Db;

            _schemeName = (string)DataReader[ReaderSchemeNameField];

            _name = (string)DataReader[ReaderNameFieldName];

        }

        private readonly IDb _db;

        private readonly string _schemeName;

        private readonly string _name;

        private IColumn [] _columns;

        private IRelation [] _fkRelations;

        internal void EndInit(IDbConnection Connection)
        {
            IDbCommand cmdColumns = CreateSelectColumnsCommand(Connection);

            using (IDataReader reader = cmdColumns.ExecuteReader())
            {
                _columns = GetColumns(reader).ToArray();
            }

            IDbCommand cmdFkRelations = CreateSelectFkRelationsCommand(Connection);

            using (IDataReader reader = cmdFkRelations.ExecuteReader())
            {
                _fkRelations = GetFkRelations(reader).ToArray();
            }
        }

        private IEnumerable<IColumn> GetColumns(IDataReader DataReader)
        {
            while (DataReader.Read())
            {
                yield return CreateColumn(DataReader);
            }
        }

        private IEnumerable<IRelation> GetFkRelations(IDataReader DataReader)
        {
            while (DataReader.Read())
            {
                yield return CreateRelation(DataReader);
            }
        }

        #region ITable Members

        public IDb Db
        {
            get { return _db; }
        }

        public abstract bool IsSysTable { get; }

        public string SchemeName
        {
            get { return _schemeName; }
        }

        public string Name
        {
            get { return _name; }
        }

        public IColumn [] Columns
        {
            get { return _columns; }
        }

        public IRelation [] FkRelations
        {
            get { return _fkRelations; }
        }

        #endregion

        #region Abstracts

        protected abstract string ReaderSchemeNameField { get; }

        protected abstract string ReaderNameFieldName { get; }

        protected abstract IDbCommand CreateSelectColumnsCommand(IDbConnection DbConnection);

        protected abstract IDbCommand CreateSelectFkRelationsCommand(IDbConnection DbConnection);

        protected abstract IColumn CreateColumn(IDataReader DataReader);

        protected abstract IRelation CreateRelation(IDataReader DataReader);

        #endregion

    }
}
