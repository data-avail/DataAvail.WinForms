using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baio.XtraBinding.XtraBindingController
{
    public interface IXtraBindingControllerProvider
    {
        object GetDataSource(Baio.XtraObjectProperties.XtraObjectProperties XtraObjectProperties);

        Baio.Data.IDataAdapter GetDataAdapter(Baio.XtraObjectProperties.XtraObjectProperties XtraObjectProperties);

        Baio.XtraBinding.IObjectCalculator GetObjectCalculator(Baio.XtraObjectProperties.XtraObjectProperties XtraObjectProperties);

        System.Windows.Forms.Form CreateReportForm(XtraBindingItemController ItemController, Baio.XOP.AppContext.AppItemContext AppItemContext);
    }
}
