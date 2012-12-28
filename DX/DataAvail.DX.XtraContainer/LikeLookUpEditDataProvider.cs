using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.DXEditors;
using DataAvail.XObject;
using System.Data;
using DataAvail.Linq;
using System.ComponentModel;

namespace DataAvail.DX.XtraContainer
{
    internal class LikeLookUpEditDataProvider : IAutoRefComboDataProvider
    {
        internal LikeLookUpEditDataProvider(IBindingListView BindingListView, XOFieldContext FieldContext)
        {
            _bindingListView = BindingListView;

            _fieldContex = FieldContext;
        }

        private readonly IBindingListView _bindingListView;

        private readonly XOFieldContext _fieldContex;

        public IEnumerable<DataRow> Rows
        {
            get { return _bindingListView.Cast<DataRowView>().Select(p=>p.Row); }
        }

        public XOFieldContext AppFieldContext
        {
            get { return _fieldContex; }
        } 
      #region IAutoRefComboDataProvider Members

        public DataAvail.DXEditors.GoogleLikeComboData GetData(object Key)
        {
            System.Data.DataRow dr = Rows.SingleOrDefault(p => p[AppFieldContext.ParentRelation.ParentField.Name] == Key);

            if (dr != null)
                return new GoogleLikeComboData() { Key = Key, Text = dr[AppFieldContext.ParentRelation.DisplayedField].ToString(), DropDownText = dr[AppFieldContext.ParentRelation.DisplayedField].ToString() };
            else
                return null;
        }

        #endregion

        #region IGoogleLikeComboDataProvider Members

        public DataAvail.DXEditors.GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            string displayField = AppFieldContext.ParentRelation.DisplayedField;

            var rows = Rows.Where(
                p => p.Field<string>(displayField)
                    .ToUpper().Contains(Expression.ToUpper()))
                    .OrderBy(p => p.Field<string>(displayField));

            var rows1 = rows.Union(Rows.Except(rows).OrderBy(p => p.Field<string>(displayField)));

            return rows1.Select(p =>
                new DataAvail.DXEditors.GoogleLikeComboData() 
                { 
                    Key = p[AppFieldContext.ParentRelation.ParentField.Name], 
                    Text = p[AppFieldContext.ParentRelation.DisplayedField].ToString(), 
                    DropDownText = p[AppFieldContext.ParentRelation.DisplayedField].ToString(),
                    Markers = GetMarkers(p[AppFieldContext.ParentRelation.DisplayedField].ToString(), Expression)
                })
                .ToArray();
        }

        #endregion

        internal static DataAvail.DXEditors.GoogleLikeComboMarker[] GetMarkers(string Text, string Expression)
        {
            int s = Text.ToString().IndexOf(Expression);

            if (s != -1)
            {
                return new DataAvail.DXEditors.GoogleLikeComboMarker[] { new DataAvail.DXEditors.GoogleLikeComboMarker(s, s + Expression.Length) };
            }

            return null;
        }

    }
}
