using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DataAvail.WinUtils.DbConnectForm
{
    public partial class DbConnectForm : System.Windows.Forms.Form
    {
        internal DbConnectForm()
        {
            InitializeComponent();
        }

        internal DbConnectForm(System.Data.IDbConnection DataConnection, string ConnectionStringTemplate, IDbConnectContext DbConnectValidate, bool IsAutoConnectAllowed)
        {
            InitializeComponent();

            _dataConnection = DataConnection;

            if (ConnectionStringTemplate == null)
                _connectionStringTemplate = "Server={0};User Id={1};Password={2};";
            else 
                _connectionStringTemplate = ConnectionStringTemplate;

            _dbConnectValidate = DbConnectValidate;

            _autoConnect = IsAutoConnectAllowed;
        }

        private bool _autoConnect = false;
   
        private readonly System.Data.IDbConnection _dataConnection;

        private readonly string _connectionStringTemplate;

        private readonly IDbConnectContext _dbConnectValidate;

        private bool _allowServerNotInList = false;
      
        /// <summary>
        /// Открывает форму для открытия соединения. В случае успеха соединения будет открыто и функция вернет значени OK, в противном 
        /// соединение открыто не будет и вернется Cancel.
        /// </summary>
        /// <param name="DataConnection">Интерфейс соединения для открытия, если соединение прошло успешно то оно будет открыто</param>
        /// <param name="ConnectionStringTemplate">Пример - "Server={0};User Id={1};Password={2};"
        /// Последовательность параметров сохранять обязательно. Т.е. Первым идет представления сервера в строке соединения,
        /// вторым имя пользователя и третьим пароль. Если данный параметр null то используется строка как в примере.</param>
        public static System.Windows.Forms.DialogResult ShowDialog(System.Data.IDbConnection DbConnection, string ConnectionStringTemplate, IDbConnectContext DbConnectValidate, bool IsAutoConnectAllowed)
        {
            DbConnectForm dbConnectForm = new DbConnectForm(DbConnection, ConnectionStringTemplate, DbConnectValidate, IsAutoConnectAllowed);

            return dbConnectForm.ShowDialog();
        }

        public static System.Windows.Forms.DialogResult ShowDialog(System.Data.IDbConnection DbConnection, bool IsAutoConnectAllowed)
        {
            return ShowDialog(DbConnection, IsAutoConnectAllowed, "XML");
        }

        public static System.Windows.Forms.DialogResult ShowDialog(System.Data.IDbConnection DbConnection, bool IsAutoConnectAllowed, string SerializationPath)
        {
            DbConnectForm dbConnectForm = new DbConnectForm(DbConnection, "Server={0};User Id={1};Password={2};", /*new SoffitConnectContext(DbConnection)*/null, IsAutoConnectAllowed) { SerializationPath = SerializationPath };

            return dbConnectForm.ShowDialog();
        }

        public static System.Windows.Forms.DialogResult ShowDialog(System.Data.IDbConnection DbConnection, IDbConnectContext DbConnectContext, bool IsAutoConnectAllowed, string SerializationPath)
        {
            DbConnectForm dbConnectForm = new DbConnectForm(DbConnection, "Server={0};User Id={1};Password={2};", DbConnectContext, IsAutoConnectAllowed) { SerializationPath = SerializationPath };

            return dbConnectForm.ShowDialog();
        }



        public bool AllowServerNotInList
        {
            get { return _allowServerNotInList; }

            set
            {
                if (_allowServerNotInList != value)
                { 
                    _allowServerNotInList = value;

                    this.comboBoxServer.DropDownStyle = _allowServerNotInList ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
          
            string err = TryOpenConnection(false);

            if (err != null)
            {
                if (System.Windows.Forms.MessageBox.Show(string.Format("При подключении к базе данных произошла ошибка : {0}. Хотите изменить параметры подключения? ", err),
                    "Ошибка соединения", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string err = TryOpenConnection(true);

            if (err == null)
            {
                System.Windows.Forms.MessageBox.Show("Подключение успешно!");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(err);
            }
        }

        private string TryOpenConnection(bool fClose)
        {
            try
            {
                _dataConnection.ConnectionString = string.Format(_connectionStringTemplate, this.comboBoxServer.Text, this.textBoxLogin.Text, this.textBoxPassword.Text);

                _dataConnection.Open();
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

            string errValidate = _dbConnectValidate == null ? null : _dbConnectValidate.Validate();

            if (errValidate != null)
            {
                fClose = true;
            }

            if (fClose) _dataConnection.Close();

            return errValidate;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Deserialize(SerializationPath);

            if (System.Windows.Forms.Form.ModifierKeys != Keys.Control && _autoConnect && checkBoxAutoConnect.Checked)
            {
                buttonOK_Click(this, EventArgs.Empty);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (!this.checkBoxRemember.Checked)
                {
                    this.textBoxPassword.Text = null;

                    this.comboBoxServer.Text = null;

                    this.checkBoxAutoConnect.Checked = false;

                    this.checkBoxRemember.Checked = false;

                    this.textBoxLogin.Text = null;


                }
                
                this.Serialzie(SerializationPath);
            }

            base.OnClosed(e);
        }

        public string SerializationPath = "XML";
    }
}