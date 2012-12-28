using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;
using DataAvail.Data;

namespace DataAvail.Data.Function
{
    partial class Function
    {

        public static Function GetSelectCommand(string SourceName, System.Data.DataTable DataTable, string Where)
        {
            string commandText = string.Format("SELECT {0} FROM {1}", 
                DataTable.Columns.Cast<System.Data.DataColumn>().ToString(p => p.ColumnName, ","),
                SourceName);

            if (!string.IsNullOrEmpty(Where))
                commandText = string.Format("{0} WHERE {1}", commandText, Where);

            Function selectCommand = DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand();

            selectCommand.Initialize(DbContext.DbContext.CurrentContext.ObjectCreator.Connection, commandText, System.Data.CommandType.Text);

            return selectCommand;
        }

        public static Function GetInsertCommand(string SourceName, System.Data.DataTable DataTable, params string[] ExcludedColumns)
        {
            IEnumerable<System.Data.DataColumn> dataCols = DataTable.Columns.Cast<System.Data.DataColumn>().Where(p=>!ExcludedColumns.Contains(p.ColumnName));

            string[] cols = dataCols.Select(p=>p.ColumnName).ToArray();

            string commandText= string.Format("INSERT INTO {0} ({1}) VALUES ({2})", SourceName, 
                cols.ToString(p=>p.ToUpper(), ","), 
                cols.ToString(p=> string.Format("{0}{1}", DbContext.DbContext.CurrentContext.ParameterValuePrefix, p), ","));

            Function insertCommand = DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand();

            insertCommand.Initialize(DbContext.DbContext.CurrentContext.ObjectCreator.Connection, commandText, System.Data.CommandType.Text, 
                dataCols, new DataAvail.Data.Function.DCFunctionParamMappingRules(null, insertCommand.ParamCreator));

            return insertCommand;
        }

        public static Function GetUpdateCommand(string SourceName, System.Data.DataTable DataTable, params string [] ExcludedColumns)
        {
            IEnumerable<System.Data.DataColumn> dataCols = DataTable.Columns.Cast<System.Data.DataColumn>().Where(p => !ExcludedColumns.Contains(p.ColumnName));

            if (!DataAvail.Data.DbContext.DbContext.CurrentContext.IsPkIncludedIntoUpdate)
                dataCols = dataCols.Except(DataTable.PrimaryKey);

            string[] cols = dataCols.Select(p => p.ColumnName).ToArray();

            string commandText = string.Format("UPDATE {0} SET {1}",
                SourceName, cols.ToString(p=>string.Format("{0}={1}{0}", p.ToUpper(), DbContext.DbContext.CurrentContext.ParameterValuePrefix), ","));

            if (DataTable.PrimaryKey.Length != 0)
                commandText = string.Format("{0} WHERE {1}", commandText, DataTable.PrimaryKey.ToString(p => string.Format("{0}={1}{0}", p.ColumnName.ToUpper(), DbContext.DbContext.CurrentContext.ParameterValuePrefix), " AND "));

            Function updateCommand = DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand();

            updateCommand.Initialize(DbContext.DbContext.CurrentContext.ObjectCreator.Connection, commandText, System.Data.CommandType.Text,
                dataCols, new DataAvail.Data.Function.DCFunctionParamMappingRules(null, updateCommand.ParamCreator));

            return updateCommand;
        }

        public static Function GetDeleteCommand(string SourceName, System.Data.DataTable DataTable)
        {
            IEnumerable<System.Data.DataColumn> dataCols = DataTable.Columns.Cast<System.Data.DataColumn>();

            string[] cols = dataCols.Select(p => p.ColumnName).ToArray();

            string commandText = string.Format("DELETE FROM {0}", SourceName);

            if (DataTable.PrimaryKey.Length != 0)
            {
                commandText = string.Format("{0} WHERE {1}",
                    DataTable.PrimaryKey.ToString(p => string.Format("{0}={1}{0}", p.ColumnName.ToUpper(),
                        DbContext.DbContext.CurrentContext.ParameterValuePrefix), " AND "));
            }
            else
            {
                commandText = string.Format("{0} WHERE {1}", commandText,
                    dataCols.ToString(p => string.Format("{0}={1}{0}", p.ColumnName.ToUpper(),
                        DbContext.DbContext.CurrentContext.ParameterValuePrefix), " AND "));                
            }

            Function deleteCommand = DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand();

            deleteCommand.Initialize(DbContext.DbContext.CurrentContext.ObjectCreator.Connection, commandText, System.Data.CommandType.Text,
                dataCols, new DataAvail.Data.Function.DCFunctionParamMappingRules(null, deleteCommand.ParamCreator));

            return deleteCommand;

        }

    }
}
