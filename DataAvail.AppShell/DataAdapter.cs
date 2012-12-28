﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.Linq;
using DataAvail.Data.DbContext;

namespace DataAvail.AppShell
{
    internal partial class DataAdapter : DataAvail.Data.DataAdapter.DataAdapterAsync
    {
        internal DataAdapter(
                XOTable AppItem)
            : base(DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.Connection,
            AppItem.IsUseDefInsertCommand ? DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand() : null,
            AppItem.IsUseDefUpdateCommand ? DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand() : null,
            AppItem.IsUseDefDeleteCommand ? DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateCommand() : null)
        {
            _appItem = AppItem;

            if (InsertFun != null)
            {
                if (AppItem.IsUseDefInsertCommand)
                {
                    InsertFun.Initialize(
                        this.DbContextObjectCreator.Connection,
                        this.GetInsertCommandText(),
                        System.Data.CommandType.Text,
                        this.GetInsertFunctionParameters(),
                        new FieldMappingRules(null, InsertFun.ParamCreator));

                    if (AppItem.IsPkAutoGenerated)
                        InsertFun.commandExecuted += new DataAvail.Data.Function.CommandExecutedHandler(InsertFun_commandExecuted);

                    _selectLastInsertedPkCommand = this.DbContextObjectCreator.CreateCommand();
                    _selectLastInsertedPkCommand.Initialize(this.DbContextObjectCreator.Connection,
                        DataAvail.Data.DbContext.DbContext.CurrentContext.GetIdentityCommandText(AppItem.SourceUpdate), System.Data.CommandType.Text);
                }
                else
                {
                    InsertFun.Initialize(
                       this.DbContextObjectCreator.Connection,
                       AppItem.InsertFunction.Name,
                       System.Data.CommandType.StoredProcedure,
                       AppItem.InsertFunction.Params,
                       new FunctionParamMappingRules(null, InsertFun.ParamCreator));
                }
            }

            if (this.UpdateFun != null)
            {
                if (AppItem.IsUseDefInsertCommand)
                {
                    UpdateFun.Initialize(
                        this.DbContextObjectCreator.Connection,
                        this.GetUpdateCommandText(),
                        System.Data.CommandType.Text,
                        this.GetUpdateFunctionParameters(),
                        new FieldMappingRules(null, UpdateFun.ParamCreator));
                }
                else
                {
                    UpdateFun.Initialize(
                 this.DbContextObjectCreator.Connection,
                 AppItem.UpdateFunction.Name,
                 System.Data.CommandType.StoredProcedure,
                 AppItem.UpdateFunction.Params,
                 new FunctionParamMappingRules(null, UpdateFun.ParamCreator));
                }
            }

            if (this.DeleteFun != null)
            {
                if (AppItem.IsUseDefDeleteCommand)
                {
                    DeleteFun.Initialize(
                        this.DbContextObjectCreator.Connection,
                        this.GetDeleteCommandText(),
                        System.Data.CommandType.Text,
                        this.GetDeleteFunctionParameters(),
                        new FieldMappingRules(null, DeleteFun.ParamCreator));
                }
                else
                {
                    DeleteFun.Initialize(
                  this.DbContextObjectCreator.Connection,
                  AppItem.DeleteFunction.Name,
                  System.Data.CommandType.StoredProcedure,
                  AppItem.DeleteFunction.Params,
                  new FunctionParamMappingRules(null, DeleteFun.ParamCreator));
                }
            }
        }


        private readonly XOTable _appItem;

        private readonly DataAvail.Data.Function.Function _selectLastInsertedPkCommand;

        private XOTable AppItem
        {
            get { return _appItem; }
        }

        private DataAvail.Data.DbContext.IDbContextObjectCreator DbContextObjectCreator
        {
            get { return DbContext.CurrentContext.ObjectCreator; }
        }

        private string GetInsertCommandText()
        {
            var insertFunctionParams = GetInsertFunctionParameters();

            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                AppItem.SourceUpdate,
                insertFunctionParams.Select(p => p.Name.ToUpper()).ToString(","),
                insertFunctionParams.Select(p => DataAvail.Data.DbContext.DbContext.CurrentContext.ParameterValuePrefix + p.Name.ToUpper()).ToString(","));
        }

        private IEnumerable<XOField> GetInsertFunctionParameters()
        {
            return AppItem.Fields.Where(p => (p.IsMapped && (!p.IsPk || !p.IsPkAutoGenerated)));
        }

        private string GetUpdateCommandText()
        {
            var updateCommandTextParams = GetUpdateFunctionParameters();

            if (!DbContext.CurrentContext.IsPkIncludedIntoUpdate)
                updateCommandTextParams = updateCommandTextParams.Where(p => !p.IsPk);

            return string.Format("UPDATE {0} SET {1} WHERE {2} = {3}",
                AppItem.SourceUpdate,
                updateCommandTextParams.Select(p => DataAvail.Data.DbContext.DbContext.CurrentContext.ParameterValuePrefix + p.Name.ToUpper())
                .ToString(updateCommandTextParams.Select(p => p.Name.ToUpper()), null, "=", ","),
                GetUpdateFunctionParameters().Where(p => p.IsPk).Select(p => p.Name.ToUpper()).First(),
                GetUpdateFunctionParameters().Where(p => p.IsPk).Select(p => DbContext.CurrentContext.ParameterValuePrefix + p.Name.ToUpper()).First());
        }

        private IEnumerable<XOField> GetUpdateFunctionParameters()
        {
            return AppItem.Fields.Where(p => p.IsMapped);
        }

        private string GetDeleteCommandText()
        {
            return string.Format("DELETE FROM {0} WHERE {1} = {2}",
                AppItem.SourceUpdate,
                GetDeleteFunctionParameters().Select(p => p.Name.ToUpper()).ToString(","),
                GetDeleteFunctionParameters().Select(p => DbContext.CurrentContext.ParameterValuePrefix + p.Name.ToUpper()).ToString(","));
        }

        private IEnumerable<XOField> GetDeleteFunctionParameters()
        {
            return AppItem.Fields.Where(p => p.IsPk);
        }

        void InsertFun_commandExecuted(object sender, DataAvail.Data.Function.CommandExecutedEventArgs args)
        {
            object retVal = _selectLastInsertedPkCommand.Execute();

            args.executionResult = retVal;
        }

        internal static DataAvail.Data.DataAdapter.IDataAdapter GetDataAdapter(XOTable AppItem)
        {
            return new DataAdapter(AppItem);
        }

    }
}
