using System;
using HoohlShmoohlEditor.Providers;
using HoohlShmoohl;

namespace HoohlShmoohlEditor
{
    internal partial class HoohlShmoohlController
    {
        private static string SHOW_ON_MAP_GOOGLE_REQ = "http://maps.google.com/maps?hl=ru&q={0}";
        private static string SHOW_ON_MAP_YANDEX_REQ = "http://maps.yandex.ru/?text={0}";
        private static string PRINT_MAP_GOOGLE_REQ = "http://maps.google.com/maps?hl=ru&q={0}&ie=UTF8&z=16&pw=2";
        private static string PRINT_MAP_YANDEX_REQ = "http://maps.yandex.ru/print/?text={0}&l=map&z=16";

        private HoohlShmoohlModel _model;
        
        private AddressInfoProvider _addressProvider;

        private bool _frozen = false;

        internal HoohlShmoohlController()
        {
            _model = new HoohlShmoohlModel();

            _model.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_model_PropertyChanged);

            _addressProvider = new AddressInfoProvider(this);
        }

        internal bool Frozen
        {
            get { return _frozen; }
            set { _frozen = value; }
        }

        void _model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnModelPropertyChanged(e);

            if (!Frozen)
                this.HandleModelPropertyChanged(e.PropertyName);
        }

        protected virtual void OnModelPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ModelPropertyChanged != null)
                ModelPropertyChanged(this, e);
        }

        internal event System.ComponentModel.PropertyChangedEventHandler ModelPropertyChanged;

        internal AddressInfoProvider AddressProvider
        {
            get { return _addressProvider; }
        }

        internal HoohlShmoohlModel Model
        {
            get { return _model; }
        }

        internal void ShowOnMap(bool IsGoogle)
        {
            string req = GetReq(IsGoogle, true);

            if (!string.IsNullOrEmpty(req))
                System.Diagnostics.Process.Start(req);
        }

        internal void PrintMap(bool IsGoogle)
        {
            string req = GetReq(IsGoogle, false);

            if (!string.IsNullOrEmpty(req))
                System.Diagnostics.Process.Start(req);
        }


        string GetReq(bool IsGoogle, bool ShowOnMapReq)
        {
            string addrStr = Model.Search;

            if (!string.IsNullOrEmpty(addrStr))
            {
                if (!string.IsNullOrEmpty(Model.Building))
                {
                    addrStr = string.Format("{0},{1}", addrStr, Model.Building);
                }

                if (IsGoogle)
                    return string.Format(ShowOnMapReq ? SHOW_ON_MAP_GOOGLE_REQ : PRINT_MAP_GOOGLE_REQ, addrStr);
                else
                    return string.Format(ShowOnMapReq ? SHOW_ON_MAP_YANDEX_REQ : PRINT_MAP_YANDEX_REQ, addrStr);
            }

            return null;
        }

        private bool IsCanGetInfoFromInternet
        {
            get
            {
                return !string.IsNullOrEmpty(Model.Country)
                    || !string.IsNullOrEmpty(Model.City)
                    || !string.IsNullOrEmpty(Model.Street);
            }
        }

        private bool IsInitializationRequired
        {
            get { return AddressInfo == null && IsCanGetInfoFromInternet; }
        }

        private void Initialize()
        {
            if (IsInitializationRequired)
                this.UpdateAddressInfo();
        }
        


        internal class AddressInfoProvider : IAddressInfoProvider
        {
            internal AddressInfoProvider(HoohlShmoohlController Controller)
            {
                _controller = Controller;
            }

            private readonly HoohlShmoohlController _controller;

            public AddressInfo AddressInfo
            {
                get 
                {
                    _controller.Initialize();

                    return _controller.AddressInfo; 
                }
            }
        }


    }
}
