/* DoubleBufferedTableLayoutPanel.cs
 * Source: http://stackoverflow.com/a/6552742/556048
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FntEditor
{
    public class DoubleBufferedTableLayoutPanel: TableLayoutPanel
    {
        public DoubleBufferedTableLayoutPanel()
            : base()
        {
            DoubleBuffered = true;
        }
    }
}
