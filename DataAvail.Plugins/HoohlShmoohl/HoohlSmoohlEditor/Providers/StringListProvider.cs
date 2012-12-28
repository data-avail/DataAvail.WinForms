using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.DXEditors;

namespace HoohlShmoohlEditor.Providers
{
    internal class StringListProvider : IGoogleLikeComboDataProvider
    {
        private readonly static string[] CityTypeList = new string [] {"город", "деревня", "поселок", "поселение", "село"};

        private readonly static string[] StreetTypeList = new string[] { "улица", "площадь", "проспект", "проезд", "тупик", "набережная", "бульвар", "шоссе" };

        internal readonly static StringListProvider CityTypeProvider = new StringListProvider(CityTypeList);

        internal readonly static StringListProvider StreetTypeProvider = new StringListProvider(StreetTypeList);

        internal StringListProvider(string[] StringList)
        {
            _stringList = StringList.OrderBy(p=>p).ToArray();
        }

        private string[] _stringList;

        #region IGoogleLikeComboDataProvider Members

        public GoogleLikeComboData[] GetData(string Expression, int TopCount)
        {
            return _stringList.Select((p,i)=>new GoogleLikeComboData() { Key = i, Text = p, DropDownText = p,
                Markers = !string.IsNullOrEmpty(Expression) && p.ToUpper().StartsWith(Expression.ToUpper()) ? 
                new GoogleLikeComboMarker [] {new GoogleLikeComboMarker(0, Expression.Length) } : null } 
                ).ToArray();
        }

        #endregion
    }
}
