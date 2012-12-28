using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.Linq;
using DataAvail.XObject;

namespace DataAvail.DX.XtraContainer
{
    internal class AutoRefComboDataProvider : DataAvail.DXEditors.IAutoRefComboDataProvider
    {
        internal AutoRefComboDataProvider(XOFieldContext FieldContext)
        {
            _appFieldContext = FieldContext;
        }

        private readonly XOFieldContext _appFieldContext;

        public XOFieldContext AppFieldContext
        {
            get { return _appFieldContext; }
        } 

        #region IAutoRefComboDataProvider Members

        public DataAvail.DXEditors.GoogleLikeComboData GetData(object Key)
        {
            object text = DataAvail.Data.DbContext.DbContext.GetScalar(
                "SELECT {0} FROM {1} WHERE {2} = {3}", 
                AppFieldContext.ParentRelation.DisplayedField, 
                AppFieldContext.ParentRelation.ParentTable.Source,
                AppFieldContext.ParentRelation.ParentField.Name,
                Key);

            if (text != null)
            {
                return new DataAvail.DXEditors.GoogleLikeComboData() { Key = Key, Text = text.ToString(), DropDownText = text.ToString() };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region IGoogleLikeComboDataProvider Members

        public DataAvail.DXEditors.GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            System.Data.DataTable data = DataAvail.Data.DbContext.DbContext.GetDataTop(
                //"SELECT TOP({0}) {1}, {2} FROM {3} WHERE UPPER({2}) LIKE ('%{4}'){5}",
                "SELECT {0}, {1} FROM {2} WHERE UPPER({1}) LIKE ('%{3}'){4}",
                TopCount,
                AppFieldContext.ParentRelation.ParentField.Name,
                AppFieldContext.ParentRelation.DisplayedField,
                AppFieldContext.ParentRelation.ParentTable.Source,
                Expression.ToUpper(),
                string.IsNullOrEmpty(AppFieldContext.ParentRelation.Filter) ? null :
                    string.Format(" AND {0}", AppFieldContext.ParentRelation.Filter)
                );

            System.Data.DataTable nextOnes = null;

            if (data.Rows.Count < TopCount)
            {
                string excl = data.Rows.Count == 0 ? null : string.Format(" WHERE {0} NOT IN ({1})",
                        AppFieldContext.ParentRelation.ParentField.Name,
                        data.Rows.Cast<System.Data.DataRow>().ToString(p => p[0].ToString(), ","));

                nextOnes = DataAvail.Data.DbContext.DbContext.GetDataTop(
                    //"SELECT TOP({0}) {1}, {2} FROM {3} {4}{5}",
                    "SELECT {0}, {1} FROM {2} {3}{4}",
                    TopCount - data.Rows.Count,
                    AppFieldContext.ParentRelation.ParentField.Name,
                    AppFieldContext.ParentRelation.DisplayedField,
                    AppFieldContext.ParentRelation.ParentTable.Source,
                    excl
                    ,string.IsNullOrEmpty(AppFieldContext.ParentRelation.Filter) ? null :
                    string.Format(" {0} {1}", string.IsNullOrEmpty(excl) ? "WHERE" : "AND", AppFieldContext.ParentRelation.Filter)
                    );
            }

            var r = data.Rows.Cast<System.Data.DataRow>();

            if (nextOnes != null)
                r = r.Union(nextOnes.Rows.Cast<System.Data.DataRow>());

            return r.Select(p =>
                new DataAvail.DXEditors.GoogleLikeComboData()
                {
                    Key = p[0],
                    Text = p[1].ToString(),
                    DropDownText = p[1].ToString(),
                    Markers = LikeLookUpEditDataProvider.GetMarkers(p[1].ToString(), Expression)
                })
                .OrderBy(p => p.Text).ToArray();
        }

        #endregion

    }
}
