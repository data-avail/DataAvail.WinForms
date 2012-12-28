using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;

namespace DataAvail.Data.DbContext
{
    public class SelectCommandManager
    {
        public SelectCommandManager(System.Data.DataTable DataTable, System.Data.IDbCommand Command)
        {
            dataTable = DataTable;

            command = Command;

            commandParams = Command.Parameters.Cast<System.Data.IDbDataParameter>();

            commandText = Command.CommandText;
        }

        public void RestoreCommand()
        {
            command.Parameters.Clear();

            command.Parameters.AddRange(commandParams);

            command.CommandText = commandText;
        }

        public void SubstituteCommand(string CommandText, System.Collections.IEnumerable Params)
        {
            if (!string.IsNullOrEmpty(CommandText))
            {
                command.CommandText = DataAvail.Data.Data.SqlCommandSubstituteWhere(command.CommandText, CommandText);

                command.Parameters.Clear();

                if (Params != null)
                    command.Parameters.AddRange(Params);
            }
        }

        private static System.Data.IDbDataParameter CreateParam(System.Data.IDbCommand Command, string ParamName, object ParamValue)
        {
            System.Data.IDbDataParameter dbParam = Command.CreateParameter();

            dbParam.ParameterName = ParamName;

            dbParam.Value = ParamValue;

            return dbParam;
        }


        public static SelectCommandManager SubstituteCommand(System.Data.DataTable DataTable, System.Data.IDbCommand Command, string CommandText)
        {
            SelectCommandManager scm = new SelectCommandManager(DataTable, Command);

            scm.SubstituteCommand(CommandText, null);

            return scm;
        }

        private readonly System.Data.DataTable dataTable;

        private readonly System.Data.IDbCommand command;

        private readonly string commandText;

        private readonly IEnumerable<System.Data.IDbDataParameter> commandParams;
    }
}
