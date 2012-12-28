using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.WinUtils
{
    public partial class AskExitForm : Form
    {
        public AskExitForm()
        {
            InitializeComponent();
        }

        private AskExitFormResult _askExitFormResult = AskExitFormResult.JustExit;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _askExitFormResult = AskExitFormResult.Reject;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _askExitFormResult = AskExitFormResult.Save;

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _askExitFormResult = AskExitFormResult.Reject;

            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _askExitFormResult = AskExitFormResult.EndEdit;

            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _askExitFormResult = AskExitFormResult.CancelEdit;

            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _askExitFormResult = AskExitFormResult.CancelExit;

            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _askExitFormResult = AskExitFormResult.JustExit;

            this.Close();
        }


        public AskExitFormResult AskExit(AskExitFormObjectState ObjectState, AskExitFormResult AdmittedResults)
        {
            /*
            this.button1.Enabled = this.button2.Enabled = this.button3.Enabled = this.button4.Enabled = this.button5.Enabled = this.button6.Enabled = false;

            if ((ProhibitedResults & AskExitFormResult.Save) == AskExitFormResult.Save)
                this.button1.Enabled = true;

            if ((ProhibitedResults & AskExitFormResult.Reject) == AskExitFormResult.Reject)
                this.button2.Enabled = true;

            if ((ProhibitedResults & AskExitFormResult.EndEdit) == AskExitFormResult.EndEdit)
                this.button3.Enabled = true;

            if ((ProhibitedResults & AskExitFormResult.CancelEdit) == AskExitFormResult.CancelEdit)
                this.button4.Enabled = true;

            if ((ProhibitedResults & AskExitFormResult.CancelExit) == AskExitFormResult.CancelExit)
                this.button5.Enabled = true;

            if ((ProhibitedResults & AskExitFormResult.JustExit) == AskExitFormResult.JustExit)
                this.button6.Enabled = true;
             */

            this.button1.Visible = this.button2.Visible = this.button3.Visible = this.button4.Visible = this.button5.Visible = this.button6.Visible = false;

            if ((AdmittedResults & AskExitFormResult.Save) == AskExitFormResult.Save)
                this.button1.Visible = true;

            if ((AdmittedResults & AskExitFormResult.Reject) == AskExitFormResult.Reject)
                this.button2.Visible = true;

            if ((AdmittedResults & AskExitFormResult.EndEdit) == AskExitFormResult.EndEdit)
                this.button3.Visible = true;

            if ((AdmittedResults & AskExitFormResult.CancelEdit) == AskExitFormResult.CancelEdit)
                this.button4.Visible = true;

            if ((AdmittedResults & AskExitFormResult.CancelExit) == AskExitFormResult.CancelExit)
                this.button5.Visible = true;

            if ((AdmittedResults & AskExitFormResult.JustExit) == AskExitFormResult.JustExit)
                this.button6.Visible = true;


            this.ShowDialog();

            return _askExitFormResult;
        }

        public static AskExitFormResult Ask(AskExitFormObjectState ObjectState, AskExitFormResult EnabledResults)
        {
            AskExitForm form = new AskExitForm();

            return form.AskExit(ObjectState, EnabledResults);
        }

        public static AskExitFormResult Ask(AskExitFormResult EnabledResults)
        {
            return Ask(AskExitFormObjectState.Unchanged, EnabledResults);
        }


    }

    public enum AskExitFormObjectState
    {
        Modifyed,
        Unchanged,
        Edit,
        ModifyedEdit
    }

    [Flags]
    public enum AskExitFormResult
    {
        None = 0x00,
        Save = 0x01,
        Reject = 0x02,
        EndEdit = 0x04,
        CancelEdit = 0x08,
        CancelExit = 0x10,
        JustExit = 0x20
    }
}
