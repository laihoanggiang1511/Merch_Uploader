namespace UploadTemplate
{
    partial class Ribbon2 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon2()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl1 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl2 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl3 = this.Factory.CreateRibbonDropDownItem();
            Microsoft.Office.Tools.Ribbon.RibbonDropDownItem ribbonDropDownItemImpl4 = this.Factory.CreateRibbonDropDownItem();
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btn_Browse = this.Factory.CreateRibbonButton();
            this.btn_Edit = this.Factory.CreateRibbonButton();
            this.btn_BrowseJSON = this.Factory.CreateRibbonButton();
            this.btn_SaveFile = this.Factory.CreateRibbonButton();
            this.group2 = this.Factory.CreateRibbonGroup();
            this.cbb_Language = this.Factory.CreateRibbonComboBox();
            this.btn_Translate = this.Factory.CreateRibbonButton();
            this.Dictionary = this.Factory.CreateRibbonGroup();
            this.cbb_Dictionary = this.Factory.CreateRibbonComboBox();
            this.buttonGroup2 = this.Factory.CreateRibbonButtonGroup();
            this.btn_UseDictionary = this.Factory.CreateRibbonButton();
            this.btn_EditDict = this.Factory.CreateRibbonButton();
            this.group3 = this.Factory.CreateRibbonGroup();
            this.btn_CheckTM = this.Factory.CreateRibbonButton();
            this.btn_GenerateTag = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.group2.SuspendLayout();
            this.Dictionary.SuspendLayout();
            this.buttonGroup2.SuspendLayout();
            this.group3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Groups.Add(this.group2);
            this.tab1.Groups.Add(this.Dictionary);
            this.tab1.Groups.Add(this.group3);
            this.tab1.Label = "MERCH UPLOAD";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btn_Browse);
            this.group1.Items.Add(this.btn_Edit);
            this.group1.Items.Add(this.btn_BrowseJSON);
            this.group1.Items.Add(this.btn_SaveFile);
            this.group1.Label = "File";
            this.group1.Name = "group1";
            // 
            // btn_Browse
            // 
            this.btn_Browse.Image = global::UploadTemplate.Properties.Resources.browse;
            this.btn_Browse.Label = "Browse Image";
            this.btn_Browse.Name = "btn_Browse";
            this.btn_Browse.ShowImage = true;
            this.btn_Browse.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Btn_Browse_Click);
            // 
            // btn_Edit
            // 
            this.btn_Edit.Image = global::UploadTemplate.Properties.Resources.sketch;
            this.btn_Edit.Label = "Edit Shirt";
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.ShowImage = true;
            this.btn_Edit.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_Edit_Click);
            // 
            // btn_BrowseJSON
            // 
            this.btn_BrowseJSON.Image = global::UploadTemplate.Properties.Resources.json;
            this.btn_BrowseJSON.Label = "Browse JSON";
            this.btn_BrowseJSON.Name = "btn_BrowseJSON";
            this.btn_BrowseJSON.ShowImage = true;
            this.btn_BrowseJSON.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_BrowseJSON_Click);
            // 
            // btn_SaveFile
            // 
            this.btn_SaveFile.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btn_SaveFile.Image = global::UploadTemplate.Properties.Resources.save;
            this.btn_SaveFile.Label = "Save JSON";
            this.btn_SaveFile.Name = "btn_SaveFile";
            this.btn_SaveFile.ShowImage = true;
            this.btn_SaveFile.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Btn_SaveFile_Click);
            // 
            // group2
            // 
            this.group2.Items.Add(this.cbb_Language);
            this.group2.Items.Add(this.btn_Translate);
            this.group2.Label = "Language";
            this.group2.Name = "group2";
            // 
            // cbb_Language
            // 
            ribbonDropDownItemImpl1.Label = "German";
            ribbonDropDownItemImpl2.Label = "French";
            ribbonDropDownItemImpl3.Label = "Italian";
            ribbonDropDownItemImpl4.Label = "Spanish";
            this.cbb_Language.Items.Add(ribbonDropDownItemImpl1);
            this.cbb_Language.Items.Add(ribbonDropDownItemImpl2);
            this.cbb_Language.Items.Add(ribbonDropDownItemImpl3);
            this.cbb_Language.Items.Add(ribbonDropDownItemImpl4);
            this.cbb_Language.Label = "To";
            this.cbb_Language.Name = "cbb_Language";
            this.cbb_Language.Text = null;
            // 
            // btn_Translate
            // 
            this.btn_Translate.Image = global::UploadTemplate.Properties.Resources.translate;
            this.btn_Translate.Label = "Translate";
            this.btn_Translate.Name = "btn_Translate";
            this.btn_Translate.ShowImage = true;
            this.btn_Translate.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Btn_Translate_Click);
            // 
            // Dictionary
            // 
            this.Dictionary.Items.Add(this.cbb_Dictionary);
            this.Dictionary.Items.Add(this.buttonGroup2);
            this.Dictionary.Items.Add(this.btn_EditDict);
            this.Dictionary.Label = "Quick Template";
            this.Dictionary.Name = "Dictionary";
            // 
            // cbb_Dictionary
            // 
            this.cbb_Dictionary.Label = "Template:";
            this.cbb_Dictionary.Name = "cbb_Dictionary";
            this.cbb_Dictionary.Text = null;
            // 
            // buttonGroup2
            // 
            this.buttonGroup2.Items.Add(this.btn_UseDictionary);
            this.buttonGroup2.Name = "buttonGroup2";
            // 
            // btn_UseDictionary
            // 
            this.btn_UseDictionary.Image = global::UploadTemplate.Properties.Resources.translate;
            this.btn_UseDictionary.Label = "Use";
            this.btn_UseDictionary.Name = "btn_UseDictionary";
            this.btn_UseDictionary.ShowImage = true;
            this.btn_UseDictionary.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_UseDictionary_Click);
            // 
            // btn_EditDict
            // 
            this.btn_EditDict.Image = global::UploadTemplate.Properties.Resources.sketch;
            this.btn_EditDict.Label = "Edit Template";
            this.btn_EditDict.Name = "btn_EditDict";
            this.btn_EditDict.ShowImage = true;
            this.btn_EditDict.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_EditDict_Click);
            // 
            // group3
            // 
            this.group3.Items.Add(this.btn_CheckTM);
            this.group3.Items.Add(this.btn_GenerateTag);
            this.group3.Label = "Utilities";
            this.group3.Name = "group3";
            // 
            // btn_CheckTM
            // 
            this.btn_CheckTM.Image = global::UploadTemplate.Properties.Resources.trademark;
            this.btn_CheckTM.Label = "Check Trademark (US)";
            this.btn_CheckTM.Name = "btn_CheckTM";
            this.btn_CheckTM.ShowImage = true;
            this.btn_CheckTM.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_CheckTM_Click);
            // 
            // btn_GenerateTag
            // 
            this.btn_GenerateTag.Label = "Generate Tag";
            this.btn_GenerateTag.Name = "btn_GenerateTag";
            this.btn_GenerateTag.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btn_GenerateTag_Click);
            // 
            // Ribbon2
            // 
            this.Name = "Ribbon2";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon2_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.group2.ResumeLayout(false);
            this.group2.PerformLayout();
            this.Dictionary.ResumeLayout(false);
            this.Dictionary.PerformLayout();
            this.buttonGroup2.ResumeLayout(false);
            this.buttonGroup2.PerformLayout();
            this.group3.ResumeLayout(false);
            this.group3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Browse;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Translate;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_Edit;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_SaveFile;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox cbb_Language;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_CheckTM;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Dictionary;
        internal Microsoft.Office.Tools.Ribbon.RibbonComboBox cbb_Dictionary;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_EditDict;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_UseDictionary;
        internal Microsoft.Office.Tools.Ribbon.RibbonButtonGroup buttonGroup2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_BrowseJSON;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group3;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btn_GenerateTag;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon2 Ribbon2
        {
            get { return this.GetRibbon<Ribbon2>(); }
        }
    }
}
