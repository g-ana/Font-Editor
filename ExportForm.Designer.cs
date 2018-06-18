namespace FntEditor
{
    partial class ExportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            this.labelFontName = new System.Windows.Forms.Label();
            this.textBoxFontName = new System.Windows.Forms.TextBox();
            this.labelExportFormat = new System.Windows.Forms.Label();
            this.comboBoxDisplayType = new System.Windows.Forms.ComboBox();
            this.labelHeaderFile = new System.Windows.Forms.Label();
            this.textBoxHeaderFilename = new System.Windows.Forms.TextBox();
            this.buttonSetPath = new System.Windows.Forms.Button();
            this.buttonDoExport = new System.Windows.Forms.Button();
            this.groupBoxExportFormat = new System.Windows.Forms.GroupBox();
            this.labelBrackets = new System.Windows.Forms.Label();
            this.textBoxVarName = new System.Windows.Forms.TextBox();
            this.checkBox_PROGMEM = new System.Windows.Forms.CheckBox();
            this.label_byte = new System.Windows.Forms.Label();
            this.checkBox_static = new System.Windows.Forms.CheckBox();
            this.checkBox_const = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonHEX = new System.Windows.Forms.RadioButton();
            this.radioButtonDEC = new System.Windows.Forms.RadioButton();
            this.radioButtonBIN = new System.Windows.Forms.RadioButton();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.saveFileDialogExport = new System.Windows.Forms.SaveFileDialog();
            this.groupBoxExportFormat.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelFontName
            // 
            resources.ApplyResources(this.labelFontName, "labelFontName");
            this.labelFontName.Name = "labelFontName";
            // 
            // textBoxFontName
            // 
            resources.ApplyResources(this.textBoxFontName, "textBoxFontName");
            this.textBoxFontName.Name = "textBoxFontName";
            this.textBoxFontName.ReadOnly = true;
            this.textBoxFontName.TabStop = false;
            // 
            // labelExportFormat
            // 
            resources.ApplyResources(this.labelExportFormat, "labelExportFormat");
            this.labelExportFormat.Name = "labelExportFormat";
            // 
            // comboBoxDisplayType
            // 
            resources.ApplyResources(this.comboBoxDisplayType, "comboBoxDisplayType");
            this.comboBoxDisplayType.FormattingEnabled = true;
            this.comboBoxDisplayType.Items.AddRange(new object[] {
            resources.GetString("comboBoxDisplayType.Items"),
            resources.GetString("comboBoxDisplayType.Items1")});
            this.comboBoxDisplayType.Name = "comboBoxDisplayType";
            // 
            // labelHeaderFile
            // 
            resources.ApplyResources(this.labelHeaderFile, "labelHeaderFile");
            this.labelHeaderFile.Name = "labelHeaderFile";
            // 
            // textBoxHeaderFilename
            // 
            resources.ApplyResources(this.textBoxHeaderFilename, "textBoxHeaderFilename");
            this.textBoxHeaderFilename.Name = "textBoxHeaderFilename";
            // 
            // buttonSetPath
            // 
            resources.ApplyResources(this.buttonSetPath, "buttonSetPath");
            this.buttonSetPath.Name = "buttonSetPath";
            this.buttonSetPath.UseVisualStyleBackColor = true;
            this.buttonSetPath.Click += new System.EventHandler(this.buttonSetPath_Click);
            // 
            // buttonDoExport
            // 
            resources.ApplyResources(this.buttonDoExport, "buttonDoExport");
            this.buttonDoExport.Name = "buttonDoExport";
            this.buttonDoExport.UseVisualStyleBackColor = true;
            this.buttonDoExport.Click += new System.EventHandler(this.buttonDoExport_Click);
            // 
            // groupBoxExportFormat
            // 
            resources.ApplyResources(this.groupBoxExportFormat, "groupBoxExportFormat");
            this.groupBoxExportFormat.Controls.Add(this.labelBrackets);
            this.groupBoxExportFormat.Controls.Add(this.textBoxVarName);
            this.groupBoxExportFormat.Controls.Add(this.checkBox_PROGMEM);
            this.groupBoxExportFormat.Controls.Add(this.label_byte);
            this.groupBoxExportFormat.Controls.Add(this.checkBox_static);
            this.groupBoxExportFormat.Controls.Add(this.checkBox_const);
            this.groupBoxExportFormat.Name = "groupBoxExportFormat";
            this.groupBoxExportFormat.TabStop = false;
            // 
            // labelBrackets
            // 
            resources.ApplyResources(this.labelBrackets, "labelBrackets");
            this.labelBrackets.Name = "labelBrackets";
            // 
            // textBoxVarName
            // 
            resources.ApplyResources(this.textBoxVarName, "textBoxVarName");
            this.textBoxVarName.Name = "textBoxVarName";
            // 
            // checkBox_PROGMEM
            // 
            resources.ApplyResources(this.checkBox_PROGMEM, "checkBox_PROGMEM");
            this.checkBox_PROGMEM.Name = "checkBox_PROGMEM";
            this.checkBox_PROGMEM.UseVisualStyleBackColor = true;
            // 
            // label_byte
            // 
            resources.ApplyResources(this.label_byte, "label_byte");
            this.label_byte.Name = "label_byte";
            // 
            // checkBox_static
            // 
            resources.ApplyResources(this.checkBox_static, "checkBox_static");
            this.checkBox_static.Name = "checkBox_static";
            this.checkBox_static.UseVisualStyleBackColor = true;
            // 
            // checkBox_const
            // 
            resources.ApplyResources(this.checkBox_const, "checkBox_const");
            this.checkBox_const.Name = "checkBox_const";
            this.checkBox_const.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioButtonHEX);
            this.groupBox1.Controls.Add(this.radioButtonDEC);
            this.groupBox1.Controls.Add(this.radioButtonBIN);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtonHEX
            // 
            resources.ApplyResources(this.radioButtonHEX, "radioButtonHEX");
            this.radioButtonHEX.Name = "radioButtonHEX";
            this.radioButtonHEX.TabStop = true;
            this.radioButtonHEX.UseVisualStyleBackColor = true;
            // 
            // radioButtonDEC
            // 
            resources.ApplyResources(this.radioButtonDEC, "radioButtonDEC");
            this.radioButtonDEC.Checked = true;
            this.radioButtonDEC.Name = "radioButtonDEC";
            this.radioButtonDEC.TabStop = true;
            this.radioButtonDEC.UseVisualStyleBackColor = true;
            // 
            // radioButtonBIN
            // 
            resources.ApplyResources(this.radioButtonBIN, "radioButtonBIN");
            this.radioButtonBIN.Name = "radioButtonBIN";
            this.radioButtonBIN.TabStop = true;
            this.radioButtonBIN.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // saveFileDialogExport
            // 
            resources.ApplyResources(this.saveFileDialogExport, "saveFileDialogExport");
            // 
            // ExportForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxExportFormat);
            this.Controls.Add(this.buttonDoExport);
            this.Controls.Add(this.buttonSetPath);
            this.Controls.Add(this.textBoxHeaderFilename);
            this.Controls.Add(this.labelHeaderFile);
            this.Controls.Add(this.comboBoxDisplayType);
            this.Controls.Add(this.labelExportFormat);
            this.Controls.Add(this.textBoxFontName);
            this.Controls.Add(this.labelFontName);
            this.Name = "ExportForm";
            this.groupBoxExportFormat.ResumeLayout(false);
            this.groupBoxExportFormat.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelFontName;
        private System.Windows.Forms.TextBox textBoxFontName;
        private System.Windows.Forms.Label labelExportFormat;
        private System.Windows.Forms.ComboBox comboBoxDisplayType;
        private System.Windows.Forms.Label labelHeaderFile;
        private System.Windows.Forms.TextBox textBoxHeaderFilename;
        private System.Windows.Forms.Button buttonSetPath;
        private System.Windows.Forms.Button buttonDoExport;
        private System.Windows.Forms.GroupBox groupBoxExportFormat;
        private System.Windows.Forms.TextBox textBoxVarName;
        private System.Windows.Forms.CheckBox checkBox_PROGMEM;
        private System.Windows.Forms.Label label_byte;
        private System.Windows.Forms.CheckBox checkBox_static;
        private System.Windows.Forms.CheckBox checkBox_const;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonHEX;
        private System.Windows.Forms.RadioButton radioButtonDEC;
        private System.Windows.Forms.RadioButton radioButtonBIN;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelBrackets;
        private System.Windows.Forms.SaveFileDialog saveFileDialogExport;
    }
}