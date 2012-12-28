using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Data.DbContext;
using DataAvail.XObject;
using DataAvail.XObject.XContexts;

namespace DataAvail.XtraBindings.Calculator
{
    /// <summary>
    /// Default implementation of IObjectCalculator interface. 
    /// </summary>
    public class DefaultObjectCalculator : DataAvail.XtraBindings.Calculator.IObjectCalculator
    {
        public DefaultObjectCalculator()
        { }

        private static readonly DataAvail.XtraBindings.Calculator.IObjectCalculator _defaultObjectCalulator = new DefaultObjectCalculator();

        public static DataAvail.XtraBindings.Calculator.IObjectCalculator DefaultObjectCalulator
        {
            get { return _defaultObjectCalulator; }
        }

        public virtual IObjectCalculatorPersistData PersistData
        {
            get { return ObjectCalculatorPersistDataDefault.persistDataDefault; }
        }

        public virtual void Calculate(DataAvail.XtraBindings.Calculator.ObjectProperties Item, string FieldName, DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType CalculateType)
        {
            XOTableContext TableContext = Item.tableContext;

            #region InitializeNew - invoked when new item is added

            if (CalculateType == DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.InitializeNew)
            {
                CalculateDefaultValues(TableContext, Item);

                CalculateChildrenRelationsWithFilters(TableContext, Item, FieldName);
            }

            #endregion

            #region Clone current row

            if (CalculateType == DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.Clone)
            {
                Item.SetValue(TableContext.PkFieldName , null);
            }

            #endregion

            #region Item is Initialized

            if (CalculateType == DataAvail.XtraBindings.Calculator.ObjectCalulatorCalculateType.Initialize)
            {
                if (PersistData != null)
                    PersistData.Load(Item);

                foreach (XOFieldContext field in  TableContext.Fields.Where(p => p.Calculator != null))
                {
                    CalculateFieldCalculator(Item, field);
                }

                CalculateChildrenRelationsWithFilters(TableContext, Item, FieldName);
            }

            #endregion

            #region Item was saved

            if (CalculateType == ObjectCalulatorCalculateType.AfterSave)
            {
                if (PersistData != null)
                    PersistData.Save(Item);

            }

            #endregion

            #region Field changed

            if (CalculateType == ObjectCalulatorCalculateType.Calculate)
            {
                foreach (XOFieldContext field in TableContext.Fields.Where(p => p.Calculator != null && p.Calculator.IssueField == FieldName))
                {
                    if (field != null)
                    {
                        CalculateFieldCalculator(Item, field);
                    }
                }
            }

            #endregion
        }

        private static void CalculateDefaultValues(XOTableContext TableContext, DataAvail.XtraBindings.Calculator.ObjectProperties Item)
        {
            foreach (XOField field in TableContext.Fields.Select(p=>p.XOField).Where(p=>p.DefaultValue != null))
            {
                Item.SetValue(field.Name, field.DefaultValue);
            }
        }

        private static void CalculateChildrenRelationsWithFilters(XOTableContext TableContext, DataAvail.XtraBindings.Calculator.ObjectProperties Item, string FieldName)
        {
            XORelation rel = TableContext.FkItemRelation;

            if (rel != null && rel.DefaultValues != null)
            {
                foreach (KeyValuePair<string, object> kvp in rel.DefaultValues)
                {
                    Item.SetValue(kvp.Key, kvp.Value);
                    Item.SetReadOnly(kvp.Key, true);
                }
            }
        }

        private static void CalculateFieldCalculator(DataAvail.XtraBindings.Calculator.ObjectProperties Item, XOFieldContext AppFieldContext)
        {
            object val = null;

            foreach (XOFieldCalculatorRouteRelation rel in AppFieldContext.Calculator.RouteRelations)
            {
                if (val == null)
                    val = Item.GetValue(AppFieldContext.Calculator.IssueField);

                if (val != System.DBNull.Value)
                {

                    string fillExr = string.Format("{0} = {1}", rel.KeyFieldForFill.Name, val);

                    if (!string.IsNullOrEmpty(rel.Filter))
                    {
                        fillExr = string.Format("{0} AND {1}", fillExr, rel.Filter);
                    }

                    val = DbContext.GetScalar("SELECT {0} FROM {1} WHERE {2}", rel.FieldForRetrieve.Name, rel.TableForFill.Source, fillExr);


                    if (val != null)
                    {
                        continue;
                    }
                }

                val = null;

                break;
            }

            if (val != null)
            {
                Item.SetValue(AppFieldContext.Name, val);
            }
            else
            {
                Item.SetValue(AppFieldContext.Name, System.DBNull.Value);
                Item.SetError(AppFieldContext.Name, "Can't evalute defiened calculator expresssion!");
            }
        }

    }
}
