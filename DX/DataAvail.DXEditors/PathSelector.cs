using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using DataAvail.Linq;

namespace DataAvail.DXEditors.DX
{
    public class PathSelector : DevExpress.XtraEditors.ButtonEdit
    {    
        static PathSelector()
        {
            RepositoryItemPathSelector.Register();
        }

        public PathSelector()
        {
        }

        public override string EditorTypeName
        {
            get { return RepositoryItemPathSelector.EditorName; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public new RepositoryItemPathSelector Properties
        {
            get { return base.Properties as RepositoryItemPathSelector ; }
        }

        [UserRepositoryItem("Register")]
        public class RepositoryItemPathSelector : DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
        {

            static RepositoryItemPathSelector()
            {
                Register();
            }

            public RepositoryItemPathSelector()
            {
                this.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                    new DevExpress.XtraEditors.Controls.EditorButton()});
                this.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                this.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisPath;
                this.AppearanceDisabled.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisPath;

                ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(RepositoryItemPathSelector_ButtonClick);
            }

            private readonly PathSelectorParams _selectorParams = new PathSelectorParams();

            public event PathSelectorFileSelectedHandler FileSelected;

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public PathSelectorParams SelectorParams
            {
                get { return _selectorParams; }
            }

            internal const string EditorName = "DARepositoryItemPathSelector";

            public static void Register()
            {
                EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName,
                typeof(PathSelector),
                typeof(RepositoryItemPathSelector),
                typeof(PathSelectorViewInfo),
                new PathSelectorPainter(), true));
            }

            public override string EditorTypeName
            {
                get { return EditorName; }
            }

            private void OnFileSelected(string FileName)
            {
                if (FileSelected != null)
                    FileSelected(this, new PathSelectorFileSelectedEventArgs(FileName));
            }

            public override bool ReadOnly
            {
                get
                {
                    return base.ReadOnly;
                }
                set
                {
                    base.ReadOnly = value;

                    this.Buttons[0].Enabled = !this.ReadOnly;
                }
            }

            void RepositoryItemPathSelector_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
            {
                if (SelectorParams.IsFolder)
                {
                    FolderBrowserDialog dlg = new FolderBrowserDialog()
                    {
                        ShowNewFolderButton = !SelectorParams.IsOpen
                    };

                    if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.SelectedPath))
                    {
                        this.OwnerEdit.Text = dlg.SelectedPath;
                    }
                }
                else
                {

                    if (SelectorParams.IsOpen)
                    {
                        OpenFileDialog dlg = new OpenFileDialog()
                        {
                            InitialDirectory = SelectorParams.DefaultPath,
                            Filter = !string.IsNullOrEmpty(SelectorParams.Filter) ? SelectorParams.Filter.Split(',').Select(p => string.Format("(*.{0})|*.{0}", p)).ToString(",") : null,
                            CheckPathExists = true,
                            DefaultExt = SelectorParams.Filter
                        };

                        if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
                        {
                            this.OwnerEdit.Text = dlg.FileName;

                            OnFileSelected(dlg.FileName);
                        }

                    }
                    else
                    {
                        SaveFileDialog dlg = new SaveFileDialog()
                        {
                            InitialDirectory = SelectorParams.DefaultPath,
                            Filter = !string.IsNullOrEmpty(SelectorParams.Filter) ? SelectorParams.Filter.Split(',').Select(p => string.Format("(*.{0})|*.{0}", p)).ToString(",") : null,
                            CheckPathExists = true,
                            DefaultExt = SelectorParams.Filter
                        };

                        if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
                        {
                            this.OwnerEdit.Text = dlg.FileName;

                            OnFileSelected(dlg.FileName);
                        }

                    }
                }
            }

            public override BaseEditViewInfo CreateViewInfo()
            {
                return new PathSelectorViewInfo(this);
            }

            public override BaseEditPainter CreatePainter()
            {
                return new PathSelectorPainter();
            }

            public class PathSelectorPainter : DevExpress.XtraEditors.Drawing.ButtonEditPainter
            {
            }

            public class PathSelectorViewInfo : DevExpress.XtraEditors.ViewInfo.ButtonEditViewInfo
            {
                public PathSelectorViewInfo(DevExpress.XtraEditors.Repository.RepositoryItem owner) : base(owner) { }
            }
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PathSelectorParams
    {
        public PathSelectorParams()
        {
            IsOpen = true;
        }

        public bool IsOpen { get; set; }

        public bool IsFolder { get; set; }

        public string DefaultPath { get; set; }

        public string Filter { get; set; }
    }

    public class PathSelectorFileSelectedEventArgs : System.EventArgs
    {
        internal PathSelectorFileSelectedEventArgs(string FileName)
        {
            this.fileName = FileName;
        }

        public readonly string fileName;
    }


    public delegate void PathSelectorFileSelectedHandler(object sender, PathSelectorFileSelectedEventArgs args);
}