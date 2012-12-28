using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils
{
    public class String
    {
        public static string FormatMask(string Mask)
        {
            if (Mask[0] == ',')
            {
                string mask = Mask.Trim(new[] { ',', ' ' });

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                int iPt = mask.IndexOf('.');

                for (int i = iPt == -1 ? mask.Length : iPt; i > 0; i -= 3)
                {
                    int k = 3;

                    if (i < 3)
                    {
                        k = i;
                    }

                    sb.Insert(0, mask.Substring(i - k, k));

                    sb.Insert(0, " ");
                }

                return string.Format("{0}{1}", sb.ToString().Trim(new[] { ' ', ',' }), iPt == -1 ? "" : mask.Substring(iPt, mask.Length - iPt));
            }
            else
            {
                return Mask;
            }
        }
    }
}
