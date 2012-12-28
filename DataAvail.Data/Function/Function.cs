using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DataAvail.Data.Function
{
    public abstract partial class Function
    {
        public Function()
        {
            _paramCreator = OnCreateParamCreator();
        }

        public Function(string CommandText, System.Data.CommandType CommandType)
            : this()
        {
            this.Initialize(null, CommandText, CommandType);
        }


        public void Initialize<T>(System.Data.IDbConnection Connection, string CommandText, System.Data.CommandType CommandType, IEnumerable<T> MapItems, IFunctionParamMappingRules<T> MapRules)
        {
            this.Initialize(Connection, CommandText, CommandType);

            this.AddParams(MapItems.Select(p => MapRules.GetParam(p)).Where(p => p != null));
        }

        public void Initialize(System.Data.IDbConnection Connection, string CommandText, System.Data.CommandType CommandType)
        {
            this.Connection = Connection;

            this._commandText = CommandText;

            this._commandType = CommandType;        
        }

        private string _commandText;

        private System.Data.CommandType _commandType = System.Data.CommandType.Text;

        private System.Data.IDbConnection _connection;

        private System.Data.IDbCommand _command;

        private readonly IFunctionParamCreator _paramCreator;

        public event CommandExecutedHandler commandExecuted;

        private System.Data.CommandType CommandType
        {
            get { return _commandType; }
        } 

        public IFunctionParamCreator ParamCreator { get { return _paramCreator; } }

        public string CommandText
        {
            get
            {
                return _commandText;
            }
        }

        public System.Data.IDbConnection Connection
        {
            get { return _connection; }

            set 
            { 
                _connection = value;

                if (_command != null)
                {
                    _command.Connection = value;
                }
            }
        }

        public  System.Data.IDbCommand Command 
        {
            get
            {
                if (_command == null)
                {
                    _command = OnCreateCommand();

                    _command.CommandText = CommandText;

                    _command.CommandType = CommandType;

                    _command.Connection = Connection;
                }

                return _command;
            }
        }

        public virtual object Execute()
        {
            if (DataSet.Log != null)
                DataSet.Log.Write(Utils.GetCommandText(Command));

            if (Command.Connection.State == System.Data.ConnectionState.Closed)
                Command.Connection.Open();

            object retVal = Command.ExecuteScalar();

            if (Command.Parameters.Contains(ReturnParameterName))
            {
                retVal = ((System.Data.IDataParameter)Command.Parameters[ReturnParameterName]).Value;
                
                ((System.Data.IDataParameter)Command.Parameters[ReturnParameterName]).Value = null;
                
            }

            CommandExecutedEventArgs args = new CommandExecutedEventArgs(retVal);

            OnCommandExecuted(args);

            retVal = args.executionResult;

            return retVal;
        }


        public System.Data.IDataParameter AddParam(string Name, System.Data.DbType DbType, int Size, object Value, System.Data.ParameterDirection ParameterDirection, bool fSetIfExists)
        {   
            int i = this.Command.Parameters.IndexOf(Name);

            if (i != -1 && fSetIfExists)
            {
                SetParam(Name, Value);

                return (System.Data.IDataParameter)this.Command.Parameters[i];
            }
            else
                return AddParam(Name, DbType, Size, Value, ParameterDirection);
        }

        public System.Data.IDataParameter AddParam(string Name, System.Data.DbType DbType, object Value, System.Data.ParameterDirection ParameterDirection)
        {
            return AddParam(Name, DbType, 1, Value, ParameterDirection);
        }

        public System.Data.IDataParameter AddParam(string Name, System.Data.DbType DbType, int Size, object Value, System.Data.ParameterDirection ParameterDirection)
        {
            System.Data.IDataParameter param = CreateParameter(Size);

            param.DbType = DbType;

            param.Value = Value ?? DBNull.Value;

            param.Direction = ParameterDirection;

            param.ParameterName = ParameterDirection == System.Data.ParameterDirection.ReturnValue ? ReturnParameterName : Name;

            Command.Parameters.Add(param);

            return param;
        }

        public void SetParam(string SourceColumnName, object Value)
        {
            System.Data.IDataParameter dataParameter = DbParams.FirstOrDefault(p => p.SourceColumn == SourceColumnName);

            if (dataParameter != null)
                dataParameter.Value = Value;
        }

        public void Clear()
        {
            Command.Parameters.Clear();
        }

        #region abstracts

        protected abstract System.Data.IDbCommand OnCreateCommand();

        protected abstract IFunctionParamCreator OnCreateParamCreator();

        protected abstract string ReturnParameterName { get; }

        #endregion

        #region Virtuals

        public virtual System.Data.IDataParameter CreateParameter(int Size)
        {
            System.Data.IDataParameter param = ParamCreator.CreateParam();

            param.Value = Size;

            return param;
        }

        public virtual System.Data.IDataParameter CreateCursorParameter()
        {
            return ParamCreator.CreateCursorParam();
        }

        public virtual System.Data.IDataParameter CreateBlobParameter()
        {
            return ParamCreator.CreateBlobParam();
        }

        public virtual System.Data.IDataParameter CreateVarCharParameter()
        {
            return ParamCreator.CreateVarCharParam();
        }

        public virtual FunctionArrayParam CreateArrayParameter(string ElementType, System.Data.ParameterDirection ParameterDirection)
        {
            return ParamCreator.CreateArrayParam(ElementType, ParameterDirection);
        }

        #endregion

        public System.Data.IDataParameter AddParam(string Name, System.Data.DbType DbType)
        {
            return AddParam(Name, DbType, System.DBNull.Value);
        }

        public System.Data.IDataParameter AddParam(string Name, System.Data.DbType DbType, object Value)
        {
            return AddParam(Name, DbType, 1, Value, System.Data.ParameterDirection.Input);
        }

        public System.Data.IDataParameter AddParam(string Name, System.Data.DbType DbType, int Size, object Value)
        {
            return AddParam(Name, DbType, Size, Value, System.Data.ParameterDirection.Input);
        }

        public void AddParam(System.Data.IDataParameter DataParameter)
        {
            Command.Parameters.Add(DataParameter);
        }

        public System.Data.IDataParameter AddReturnParam(System.Data.DbType DbType)
        {
            return AddReturnParam(DbType, 1);
        }

        public System.Data.IDataParameter AddReturnParam(System.Data.DbType DbType, int Size)
        {
            return AddParam(null, DbType, Size, null, System.Data.ParameterDirection.ReturnValue);
        }

        public System.Data.IDataParameter AddBlobParameter(string ParamName)
        {
            System.Data.IDataParameter param = CreateBlobParameter();

            param.ParameterName = ParamName;

            AddParam(param);

            return param;
        }

        public System.Data.IDataParameter AddCursorParameter(string ParamName)
        {
            System.Data.IDataParameter param = CreateCursorParameter();

            param.ParameterName = ParamName;

            AddParam(param);

            return param;
        }

        public System.Data.IDataParameter AddVarCharParameter(string ParamName)
        {
            System.Data.IDataParameter param = CreateVarCharParameter();

            param.ParameterName = ParamName;

            AddParam(param);

            return param;
        }

        private IEnumerable<System.Data.IDbDataParameter> DbParams
        {
            get { return Command.Parameters.Cast<System.Data.IDbDataParameter>(); }
        }

        public void AddParams(IEnumerable<System.Data.IDbDataParameter> Params)
        {
            foreach (System.Data.IDbDataParameter param in Params)
            {
                Command.Parameters.Add(param);
            }
        }

        public void SetParams(IDictionary<string, object> Params)
        {
            foreach (KeyValuePair<string, object> kvp in Params)
            {
                System.Data.IDbDataParameter dbParam = DbParams.SingleOrDefault(p => p.SourceColumn == kvp.Key);

                if (dbParam != null)
                {
                    dbParam.Value = kvp.Value;
                }
            }
        }

        /// <summary>
        /// Find parameters with [key] names and substitute their names to [value] name.
        /// If [value] name is null delete found parameter.
        /// </summary>
        /// <param name="ParamNames"></param>
        public void SwapParams(IDictionary<string, string> ParamNames)
        {
            foreach (var r in (from s in DbParams
                              join t in ParamNames on s.SourceColumn equals t.Key
                              select new { s, t.Value }).ToArray())
            {
                if (r.Value != null)
                {
                    r.s.ParameterName = r.Value;
                }
                else
                {
                    Command.Parameters.Remove(r.s);
                }
            }
        }

        protected virtual void OnCommandExecuted(CommandExecutedEventArgs args)
        {
            if (commandExecuted != null)
                commandExecuted(this, args);
        }

    }
}