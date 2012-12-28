using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAvail.Linq;

namespace DataAvail.Data.DbContext
{
    public partial class DbContextDataAdapter : DataAvail.Data.DbContext.IDbContextDataAdapter
    {
        public DbContextDataAdapter(IDataTableStubManager Stubmanager)
        {
            this._stubManager = Stubmanager;
        }

        #region IDbContextDataAdapter Members

        public void Fill(IEnumerable<System.Data.DataRow> Rows)
        {
            System.Data.DataRow firstRow = Rows.FirstOrDefault();

            if (firstRow != null)
            {
                System.Data.DataTable clonedTable = firstRow.Table.Clone();

                IDataTableStub dataTableStub = GetDataTableStub(clonedTable);

                Fill(dataTableStub, Rows.ToWhere(), false);

                Merge(firstRow.Table, clonedTable);
            }
        }

        public void Fill(System.Data.DataTable DataTable, string Where)
        {
            IDataTableStub dataTableStub = GetDataTableStub(DataTable);

            Fill(dataTableStub, Where, true);
        }

        public void Update(System.Data.DataTable DataTable)
        {
            IDataTableStub dataTableStub = GetDataTableStub(DataTable);

            dataTableStub.Update();
        }

        #endregion

        private static void Merge(System.Data.DataTable DestTable, System.Data.DataTable SrcTable)
        {
            string pk = DestTable.PrimaryKey[0].ColumnName;

            System.Data.DataRow[] pkChangedRows = DestTable.AsEnumerable().Where(p => p.RowState == DataRowState.Modified
                && !p[pk, DataRowVersion.Original].Equals(p[pk, DataRowVersion.Current])).ToArray();

            try
            {
                //When PK has changed, move row to added state
                foreach (System.Data.DataRow dr in pkChangedRows)
                {
                    dr.AcceptChanges();
                    dr.SetAdded();
                }

                DestTable.Merge(SrcTable);

            }
            finally
            {
                //And when back to modifyed
                foreach (System.Data.DataRow dr in pkChangedRows)
                {
                    dr.AcceptChanges();
                    dr.SetModified();
                }
            }

        }

        private static void Fill(DataAvail.Data.DbContext.DbContextDataAdapter.IDataTableStub DataTableStub, string Where, bool ClearBefore)
        {
            Fill(DataTableStub, SelectCommandManager.SubstituteCommand(DataTableStub.DataTable, DataTableStub.SelectCommand, Where), ClearBefore);
        }

        private static void Fill(IDataTableStub DataTableStub, SelectCommandManager SelectCommandManager, bool ClearBefore)
        {
            try
            {
                if (ClearBefore)
                    DataTableStub.DataTable.Clear();

                DataTableStub.Fill();
            }
            finally
            {
                SelectCommandManager.RestoreCommand();
            }
        }

        public void InitializeCommands(System.Data.DataTable DataTable, IDbCommand InsertCommand, IDbCommand UpdateCommand, IDbCommand DeleteCommand, IDbCommand SelectCommand)
        {
            IDataTableStub stub = GetDataTableStub(DataTable);

            stub.InsertCommand = InsertCommand ?? DataAvail.Data.Function.Function.GetInsertCommand(DataTable.TableName, DataTable).Command;
            stub.UpdateCommand = UpdateCommand ?? DataAvail.Data.Function.Function.GetUpdateCommand(DataTable.TableName, DataTable).Command;
            stub.DeleteCommand = DeleteCommand ?? DataAvail.Data.Function.Function.GetDeleteCommand(DataTable.TableName, DataTable).Command;
            stub.SelectCommand = SelectCommand ?? DataAvail.Data.Function.Function.GetSelectCommand(DataTable.TableName, DataTable, null).Command;
        }
    }
}
