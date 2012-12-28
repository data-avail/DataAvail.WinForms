using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Data
{
    public static class Data
    {
        public static string SqlCommandSubstituteWhere(string SrcCommand, string WhereCondition)
        {
            if (string.IsNullOrEmpty(SrcCommand))
            {
                throw new System.ArgumentException("Argument couldn't be null or empty", "SrcCommand");
            }

            string cmd = SrcCommand.ToUpper();

            int i = cmd.IndexOf("WHERE");

            if (string.IsNullOrEmpty(WhereCondition))
            {
                if (i == -1)
                {
                    return cmd;
                }
                else
                {
                    return cmd.Substring(i);
                }
            }
            else
            {
                if (i == -1)
                {
                    return string.Format("{0} WHERE {1}", cmd, WhereCondition.ToUpper());
                }
                else
                {
                    cmd = cmd.Substring(i);

                    return string.Format("{0} WHERE {1}", cmd, WhereCondition.ToUpper());
                }
            }
        }
    }
}
