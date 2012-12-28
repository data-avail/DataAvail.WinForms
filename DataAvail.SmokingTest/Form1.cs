using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.SmokingTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                System.Xml.Linq.XElement el = System.Xml.Linq.XDocument.Load(@"..\..\AppModel\AppModel.xml").Root;

                DataAvail.XObject.XOP.XOPDataSet dataSet = new DataAvail.XObject.XOP.XOPDataSet(el);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Assert(false, ex.Message);

                throw;
            }

            
        }
    }
}
