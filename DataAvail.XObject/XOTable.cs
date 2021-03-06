﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject.XContexts;
using DataAvail.XObject.XOP;
using DataAvail.XObject.XWP;
using DataAvail.XObject.XSP;
using DataAvail.Utils;


namespace DataAvail.XObject
{
    public class XOTable
    {
        public XOTable(XOApplication XOApplication, XOPTable XOPTable, XWPTable XWPTable, XSPTable XSPTable)
        {
            _xoApplication = XOApplication;

            _xopTable = XOPTable;

            _xwpTable = XWPTable;

            _xspTable = XSPTable;

            _fields = XopTable.Fields.Fields.Select(p => new XOField(this, p,
                XWPTable != null ? XWPTable.Fields.Fields.FirstOrDefault(s => s.FieldName == p.Name) : null,
                XSPTable != null ? XSPTable.Fields.FirstOrDefault(s => s.FieldName == p.Name) : null)).ToArray();

            if (XoApplication.XopDataSet.Relations != null)
            {
                _childrenRelations = XoApplication.XopDataSet.Relations.Relations.Where(p => p.ParentTable == this.Name)
                    .Select(p => new XORelation(
                        this, p, GetXwpRelation(p.Name))
                        ).ToArray();
            }
            else
            {
                _childrenRelations = new XORelation[] { };
            }
        }

        private readonly XOApplication _xoApplication;

        private readonly XOPTable _xopTable;

        private readonly XWPTable _xwpTable;

        private readonly XSPTable _xspTable;

        private readonly XOField[] _fields;

        private XORelation[] _childrenRelations;

        internal void EndInit()
        {
            foreach (XOField field in Fields)
                field.EndInit();

            foreach (XORelation rel in ChildrenRelations)
                rel.EndInit();
        }

        public XOApplication XoApplication
        {
            get { return _xoApplication; }
        }

        public XOPTable XopTable
        {
            get { return _xopTable; }
        }

        public XWPTable XwpTable
        {
            get { return _xwpTable; }
        }

        public XSPTable XspTable
        {
            get { return _xspTable; }
        }

        public XOMode Mode
        {
            get
            {
                XOMode mode = XoApplication.Mode;

                if (XspTable != null)
                    mode = XspTable.Mode;

                return mode;
            }
        }

        public XOField[] Fields
        {
            get { return _fields; }
        }

        public string Name
        {
            get { return XopTable.Name; }
        }

        public string Caption
        {
            get { return XwpTable == null || string.IsNullOrEmpty(XwpTable.Caption) ? Name : XwpTable.Caption; }
        }

        public string ItemCaption
        {
            get { return XwpTable == null || string.IsNullOrEmpty(XwpTable.ItemCaption) ? Caption : XwpTable.ItemCaption; }
        }

        public bool IsPkAutoGenerated
        {
            get
            {
                return PkField == null ? false : PkField.XopField.IsPkAutoGenerated;
            }
        }

        public string PkFieldName
        {
            get 
            {
                return PkField == null ? null : PkField.XopField.Name;
            }
        }

        private bool GetIsCanSaveMode(XWPTableSaveMode SaveMode, XWPTableSaveMode IsCanMode)
        {
            if (SaveMode != XWPTableSaveMode.Default &&
                !EnumFlags.IsContain(SaveMode, IsCanMode))
            {
                return false;
            }

            return true;        
        }

        public XWPTableSaveMode SaveMode
        {
            get 
            {
                if (XwpTable != null && XwpTable.SaveMode != XWPTableSaveMode.Default)
                    return XwpTable.SaveMode;

                if (XoApplication.XwpApplication != null && 
                    XoApplication.XwpApplication.DataView != null &&
                    XoApplication.XwpApplication.DataView.SaveMode != XWPTableSaveMode.Default)
                    return XoApplication.XwpApplication.DataView.SaveMode;

                return XWPTableSaveMode.Default;
           }
        }

        public XWPTableSaveMode ChildSaveMode
        {
            get
            {
                if (XwpTable != null && XwpTable.ChildSaveMode != XWPTableSaveMode.Default)
                    return XwpTable.ChildSaveMode;

                if (XoApplication.XwpApplication != null &&
                    XoApplication.XwpApplication.DataView != null &&
                    XoApplication.XwpApplication.DataView.ChildSaveMode != XWPTableSaveMode.Default)
                    return XoApplication.XwpApplication.DataView.ChildSaveMode;

                return XWPTableSaveMode.Default;
            }
        }

        public bool IsCanSaveInCache
        {
            get 
            {
                return GetIsCanSaveMode(SaveMode, XWPTableSaveMode.Cache);
            }
        }

        public bool IsCanSaveChildInCache
        {
            get
            {
                return GetIsCanSaveMode(ChildSaveMode, XWPTableSaveMode.Cache);
            }
        }


        public bool IsCanSaveInStorage
        {
            get 
            {
                return GetIsCanSaveMode(SaveMode, XWPTableSaveMode.Repository);
            }
        }

        public bool IsCanSaveChildInStorage
        {
            get
            {
                return GetIsCanSaveMode(ChildSaveMode, XWPTableSaveMode.Repository);
            }
        }

        public bool IsCanMove
        {
            get { return true; }
        }

        public bool IsCanClone
        {
            get 
            {
                return true;
            }
        }

        public bool IsCanView
        {
            get 
            {
                return XspTable == null || EnumFlags.IsContain(XspTable.Mode, XOMode.View);
            }
        }

        public bool IsCanEdit
        {
            get
            {
                return XspTable == null || EnumFlags.IsContain(XspTable.Mode, XOMode.Edit) || EnumFlags.IsContain(XspTable.Mode, XOMode.Add);
            }
        }

        public bool IsCanAdd
        {
            get
            {
                return
                  (IsUseDefInsertCommand || XopTable.Functions.Functions.FirstOrDefault(p => p.CommandType == XOPTableCommandType.Insert) != null)
                  && (XspTable == null || EnumFlags.IsContain(XspTable.Mode, XOMode.Add));
            }
        }

        public bool IsCanRemove
        {
            get { return 
                    (IsUseDefDeleteCommand || XopTable.Functions.Functions.FirstOrDefault(p=>p.CommandType == XOPTableCommandType.Delete) != null)
                    && (XspTable == null || EnumFlags.IsContain(XspTable.Mode, XOMode.Delete));
            }
        }

        public string SerializationName
        {
            get { return this.Name; }
        }

        public bool IsUseDefInsertCommand
        {
            get
            {
                return XopTable.StdCommandsType == XOPTableCommandType.Default ? XoApplication.XopDataSet.UseStdCommands : EnumFlags.IsContain(XopTable.StdCommandsType, XOPTableCommandType.Select);
            }
        }

        public bool IsUseDefUpdateCommand
        {
            get
            {
                return XopTable.StdCommandsType == XOPTableCommandType.Default ? XoApplication.XopDataSet.UseStdCommands : EnumFlags.IsContain(XopTable.StdCommandsType, XOPTableCommandType.Update);
            }
        }

        public bool IsUseDefDeleteCommand
        {
            get
            {
                return  XopTable.StdCommandsType == XOPTableCommandType.Default ? XoApplication.XopDataSet.UseStdCommands : EnumFlags.IsContain(XopTable.StdCommandsType, XOPTableCommandType.Delete);
            }
        }

        public string Source
        {
            get
            {
                return XopTable.Source == null ? XopTable.Name : XopTable.Source;
            }
        }

        public string SourceUpdate
        {
            get
            {
                return XopTable.SourceUpdate == null ? Source : XopTable.SourceUpdate;
            }
        }

        public bool AutoFill
        {
            get { return XopTable.AutoFill; }
        }

        public bool PersistFill
        {
            get { return XopTable.PersistFill; }
        }

        public XORelation[] ShownChildrenRelations
        {
            get { return ChildrenRelations.Where(p=>p.IsShown).ToArray(); }
        }

        public XORelation GetChildRelation(XOField ChildField)
        {
            return ChildrenRelations.FirstOrDefault(p => p.ChildField == ChildField);
        }

        public XORelation [] ChildrenRelations
        {
            get { return _childrenRelations; }
        }

        private XOField PkField
        {
            get
            {
                return Fields.FirstOrDefault(p => p.XopField.IsPk);
            }
        }

        public XOFunction InsertFunction
        {
            get 
            { 
                XOPFunction func = XopTable.Functions.Functions.FirstOrDefault(p=>p.CommandType == XOPTableCommandType.Insert); 

                return func == null ? null : new XOFunction(this, func);
            }
        }

        public XOFunction UpdateFunction
        {
            get 
            { 
                XOPFunction func = XopTable.Functions.Functions.FirstOrDefault(p=>p.CommandType == XOPTableCommandType.Update); 

                return func == null ? null : new XOFunction(this, func);
            }
        }

        public XOFunction DeleteFunction
        {
            get
            {
                XOPFunction func = XopTable.Functions.Functions.FirstOrDefault(p => p.CommandType == XOPTableCommandType.Delete);

                return func == null ? null : new XOFunction(this, func);
            }
        }

        public XOTableContext CreateTableChildContext(XOFieldContext ParentFieldContext)
        {
            return XoApplication.GetTableContext(XContext.GetChildContext(ParentFieldContext), this);
        }

        public XOTableContext CreateTableFkItemSelectContext(XOFieldContext ChildFieldContext)
        {
            return XoApplication.GetTableContext(XContext.GetFkItemSelectContext(ChildFieldContext), this);
        }

        public XOTableContext CreateTableContext(XContext context)
        {
            return XoApplication.GetTableContext(context, this);
        }

        private XWPRelation GetXwpRelation(string Name)
        {
            if (XoApplication != null && XoApplication.XwpApplication != null && XoApplication.XwpApplication.DataView != null && XoApplication.XwpApplication.DataView.Relations != null)
            {
                return XoApplication.XwpApplication.DataView.Relations.Relations.FirstOrDefault(s => s.RelationName == Name);
            }
            else
            {
                return null;
            }
        }
    }
}
