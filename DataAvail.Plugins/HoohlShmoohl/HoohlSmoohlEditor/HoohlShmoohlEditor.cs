using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace HoohlShmoohlEditor
{
    public partial class HoohlShmoohlEditor : DevExpress.XtraEditors.XtraUserControl, INotifyPropertyChanged, ISupportInitialize
    {
        public HoohlShmoohlEditor()
        {
            InitializeComponent();

            _controller = new HoohlShmoohlController();

            Controller.ModelPropertyChanged += new PropertyChangedEventHandler(Model_PropertyChanged);

            Initialize();

        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    this.searchGoogleLikeCombo.TextValue = "";
                    this.searchGoogleLikeCombo.Focus();
                }));
            }
            
        }
        
        private readonly HoohlShmoohlController _controller;

        internal HoohlShmoohlController Controller
        {
            get { return _controller; }
        } 

        private HoohlShmoohlModel Model
        {
            get { return Controller.Model; }
        }

        public HoohlShmoohlModel Properties
        {
            get { return Model; }
        }

        private void Initialize()
        {

            searchGoogleLikeCombo.DataProvider = new Providers.AddressProvider();
            countryGoogleLikeCombo.DataProvider = new Providers.CountryProvider();
            cityGoogleLikeCombo.DataProvider = new Providers.CityProvider(Controller.AddressProvider);
            streetGoogleLikeCombo.DataProvider = new Providers.StreetProvider(Controller.AddressProvider);
            buildingGoogleLikeCombo.DataProvider = new Providers.BuildingsProvider(Controller.AddressProvider);
            cityTypeGoogleLikeCombo.DataProvider = Providers.StringListProvider.CityTypeProvider;
            streetTypeGoogleLikeCombo.DataProvider = Providers.StringListProvider.StreetTypeProvider;

            this.searchGoogleLikeCombo.DataBindings.Add(new Binding("TextValue", Model, "Search"));
            this.countryGoogleLikeCombo.DataBindings.Add(new Binding("TextValue", Model, "Country"));
            this.cityGoogleLikeCombo.DataBindings.Add(new Binding("TextValue", Model, "City"));
            this.cityTypeGoogleLikeCombo.DataBindings.Add(new Binding("TextValue", Model, "CityType"));
            this.streetGoogleLikeCombo.DataBindings.Add(new Binding("TextValue", Model, "Street"));
            this.streetTypeGoogleLikeCombo.DataBindings.Add(new Binding("TextValue", Model, "StreetType"));
            this.buildingGoogleLikeCombo.DataBindings.Add(new Binding("TextValue", Model, "Building"));
            this.addrMemoEdit.DataBindings.Add(new Binding("Text", Model, "Address", true, DataSourceUpdateMode.OnPropertyChanged));

            this.adminAreaTextEdit.DataBindings.Add(new Binding("Text", Model, "AdministrativeArea"));
            this.subAdminAreaTextEdit.DataBindings.Add(new Binding("Text", Model, "SubAdministrativeArea"));
            this.dependedLoclityTextEdit.DataBindings.Add(new Binding("Text", Model, "DependedLocality"));
            this.postalCodeTextEdit.DataBindings.Add(new Binding("Text", Model, "PostalCode", true, DataSourceUpdateMode.OnPropertyChanged, null));
            this.roomTextEdit.DataBindings.Add(new Binding("Text", Model, "Room"));
        }

        private void showOnMapHyperLinkEdit_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            this.ShowOnMap();
        }

        private void parintAddrHyperLinkEdit_OpenLink(object sender, DevExpress.XtraEditors.Controls.OpenLinkEventArgs e)
        {
            this.PrintMap();
        }

        public bool HandleKeyUp(KeyEventArgs KeyEventArgs)
        {
            if (KeyEventArgs.Control)
            {
                switch (KeyEventArgs.KeyCode)
                {
                    case Keys.M:
                        ShowOnMap();
                        return true;
                    case Keys.P:
                        PrintMap();
                        return true;
                }
            }

            return false;
        }

        private void ShowOnMap()
        {
            this.Validate();

            Controller.ShowOnMap(IsGoogleMapActive);

        }

        private void PrintMap()
        {
            this.Validate();

            Controller.PrintMap(IsGoogleMapActive);
        }

        private bool IsGoogleMapActive
        {
            get
            {
                return addrTypeImageComboBoxEdit.SelectedIndex == 0;
            }

            set
            {
                addrTypeImageComboBoxEdit.SelectedIndex = value ?  0 : 1;
            }
        }


        #region binding properties

        public string Country
        {
            get { return Model.Country; }
            set { Model.Country = value; }
        }

        public string AdministrativeArea
        {
            get { return Model.AdministrativeArea; }
            set { Model.AdministrativeArea = value; }
        }

        public string SubAdministrativeArea
        {
            get { return Model.SubAdministrativeArea; }
            set { Model.SubAdministrativeArea = value; }
        }

        public string DependedLocality
        {
            get { return Model.DependedLocality; }
            set { Model.DependedLocality = value; }
        }

        public string City
        {
            get { return Model.City; }
            set { Model.City = value; }
        }

        public string CityType
        {
            get { return Model.CityType; }
            set { Model.CityType = value; }
        }

        public string Street
        {
            get { return Model.Street; }
            set { Model.Street = value; }
        }

        public string StreetType
        {
            get { return Model.StreetType; }
            set { Model.StreetType = value; }
        }

        public string Building
        {
            get { return Model.Building; }
            set { Model.Building = value; }
        }

        public string Room
        {
            get { return Model.Room; }
            set { Model.Room = value; }
        }

        public string PostalCode
        {
            get { return Model.PostalCode; }
            set { Model.PostalCode = value; }
        }

        public string Address
        {
            get { return Model.Address; }
            set { Model.Address = value; }
        }

        void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }        

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        public void BeginInit()
        {
            this.Controller.Frozen = true;
        }

        public void EndInit()
        {
            this.Controller.Frozen = false;
        }
    }
}
