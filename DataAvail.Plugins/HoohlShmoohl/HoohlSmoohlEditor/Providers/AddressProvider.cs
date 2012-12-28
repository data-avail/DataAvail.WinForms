using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.DXEditors;
using HoohlShmoohl;

namespace HoohlShmoohlEditor.Providers
{
    class AddressProvider : IGoogleLikeComboDataProvider
    {
        #region IGoogleLikeComboDataProvider Members

        public GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            try
            {
                return AddressQuery.GetAddressesSuggestions(Expression, null, null, 15, AddressTargetType.Arbitrary)
                    .Select((p, i) => new GoogleLikeComboData() 
                    { Key = i, Text = p.Suggestion, DropDownText = p.Suggestion, 
                        Markers = p.Markers.Select( s => new GoogleLikeComboMarker(s.Start, s.End)).ToArray() }).ToArray();
            }
            catch 
            {
                return new GoogleLikeComboData[] { };
            }
        }

        #endregion
    }
}
