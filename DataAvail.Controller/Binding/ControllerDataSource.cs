using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAvail.XObject;
using DataAvail.XtraBindings;

namespace DataAvail.Controllers.Binding
{
    public class ControllerDataSource
    {
        internal ControllerDataSource(Controller Controller, XOFieldContext XOFieldContext)
        {
            _controller = Controller;

            _xOFieldContext = XOFieldContext;
        }

        private readonly Controller _controller;

        private readonly XOFieldContext _xOFieldContext;

        private ControllerDataSourceProperties _dataSourceProps;

        private CCExtBindingProperties _extBindingProps;

        private XOFieldContext XOFieldContext
        {
            get { return _xOFieldContext; }
        }

        private Controller Controller
        {
            get { return _controller; }
        }

        public object EditValueDataSource
        {
            get { return _controller.XtraBinding.BindingSource; }
        }

        public object DataSourceProperties
        {
            get 
            {
                if (_dataSourceProps == null)
                {
                    XORelation rel = XOFieldContext.ParentRelation;

                    if (rel != null)
                    {
                        _dataSourceProps = new RelationDataSourceProperits(rel);
                    }
                }

                return _dataSourceProps;
            }
        }

        public object ControlProperties
        {
            get 
            {
                if (_extBindingProps == null)
                {
                    _extBindingProps = new CCExtBindingProperties(XOFieldContext);
                }

                return _extBindingProps.ControlProperties;
            }
        }

        internal CCExtBindingProperties ExtBindingProperties
        {
            get { return _extBindingProps; }
        }

        private class RelationDataSourceProperits : ControllerDataSourceProperties
        {
            internal RelationDataSourceProperits(XORelation XORelation)
            {
                this.DataSource = GetParentRelationDataSource(XORelation);

                this.ValueMember = XORelation.ParentField.Name;

                this.DisplayMember = XORelation.DisplayedField;

                this.Filter = XORelation.Filter;
            }

            private static object GetParentRelationDataSource(XORelation ParentRelation)
            {
                object dataSource = Controller.controllerContext.GetDataSource(ParentRelation.ParentTable);

                if (dataSource == null)
                    throw new Exception("Uh huh data source fro parent table not found. Please confirm it is defined in model file");

                return XtraBinding.ItemAdapter.GetBindingListView(dataSource);
            }

        }
    }
}
