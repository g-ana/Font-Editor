/* ImportForm.cs
 * Ана Ѓорѓевиќ, 2018
 * inspired by Juhász, Ádám L.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FntEditor
{
    public partial class ImportForm : Form
    {
        public ImportForm()
        {
            InitializeComponent();
         }

        private void FormLoad(object sender, EventArgs e)
        {
            heightRadioButton8x5.Checked = true;
        }

        public int PatternScanLines
        {
            get
            {
                if (customHeightRadioButton.Checked)
                    return (int)heightNumeric.Value;
                else if (heightRadioButton7x5.Checked)
                    return 7;
                else if (heightRadioButton8x5.Checked
                    || heightRadioButton8x8.Checked)
                    return 8;
                else if (heightRadioButton14x8.Checked)
                    return 14;
                else
                    return 0; // never should happen
            }
        }

        public int PatternNumber { get { return (int)countNumeric.Value; } }

        public int PatternWidth {
            get
            {
                if (customHeightRadioButton.Checked)
                    return (int)widthNumeric.Value;
                else if (heightRadioButton7x5.Checked
                    || heightRadioButton8x5.Checked)
                    return 5;
                else if (heightRadioButton8x8.Checked
                    || heightRadioButton14x8.Checked)
                    return 8;
                else
                    return 0; // never should happen
            }
        }

        private void HeightNumericChanged(object sender, EventArgs e)
        {
            customHeightRadioButton.Checked = true;
        }

        private void widthNumeric_ValueChanged(object sender, EventArgs e)
        {
            customHeightRadioButton.Checked = true;
        }
    }
}
