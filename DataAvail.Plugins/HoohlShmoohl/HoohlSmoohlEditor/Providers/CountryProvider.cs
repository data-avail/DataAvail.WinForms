using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.DXEditors;
using HoohlShmoohl;

namespace HoohlShmoohlEditor.Providers
{
    class CountryProvider : IGoogleLikeComboDataProvider
    {


        #region IGoogleLikeComboDataProvider Members

        public GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            
            try
            {
                return GoogleAddressQuery.GetCountrySuggestions(Expression, 15)
                    .Select((p, i) => new GoogleLikeComboData()
                    {
                        Key = i,
                        Text = p.Suggestion,
                        DropDownText = p.Suggestion,
                        Markers = p.Markers.Select(s => new GoogleLikeComboMarker(s.Start, s.End)).ToArray()
                    }).ToArray();
            }
            catch
            {
                return new GoogleLikeComboData[] { };
            }

            return null;
        }

        #endregion
    }
}
