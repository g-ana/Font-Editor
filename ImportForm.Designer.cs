namespace FntEditor
{
    partial class ImportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportForm));
            this.patternHeightGroup = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.widthNumeric = new System.Windows.Forms.NumericUpDown();
            this.heightNumeric = new System.Windows.Forms.NumericUpDown();
            this.heightTable = new System.Windows.Forms.TableLayoutPanel();
            this.heightRadioButton7x5 = new System.Windows.Forms.RadioButton();
            this.heightRadioButton8x5 = new System.Windows.Forms.RadioButton();
            this.heightRadioButton8x8 = new System.Windows.Forms.RadioButton();
            this.heightRadioButton14x8 = new System.Windows.Forms.RadioButton();
            this.customHeightRadioButton = new System.Windows.Forms.RadioButton();
            this.countTable = new System.Windows.Forms.TableLayoutPanel();
            this.countLabel = new System.Windows.Forms.Label();
            this.countNumeric = new System.Windows.Forms.NumericUpDown();
            this.resultFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.patternHeightGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumeric)).BeginInit();
            this.heightTable.SuspendLayout();
            this.countTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.countNumeric)).BeginInit();
            this.resultFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // patternHeightGroup
            // 
            resources.ApplyResources(this.patternHeightGroup, "patternHeightGroup");
            this.patternHeightGroup.Controls.Add(this.label2);
            this.patternHeightGroup.Controls.Add(this.label1);
            this.patternHeightGroup.Controls.Add(this.widthNumeric);
            this.patternHeightGroup.Controls.Add(this.heightNumeric);
            this.patternHeightGroup.Controls.Add(this.heightTable);
            this.patternHeightGroup.Name = "patternHeightGroup";
            this.patternHeightGroup.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // widthNumeric
            // 
            resources.ApplyResources(this.widthNumeric, "widthNumeric");
            this.widthNumeric.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.widthNumeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.widthNumeric.Name = "widthNumeric";
            this.widthNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.widthNumeric.ValueChanged += new System.EventHandler(this.widthNumeric_ValueChanged);
            // 
            // heightNumeric
            // 
            resources.ApplyResources(this.heightNumeric, "heightNumeric");
            this.heightNumeric.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.heightNumeric.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.heightNumeric.Name = "heightNumeric";
            this.heightNumeric.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // heightTable
            // 
            resources.ApplyResources(this.heightTable, "heightTable");
            this.heightTable.Controls.Add(this.heightRadioButton7x5, 0, 0);
            this.heightTable.Controls.Add(this.heightRadioButton8x5, 1, 0);
            this.heightTable.Controls.Add(this.heightRadioButton8x8, 0, 1);
            this.heightTable.Controls.Add(this.heightRadioButton14x8, 1, 1);
            this.heightTable.Controls.Add(this.customHeightRadioButton, 0, 2);
            this.heightTable.Name = "heightTable";
            // 
            // heightRadioButton7x5
            // 
            resources.ApplyResources(this.heightRadioButton7x5, "heightRadioButton7x5");
            this.heightRadioButton7x5.Name = "heightRadioButton7x5";
            this.heightRadioButton7x5.TabStop = true;
            this.heightRadioButton7x5.UseVisualStyleBackColor = true;
            // 
            // heightRadioButton8x5
            // 
            resources.ApplyResources(this.heightRadioButton8x5, "heightRadioButton8x5");
            this.heightRadioButton8x5.Name = "heightRadioButton8x5";
            this.heightRadioButton8x5.UseVisualStyleBackColor = true;
            // 
            // heightRadioButton8x8
            // 
            resources.ApplyResources(this.heightRadioButton8x8, "heightRadioButton8x8");
            this.heightRadioButton8x8.Name = "heightRadioButton8x8";
            this.heightRadioButton8x8.UseVisualStyleBackColor = true;
            // 
            // heightRadioButton14x8
            // 
            resources.ApplyResources(this.heightRadioButton14x8, "heightRadioButton14x8");
            this.heightRadioButton14x8.Name = "heightRadioButton14x8";
            this.heightRadioButton14x8.UseVisualStyleBackColor = true;
            // 
            // customHeightRadioButton
            // 
            resources.ApplyResources(this.customHeightRadioButton, "customHeightRadioButton");
            this.customHeightRadioButton.Checked = true;
            this.customHeightRadioButton.Name = "customHeightRadioButton";
            this.customHeightRadioButton.TabStop = true;
            this.customHeightRadioButton.UseVisualStyleBackColor = true;
            // 
            // countTable
            // 
            resources.ApplyResources(this.countTable, "countTable");
            this.countTable.Controls.Add(this.countLabel, 0, 0);
            this.countTable.Controls.Add(this.countNumeric, 1, 0);
            this.countTable.Name = "countTable";
            // 
            // countLabel
            // 
            resources.ApplyResources(this.countLabel, "countLabel");
            this.countLabel.Name = "countLabel";
            // 
            // countNumeric
            // 
            resources.ApplyResources(this.countNumeric, "countNumeric");
            this.countNumeric.Maximum = new decimal(new int[] {
            1114111,
            0,
            0,
            0});
            this.countNumeric.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.countNumeric.Name = "countNumeric";
            this.countNumeric.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // resultFlow
            // 
            this.resultFlow.Controls.Add(this.okButton);
            this.resultFlow.Controls.Add(this.cancelButton);
            resources.ApplyResources(this.resultFlow, "resultFlow");
            this.resultFlow.Name = "resultFlow";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // ImportForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.resultFlow);
            this.Controls.Add(this.countTable);
            this.Controls.Add(this.patternHeightGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.FormLoad);
            this.patternHeightGroup.ResumeLayout(false);
            this.patternHeightGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumeric)).EndInit();
            this.heightTable.ResumeLayout(false);
            this.heightTable.PerformLayout();
            this.countTable.ResumeLayout(false);
            this.countTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.countNumeric)).EndInit();
            this.resultFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox patternHeightGroup;
        private System.Windows.Forms.TableLayoutPanel heightTable;
        private System.Windows.Forms.RadioButton heightRadioButton7x5;
        private System.Windows.Forms.RadioButton heightRadioButton8x5;
        private System.Windows.Forms.RadioButton heightRadioButton8x8;
        private System.Windows.Forms.RadioButton heightRadioButton14x8;
        private System.Windows.Forms.TableLayoutPanel countTable;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.NumericUpDown countNumeric;
        private System.Windows.Forms.FlowLayoutPanel resultFlow;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown widthNumeric;
        private System.Windows.Forms.NumericUpDown heightNumeric;
        private System.Windows.Forms.RadioButton customHeightRadioButton;
    }
}