using System;
using System.Collections.Generic;
using System.Text;

namespace DataAvail.WinUtils.DbConnectForm
{
    partial class DbConnectForm
    {
        [Serializable]
        public class DbConnectFormSettings
        {
            public string Password = "";

            public string User = "";

            public string[] Servers = new string[] { "" };

            public string DefaultServer = "";

            public bool AutoConnect = false;

            public bool Remember = false;
        }

        private DbConnectFormSettings _dbConnectFormSettings;

        private void Serialzie(string Path)
        {
            _dbConnectFormSettings.Password = this.textBoxPassword.Text;

            _dbConnectFormSettings.DefaultServer = this.comboBoxServer.Text;

            _dbConnectFormSettings.AutoConnect = this.checkBoxAutoConnect.Checked;

            _dbConnectFormSettings.Remember = this.checkBoxRemember.Checked;

            _dbConnectFormSettings.User = this.textBoxLogin.Text;

            DataAvail.Utils.XmlSerializer<DbConnectFormSettings>.Serialize(_dbConnectFormSettings, SerializationFileName);
        }

        private void Deserialize(string Path)
        {
            _dbConnectFormSettings = DataAvail.Utils.XmlSerializer<DbConnectFormSettings>.DeSerialize(SerializationFileName);

            if (_dbConnectFormSettings == null)
            {
                _dbConnectFormSettings = new DbConnectFormSettings();
            }

            this.textBoxPassword.Text = _dbConnectFormSettings.Password;

            this.textBoxLogin.Text = !string.IsNullOrEmpty(_dbConnectFormSettings.User) ? _dbConnectFormSettings.User : _dbConnectValidate.DefaultUserName;

            this.comboBoxServer.Items.AddRange(_dbConnectFormSettings.Servers);

            this.comboBoxServer.Text = _dbConnectFormSettings.DefaultServer;

            this.checkBoxAutoConnect.Checked = _dbConnectFormSettings.AutoConnect;

            this.checkBoxRemember.Checked = _dbConnectFormSettings.Remember;

        }

        private const string SerializationFileName = @"XML/DbConnectFormSettings.xml";
    }
}
