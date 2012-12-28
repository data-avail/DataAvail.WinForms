using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Controllers.Binding;
using DataAvail.XObject;
using DataAvail.DXEditors;
using System.Data;
using System.ComponentModel;
using System.Windows.Forms;

namespace DataAvail.DX.XtraEditors
{
    public class LikeLookUpEdit : AutoRefCombo, IControllerDataSourceControl
    {
        public LikeLookUpEdit(XOFieldContext XOFieldContext)
            : base(XOFieldContext)
        {}

        #region IControllerDataSourceControl Members

        public ControllerDataSourceProperties DataSourceProperties
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region LikeLookUpEditDataSourceProperties

        private class LikeLookUpEditDataSourceProperties : ControllerDataSourceProperties
        {
            internal LikeLookUpEditDataSourceProperties(AutoRefCombo AutoRefCombo)
            {
                _autoRefCombo = AutoRefCombo;
            }

            private readonly AutoRefCombo _autoRefCombo;

            private AutoRefCombo AutoRefCombo
            {
                get { return _autoRefCombo; }
            }

            protected override void OnDataSourceChanged()
            {
                //this.LookUpEdit.DxEdit.Properties.DataSource = new BindingSource(DataSource, null);
            }

            protected override void OnValueMemberChanged()
            {
                //this.LookUpEdit.DxEdit.Properties.ValueMember = ValueMember;
            }

            protected override void OnDisplayMemberChanged()
            {
                /*
                this.LookUpEdit.DxEdit.Properties.DisplayMember = DisplayMember;

                this.LookUpEdit.DxEdit.Properties.Columns.Clear();
                this.LookUpEdit.DxEdit.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(DisplayMember));
                 */
            }

            protected override void OnFilterChanged()
            {
                if (DataSourceListView != null)
                    DataSourceListView.Filter = this.Filter;
            }

            private IBindingListView DataSourceListView
            {
                get { return null;}//return ((BindingSource)this.AutoRefCombo.DxEdit.Properties.DataS.Properties.DataSource).DataSource as IBindingListView; }
            }
        }


        #endregion

        #region LikeLookUpEditDataProvider

        internal class LikeLookUpEditDataProvider : IAutoRefComboDataProvider
        {
            internal LikeLookUpEditDataProvider(XOFieldContext FieldContext)
            {
                _fieldContex = FieldContext;
            }

            private DataTable _dataTable;

            private readonly XOFieldContext _fieldContex;

            public DataTable DataTable
            {
                get { return _dataTable; }

                set { _dataTable = value; }
            }

            public XOFieldContext AppFieldContext
            {
                get { return _fieldContex; }
            }
            #region IAutoRefComboDataProvider Members

            public GoogleLikeComboData GetData(object Key)
            {
                System.Data.DataRow dr = DataTable.Rows.Cast<DataRow>().SingleOrDefault(p => p[AppFieldContext.ParentRelation.ParentField.Name] == Key);

                if (dr != null)
                    return new GoogleLikeComboData() { Key = Key, Text = dr[AppFieldContext.ParentRelation.DisplayedField].ToString(), DropDownText = dr[AppFieldContext.ParentRelation.DisplayedField].ToString() };
                else
                    return null;
            }

            #endregion

            #region IGoogleLikeComboDataProvider Members

            public GoogleLikeComboData[] GetData(string Expression, int TopCount)
            {
                var rows = DataTable.Rows.Cast<DataRow>().Where(
                    p => p.Field<string>(AppFieldContext.ParentRelation.DisplayedField).ToUpper().Contains(Expression.ToUpper()));

                rows = rows.Union(DataTable.Rows.Cast<DataRow>().Except(rows));

                return rows.Select(p =>
                    new GoogleLikeComboData() { Key = p[AppFieldContext.ParentRelation.ChildField.Name], Text = p[AppFieldContext.ParentRelation.DisplayedField].ToString(), DropDownText = p[AppFieldContext.ParentRelation.DisplayedField].ToString() })
                    .OrderBy(p => p.Text).ToArray();
            }

            public int GetHighlighStart(string Text, string Expression)
            {
                int startIndex = Text.ToUpper().IndexOf(Expression.ToUpper());

                return startIndex;
            }

            #endregion
        }

        #endregion

    }
}
