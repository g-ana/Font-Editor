/* ExportForm.cs
 * Ана Ѓорѓевиќ, 2018
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// custom library in dll
using FntCore;
using System.IO;

namespace FntEditor
{
    public partial class ExportForm : Form
    {
        CharacterPattern table;
        public ExportForm(CharacterPattern tab, String filename)
        {
            InitializeComponent();
            table = tab;
            textBoxFontName.Text = String.Format("{0} ch {1} x {2}", table.PatternCount, table.width, table.ScanLines);
            textBoxHeaderFilename.Text = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + ".h";
            labelBrackets.Text = String.Format("[][{0}]", table.width);
            comboBoxDisplayType.SelectedIndex = 1;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
/*
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        */
        private void buttonSetPath_Click(object sender, EventArgs e)
        {
            DialogResult result;
            saveFileDialogExport.Title = "C header file";
            saveFileDialogExport.Filter = "C header file|*.h";
            saveFileDialogExport.OverwritePrompt = false;
            saveFileDialogExport.FileName = Path.GetFileName(textBoxHeaderFilename.Text);
            result = saveFileDialogExport.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textBoxHeaderFilename.Text = Path.GetFullPath(saveFileDialogExport.FileName);
                Update();
            }

        }

        private void buttonDoExport_Click(object sender, EventArgs e)
        {
            DialogResult result;
            if (File.Exists(textBoxHeaderFilename.Text))
            {
                result = MessageBox.Show(this, String.Format(Properties.Resources.OverwriteExistingFile, textBoxHeaderFilename.Text),
                    Properties.Resources.OverwriteFileTitle, 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, 
                    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                    return;
            }
            // save
            CharacterPattern.exportHformat exFmtType = 0;
            if (radioButtonBIN.Checked)
                exFmtType = CharacterPattern.exportHformat.BIN;
            if (radioButtonDEC.Checked)
                exFmtType = CharacterPattern.exportHformat.DEC;
            if (radioButtonHEX.Checked)
                exFmtType = CharacterPattern.exportHformat.HEX;

            switch(comboBoxDisplayType.SelectedIndex)
            {
                case -1: MessageBox.Show("select export display"); break;
                case 0:
                    table.SaveLCD2File(textBoxHeaderFilename.Text,
                        exFmtType,
                        checkBox_const.Checked,
                        checkBox_static.Checked,
                        checkBox_PROGMEM.Checked,
                        textBoxVarName.Text);
                    break;
                case 1: 
                    table.SaveNokia5110File(textBoxHeaderFilename.Text,
                        exFmtType,
                        checkBox_const.Checked,
                        checkBox_static.Checked,
                        checkBox_PROGMEM.Checked,
                        textBoxVarName.Text);
                    //MessageBox.Show("Succesfully exported");
                    break;
                default: MessageBox.Show("Unknown export type"); break;
            }
            this.Close();
        }
    }
}
