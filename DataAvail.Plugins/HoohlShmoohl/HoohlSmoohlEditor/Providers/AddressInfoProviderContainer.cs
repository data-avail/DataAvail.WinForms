using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HoohlShmoohl;

namespace HoohlShmoohlEditor.Providers
{
    internal abstract class AddressInfoProviderContainer 
    {
        internal AddressInfoProviderContainer(IAddressInfoProvider AddressInfoProvider)
        {
            _addressInfoProvider = AddressInfoProvider;
        }

        private readonly IAddressInfoProvider _addressInfoProvider;

        protected AddressInfo AddressInfo
        {
            get { return _addressInfoProvider.AddressInfo; }
        }
    }
}
