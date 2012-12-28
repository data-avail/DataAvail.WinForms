using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DataAvail.XtraSearcherContainer
{
    public static class XtraSearchContainer
    {
        public static string BuildExpression(System.Windows.Forms.Control Control)
        {
            System.Text.StringBuilder qryExp = new System.Text.StringBuilder();

            string exp = null;

            foreach (System.Windows.Forms.Control ctl in Control.Controls)
            {
                if (ctl is DataAvail.XtraSearcherEditors.IXtraSearchEdit)
                {
                    exp = ((DataAvail.XtraSearcherEditors.IXtraSearchEdit)ctl).GetExpression();

                }
                else
                {
                    exp = BuildExpression(ctl);
                }

                if (!string.IsNullOrEmpty(exp))
                {
                    if (qryExp.Length == 0)
                    {
                        qryExp.Append(exp);
                    }
                    else
                    {
                        qryExp.Append(" AND ");
                        qryExp.Append(exp);
                    }
                }
            }


            return qryExp.ToString();
        }
    }
}
