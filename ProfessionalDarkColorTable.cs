using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FntEditor
{
    class ProfessionalDarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(204, 102, 0); }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(204, 102, 0); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(204, 102, 0); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(102, 51, 0); }
        }

        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(102, 51, 0); }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(102, 51, 0); }
        }

        public override Color MenuItemBorder
        {
            get { return Color.FromArgb(102, 51, 0); }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(80, 80, 80); }
        }

        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(64, 64, 64); }
        }

        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(48, 48, 48); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(48, 48, 48); }
        }

        public override Color MenuStripGradientBegin
        {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color MenuStripGradientEnd
        {
            get { return Color.FromArgb(48, 48, 48); }
        }

        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color SeparatorDark
        {
            get { return Color.White; }
        }

        public override Color SeparatorLight
        {
            get { return Color.White; }
        }

        public override Color StatusStripGradientBegin
        {
            get { return Color.FromArgb(48, 48, 48); }
        }

        public override Color StatusStripGradientEnd
        {
            get { return Color.FromArgb(48, 48, 48); }
        }

        public override Color GripDark
        {
            get { return Color.FromArgb(32, 32, 32); }
        }

        public override Color GripLight
        {
            get { return Color.FromArgb(64, 64, 64); }
        }
    }
}
