using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Data.DbContext;
using System.Data;
using DataAvail.Linq;


namespace DataAvail.XtraBindings.Calculator
{
    /// <summary>
    /// Load / Save fields states from table.
    /// Table should have colums:
    /// TABLE_NAME, string,50
    /// STATE, string, 50
    /// OBJID, int
    /// COLS, string, max 
    /// </summary>
    public class ObjectCalculatorPersistDataDefault : IObjectCalculatorPersistData
    {
        public readonly static ObjectCalculatorPersistDataDefault persistDataDefault = new ObjectCalculatorPersistDataDefault();

        public ObjectCalculatorPersistDataDefault()
        {
            _dataTable = DataAvail.Data.DbContext.DbContext.CurrentContext.ObjectCreator.CreateDataTable(null);
            _dataTable.TableName = "PERSIST_FIELDS_STATE";
            _dataTable.Columns.Add("TABLE_NAME", typeof(string));
            _dataTable.Columns.Add("STATE", typeof(string));
            _dataTable.Columns.Add("OBJID", typeof(int));
            _dataTable.Columns.Add("COLS", typeof(string));

            DataAvail.Data.DbContext.DbContext.CurrentContext.DataAdapter.InitializeCommands(_dataTable, null, null, null, null);
        }

        private readonly System.Data.DataTable _dataTable;

        public string DataTableName
        {
            get { return _dataTable.TableName; }

            set {_dataTable.TableName = value;}
        }

        #region IObjectCalculatorPersistData Members

        public void Load(ObjectProperties ObjectProperties)
        {
            if (_dataTable != null)
            {
                _dataTable.Fill(string.Format("TABLE_NAME = '{0}' AND OBJID = {1}", ObjectProperties.tableContext.XOTable.Source, ObjectProperties.GetValue(ObjectProperties.tableContext.PkFieldName)));

                foreach (var r in _dataTable.Rows.Cast<System.Data.DataRow>()
                    .Select(p => new { state = p.Field<string>("STATE"), fields = p.Field<string>("COLS").Split(',') }))
                {
                    foreach (string field in r.fields)
                    {
                        ObjectProperties.SetState(field, r.state);
                    }
                }

                _dataTable.Clear();
            }
        }

        public void Save(ObjectProperties ObjectProperties)
        {
            if (ObjectProperties[ObjectProperties.STATE_PROP_NAME] != null)
            {
                _dataTable.Fill(string.Format("TABLE_NAME = '{0}' AND OBJID = {1}", ObjectProperties.tableContext.Source, ObjectProperties.GetValue(ObjectProperties.tableContext.PkFieldName)));
                
                foreach (System.Data.DataRow dataRow in _dataTable.Rows.Cast<System.Data.DataRow>())
                {
                    dataRow.Delete();
                }

                foreach (var r in ObjectProperties[ObjectProperties.STATE_PROP_NAME].value.GroupBy(p => p.value).
                    Select(p => new { state = p.Key, fields = p.Select(s=>s.key).ToString(",") }))
                {
                    _dataTable.Rows.Add(ObjectProperties.tableContext.Source, r.state, ObjectProperties.GetValue(ObjectProperties.tableContext.PkFieldName), r.fields);
                }

                _dataTable.Update();

                _dataTable.Clear();
            }
        }


        #endregion
    }
}
