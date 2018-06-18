/* TableForm.cs
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Runtime.Serialization;
using FntCore;
using System.Diagnostics;

namespace FntEditor
{
    public partial class TableForm : Form
    {
        CharacterPattern table;
        Dictionary<int, PatternEditor> editors = new Dictionary<int, PatternEditor>();
        bool modified;
        private TableLayoutPanel patternTable;
        string fileName = Properties.Resources.Untitled;
        Point focus = Point.Empty;

        readonly Color inactiveBorder = Color.FromArgb(192, 192, 192);
        readonly Color activeBorder = Color.White;

        private AutoResetEvent are = new AutoResetEvent(true);

        int patternWidth = 8;

        public string StatusMessage
        {
            get { return statusLabel.Text; }
            set { statusLabel.Text = value; }
        }

        public bool EnableMenuItems
        {
            set
            {
                saveAsToolStripMenuItem.Enabled = value;
                saveToolStripMenuItem.Enabled = value;
                exportFontPatternToolStripMenuItem.Enabled = value;
            }
        }

        public TableForm()
        {
            table = new CharacterPattern();
            table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
            InitializeComponent();
            Modified = false;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public TableForm(int scanLines, int number)
            : this()
        {
            table = new CharacterPattern(number, scanLines);
            table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
        }

        public TableForm(string file) : this(file, 0, 0) { }

        public TableForm(string file, int scanLines, int number)
            : this()
        {
            if (retableBackgroundWorker.IsBusy)
            {
                retableBackgroundWorker.CancelAsync();
                are.WaitOne();
            }
            if (Path.GetExtension(file).Equals(".fnp"))
            {
                try
                {
                    using (Stream fileStream = new FileStream(file, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                        table = formatter.Deserialize(fileStream) as CharacterPattern;
                        table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
                        fileName = file;
                        Modified = false;
                    }
                    if (table == null)
                    {
                        MessageBox.Show(this, Properties.Resources.SerializationErrorMsg,
                            Properties.Resources.IOErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        table = new CharacterPattern();
                        table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
                        fileName = Properties.Resources.Untitled;
                        Modified = false;
                    }
                }
                catch (SystemException e)
                {
                    // For Sysinternal DebugView support
                    Debug.WriteLine(e, "FntEditor");
                    MessageBox.Show(this,
                        String.Format(Properties.Resources.IOErrorMsg, file), //e.Message) ,
                        Properties.Resources.IOErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (scanLines == 0 || number == 0)
                {
                    ImportForm iform = new ImportForm();
                    DialogResult result = iform.ShowDialog();
                    try
                    {
                        if (result == System.Windows.Forms.DialogResult.OK)
                            table = new CharacterPattern(iform.PatternNumber, iform.PatternScanLines, file);
                        table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
                        fileName = Path.GetFileNameWithoutExtension(file);
                        Modified = true;
                    }
                    catch (IOException e)
                    {
                        // Warn the user of EOF, MissingNO or other.
                        Debug.WriteLine(e, "FntEditor");
                        MessageBox.Show(this,
                            String.Format(Properties.Resources.IOErrorMsg, file), //e.Message),
                            Properties.Resources.IOErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        table = new CharacterPattern(number, scanLines, file);
                        table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
                        fileName = Path.GetFileNameWithoutExtension(file);
                        Modified = true;
                    }
                    catch (IOException e)
                    {
                        // Warn the user of EOF, MissingNO or other.
                        Debug.WriteLine(e, "FntEditor");
                        MessageBox.Show(this,
                            String.Format(Properties.Resources.IOErrorMsg, file), //e.Message),
                            Properties.Resources.IOErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // TODO Handle past hazard, when loaded.

        void table_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Modified = true;
            if (e is IndexedPropertyChangedEventArg)
            {
                IndexedPropertyChangedEventArg e2 = e as IndexedPropertyChangedEventArg;
                patternTable.Controls[String.Format("pattern{0}", e2.ChangedIndexes[0])].Invalidate();
            }
            else Refresh();
        }

        private void ReinitializeTable()
        {
            if (retableBackgroundWorker.IsBusy)
                are.WaitOne();
            SuspendLayout();
            UseWaitCursor = true;
            tableContainer.Controls.Remove(patternTable);
            EnableMenuItems = false;

            are.Reset();
            progressBar.Value = 0;
            progressBar.Visible = true;
            //progressBar.Maximum = table.PatternCount;
            retableBackgroundWorker.RunWorkerAsync(new DoubleBufferedTableLayoutPanel());
            StatusMessage = Properties.Resources.RebuildingUI;
        }

        private bool Modified
        {
            get { return modified; }
            set
            {
                Text = Path.GetFileNameWithoutExtension(fileName) +
                    (value ? "*" : "") + " \u2013 " + (Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
                modified = value;
            }
        }

        void EditPattern(object sender, EventArgs e)
        {
            if (sender is PictureBox && (sender as PictureBox).Name.StartsWith("pattern"))
            {
                int pattern = Convert.ToInt32((sender as PictureBox).Name.Substring((patternWidth-1)));
                if (!editors.ContainsKey(pattern))
                    editors.Add(pattern, new PatternEditor(table, pattern));
                else if (editors[pattern] == null || editors[pattern].IsDisposed)
                    editors[pattern] = new PatternEditor(table, pattern);
                editors[pattern].EditorOwner = this;
                editors[pattern].Show();
            }
        }

        void OnPaintPattern(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            int pattern = (int)((PictureBox)sender).Tag;
            PaintPattern(graphics, BackColor, ForeColor, pattern, 1);
            if (!(sender is ToolStripMenuItem) && focus.X == pattern % 16 && focus.Y == pattern / 16)
                graphics.DrawRectangle(new Pen(activeBorder), 0, 0, 10, table.ScanLines + 1);
            else if (!(sender is ToolStripMenuItem))
                graphics.DrawRectangle(new Pen(inactiveBorder), 0, 0, 10, table.ScanLines + 1);
        }

        private void PaintPattern(Graphics graphics, Color backColor, Color foreColor, int pattern, int offset)
        {
            graphics.FillRegion(new SolidBrush(backColor), new Region(new Rectangle(0, 0, patternWidth, table.ScanLines)));
            for (int row = 0; row < table.ScanLines; ++row)
            {
                for (int col = 0; col < patternWidth; ++col)
                {
                    if ((table[pattern, row] & (1 << (patternWidth-1) - col)) != 0)
                        graphics.FillRectangle(new SolidBrush(foreColor),
                            col + offset, row + offset, col == (patternWidth-1) && ((pattern & 0xE0) == 0xC0 || (pattern & 0xF0) == 0xB0 ||
                            pattern > 0xFF) ? 2 : 1, 1);
                }
            }
        }

        internal bool Save(bool saveDifferent)
        {
            if (saveDifferent || !File.Exists(fileName))
            {
                DialogResult result;
                saveFileDialog.Title = Properties.Resources.SaveTitle;
                saveFileDialog.Filter = Properties.Resources.FontProjectFilter;
                saveFileDialog.FileName = fileName;
                result = saveFileDialog.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.Cancel)
                    return true;
                fileName = saveFileDialog.FileName;
            }
            using (Stream file = new FileStream(fileName, FileMode.OpenOrCreate))
                new BinaryFormatter().Serialize(file, table);
            Modified = false;
            //MessageBox.Show(String.Format("{0} x {1}", table.width, table.ScanLines));

            return false;
        }

        private bool ModifiedWarnCancel()
        {
            DialogResult result;
            if (Modified)
            {
                result = MessageBox.Show(this, Properties.Resources.ModifiedMsg, Properties.Resources.ModifiedTitle,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        return Save(false);
                    case System.Windows.Forms.DialogResult.No:
                        return false;
                    default:
                        return true;
                }
            }
            return false;
        }

        internal void ImportFnt(object sender, EventArgs e)
        {
            if (retableBackgroundWorker.IsBusy)
            {
                retableBackgroundWorker.CancelAsync();
                are.WaitOne();
            }
            else if (ModifiedWarnCancel()) return;
            DialogResult result;
            openFileDialog.Title = Properties.Resources.ImportTitle;
            openFileDialog.Filter = Properties.Resources.FontFilter;
            openFileDialog.FileName = "";
            result = openFileDialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;
            ImportForm iform = new ImportForm();
            result = iform.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;
            try
            {
                table = new CharacterPattern(iform.PatternNumber, iform.PatternScanLines, openFileDialog.FileName);
                fileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
                Modified = true;
                ReinitializeTable();
            }
            catch (IOException ex)
            {
                // Warn the user of EOF, MissingNO or other.
                Debug.WriteLine(ex, "FntEditor");
                MessageBox.Show(this,
                    String.Format(Properties.Resources.IOErrorMsg, ex.Message),
                    Properties.Resources.IOErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void ExportFnt(object sender, EventArgs e)
        {
            if (ModifiedWarnCancel()) return;
            DialogResult result;
            saveFileDialog.Title = Properties.Resources.ExportTitle;
            saveFileDialog.Filter = Properties.Resources.FontFilter;
            saveFileDialog.FileName = Path.GetFileNameWithoutExtension(fileName);
            result = saveFileDialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;
            table.SaveToFile(saveFileDialog.FileName);
        }

        internal void NewPattern(object sender, EventArgs e)
        {
            if (retableBackgroundWorker.IsBusy)
            {
                retableBackgroundWorker.CancelAsync();
                are.WaitOne();
            }
            else if (ModifiedWarnCancel()) return;
            DialogResult result;
            ImportForm iform = new ImportForm();
            result = iform.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;
            table = new CharacterPattern(iform.PatternNumber, iform.PatternScanLines, iform.PatternWidth);
            //table.width = iform.PatternWidth;
            table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
            fileName = Properties.Resources.Untitled;
            Modified = false;
            ReinitializeTable();
            //MessageBox.Show(String.Format("{0} x {1}", table.width, table.ScanLines));

        }

        private void OnOpen(object sender, EventArgs e)
        {
            Open();
        }

        internal void Open()
        {
            if (retableBackgroundWorker.IsBusy)
            {
                retableBackgroundWorker.CancelAsync();
                are.WaitOne();
            }
            else if (ModifiedWarnCancel()) return;
            DialogResult result;
            openFileDialog.Title = Properties.Resources.OpenTitle;
            openFileDialog.Filter = Properties.Resources.FontProjectFilter;
            openFileDialog.FileName = "";
            result = openFileDialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.Cancel)
                return;
            foreach (PatternEditor e in editors.Values)
                e.Close();
            editors.Clear();
            try
            {
                using (Stream file = openFileDialog.OpenFile())
                    table = (CharacterPattern)new BinaryFormatter().Deserialize(file);
                //MessageBox.Show(String.Format("{0} x {1}", table.width, table.ScanLines));
            }
            catch (SystemException e)
            {
                Debug.WriteLine(e, "FntEditor");
                MessageBox.Show(this,
                    String.Format(Properties.Resources.IOErrorMsg, openFileDialog.FileName), //e.Message),
                    Properties.Resources.IOErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            table.PropertyChanged += new PropertyChangedEventHandler(table_PropertyChanged);
            fileName = openFileDialog.FileName;
            Modified = false;
            ReinitializeTable();
        }

        private void OnSave(object sender, EventArgs e)
        {
            Save(false);
        }

        private void OnSaveAs(object sender, EventArgs e)
        {
            Save(true);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Close();
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.ApplicationExitCall:
                case CloseReason.UserClosing:
                case CloseReason.None: // Not sure...
                    if (ModifiedWarnCancel())
                        e.Cancel = true;
                    if (retableBackgroundWorker.IsBusy)
                    {
                            retableBackgroundWorker.CancelAsync();
                            are.WaitOne();
                    }
                    break;
                case CloseReason.FormOwnerClosing:
                case CloseReason.MdiFormClosing:
                    // This is invalid for this form.
                    if (retableBackgroundWorker.IsBusy)
                    {
                        retableBackgroundWorker.CancelAsync();
                        are.WaitOne();
                    }
                    break;
                case CloseReason.TaskManagerClosing:
                case CloseReason.WindowsShutDown:
                    // TODO Handle hazards.
                    if (retableBackgroundWorker.IsBusy)
                    {
                        retableBackgroundWorker.CancelAsync();
                        are.WaitOne();
                    }
                    break;
                default:
                    break;
            }
        }

        private void backgroundRetable(object sender, DoWorkEventArgs e)
        {
            TableLayoutPanel patternTable = (e.Argument as TableLayoutPanel);
            patternTable.SuspendLayout();

            //
            // patternTable
            //
            //patternTable.Dock = DockStyle.Fill;
            patternTable.ColumnCount = 17;
            //patternTable.RowCount = 2;
            patternTable.AutoSize = true;
            patternTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            patternTable.RowCount = (table.PatternCount >> 4) + 2;

            focus = Point.Empty;

            // H-Header
            {
                Label headLabel;
                PictureBox patternImage;
                for (int i = 0; i < 16; ++i)
                {
                    if (patternTable.Controls.ContainsKey(String.Format("hHeadLabel{0}", i)))
                        continue;
                    headLabel = new Label();
                    headLabel.Text = String.Format("{0:X}", i);
                    headLabel.Name = String.Format("hHeadLabel{0}", i);
                    headLabel.TextAlign = ContentAlignment.MiddleCenter;
                    headLabel.AutoSize = true;
                    headLabel.Dock = DockStyle.Fill;
                    //headLabel.ForeColor = Color.White;
                    headLabel.ForeColor = Color.Black;
                    patternTable.Controls.Add(headLabel, i + 1, 0);
                    if (retableBackgroundWorker.CancellationPending)
                    {
                        e.Result = patternTable;
                        e.Cancel = true;
                        are.Set();
                        return;
                    }
                }
                int pastProgress = 0;
                for (int pattern = 0; pattern < table.PatternCount; ++pattern)
                {
                    if (pattern % 16 == 0)
                    {
                        if (!patternTable.Controls.ContainsKey(String.Format("vHeadLabel{0}", pattern >> 4)))
                        {
                            headLabel = new Label();
                            headLabel.Text = String.Format("0x{0:X}#", pattern >> 4);
                            headLabel.Name = String.Format("vHeadLabel{0}", pattern >> 4);
                            headLabel.TextAlign = ContentAlignment.MiddleCenter;
                            headLabel.AutoSize = true;
                            headLabel.Dock = DockStyle.Fill;
                            //                            headLabel.ForeColor = Color.White;
                            headLabel.ForeColor = Color.Black;
                            patternTable.Controls.Add(headLabel, 0, (int)(pattern / 16) + 1);
                        }
                    }
                    string name = String.Format("pattern{0}", pattern);
                    if (patternTable.Controls.ContainsKey(name))
                        patternTable.Controls[name].Height = table.ScanLines + 2;
                    else
                    {
                        patternImage = new PictureBox();
                        patternImage.Name = name;
                        patternImage.Tag = pattern;
                        patternImage.Width = 11;
                        patternImage.Height = table.ScanLines + 2;
                        patternImage.Paint += new PaintEventHandler(OnPaintPattern);
                        patternImage.Click += new EventHandler(SelectPattern);
                        patternImage.DoubleClick += new EventHandler(EditPattern);
                        patternTable.Controls.Add(patternImage, pattern % 16 + 1, (int)(pattern / 16) + 1);
                    }
                    if (retableBackgroundWorker.CancellationPending)
                    {
                        e.Result = patternTable;
                        e.Cancel = true;
                        are.Set();
                        return;
                    }
                    if (pastProgress != pattern * 10000 / table.PatternCount)
                        retableBackgroundWorker.ReportProgress(pattern * 10000 / table.PatternCount);
                    pastProgress = pattern * 10000 / table.PatternCount;
                }
            }

            patternTable.ResumeLayout();
            e.Result = patternTable;
            are.Set();
        }

        void SelectPattern(object sender, EventArgs e)
        {
            PictureBox pSender = sender as PictureBox;
            if (pSender == null) return;
            if (!(pSender.Tag is int)) return;
            int pattern = (int)pSender.Tag;
            int pastPattern = focus.X + focus.Y * 16;
            focus.X = pattern % 16;
            focus.Y = pattern / 16;
            patternTable.Controls[String.Format("pattern{0}", pastPattern)].Refresh();
            pSender.Refresh();
        }

        private void progressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBar.IsDisposed)
                return;
            progressBar.Value = e.ProgressPercentage / 10;
            progressBar.ToolTipText = String.Format("{0:N2}%", (float)e.ProgressPercentage / 100f);
            StatusMessage = String.Format(Properties.Resources.RebuildingUI + " {0:N2}% ",
                (float)e.ProgressPercentage / 100f);
        }

        private void retableCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }
            patternTable = e.Result as DoubleBufferedTableLayoutPanel;
            tableContainer.Controls.Add(patternTable);
            UseWaitCursor = false;
            EnableMenuItems = true;
            ResumeLayout();
            Invalidate(true);

            progressBar.Visible = false;
            StatusMessage = Properties.Resources.DoneMsg;
            are.Reset();
        }

        private void OpenHelp(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Program.HelpFile);
            MessageBox.Show("ToDo ... Help...");
        }

        private void RequestHelp(object sender, HelpEventArgs hlpevent)
        {
            //            Help.ShowHelp(this, Program.HelpFile, HelpNavigator.Topic,
            //                Program.TopicId("UserInterface"));
            MessageBox.Show("ToDo ... Help...");

        }

        private void ShowAbout(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        private void FormLoading(object sender, EventArgs e)
        {
            ReinitializeTable();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (retableBackgroundWorker.IsBusy) return;
            if (!e.Alt && !e.Control && !e.Shift)
            {
                Control active, inactive;
                switch (e.KeyData)
                {
                    case Keys.Up:
                        inactive = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        if (focus.Y > 0)
                            --focus.Y;
                        else
                            focus.Y = table.PatternCount / 16 - 1;
                        e.Handled = true;
                        active = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        tableContainer.ScrollControlIntoView(active);
                        inactive.Refresh();
                        active.Refresh();
                        break;
                    case Keys.Down:
                        inactive = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        if (focus.Y < table.PatternCount / 16 - 1)
                            ++focus.Y;
                        else
                            focus.Y = 0;
                        e.Handled = true;
                        active = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        tableContainer.ScrollControlIntoView(active);
                        inactive.Refresh();
                        active.Refresh();
                        break;
                    case Keys.Left:
                        inactive = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        if (focus.X > 0)
                            --focus.X;
                        else
                            focus.X = 15;
                        e.Handled = true;
                        active = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        tableContainer.ScrollControlIntoView(active);
                        inactive.Refresh();
                        active.Refresh();
                        break;
                    case Keys.Right:
                        inactive = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        if (focus.X < 15)
                            ++focus.X;
                        else
                            focus.X = 0;
                        e.Handled = true;
                        active = patternTable.Controls[String.Format("pattern{0}", focus.X + focus.Y * 16)];
                        tableContainer.ScrollControlIntoView(active);
                        inactive.Refresh();
                        active.Refresh();
                        break;
                    case Keys.Enter:
                        EditPattern(patternTable.Controls[String.Format("pattern{0}", focus.Y * 16 + focus.X)],
                            EventArgs.Empty);
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        internal void Copy(int pattern)
        {
            Image img = new Bitmap(patternWidth, table.ScanLines);
            Graphics g = Graphics.FromImage(img);
            PaintPattern(g, Color.White, Color.Black, pattern, 0);
            Clipboard.SetImage(img);
        }

        internal void Clear(int pattern)
        {
            for (int i = 0; i < table.ScanLines; ++i)
                table[pattern, i] = 0;
        }

        internal void Paste(int pattern)
        {
            if (!Clipboard.ContainsImage()) return;
            Image img;
            img = Clipboard.GetImage();
            Graphics g = Graphics.FromImage(img);
            Bitmap bmp;
            bmp = new Bitmap(img, new Size(patternWidth, table.ScanLines));
            for (int y = 0; y < table.ScanLines; ++y)
            {
                table[pattern, y] = 0;
                for (int x = 0; x < patternWidth; ++x)
                {
                    Color c =bmp.GetPixel(x,y);
                    // source for weights: http://en.wikipedia.org/wiki/YUV
                    // 255^2 / 2 = 32512.5
                    table[pattern, y] |= (byte)((c.A * (.299 * c.R + .587 * c.G + .114 * c.B) < 32512.5 ? 1 : 0) << ((patternWidth-1) - x));
                }
            }
        }

        internal void Invert(int pattern)
        {
            for (int y = 0; y < table.ScanLines; ++y)
                table[pattern, y] ^= 0xFF;
        }

        internal void ShiftRight(int patternIndex)
        {
            for (int i = 0; i < table.ScanLines; ++i)
                table[patternIndex, i] = (byte)((table[patternIndex, i] >> 1) |
                    ((table[patternIndex, i] << (patternWidth-1)) & 0x80));
        }
        internal void ShitLeft(int patternIndex)
        {
            for (int i = 0; i < table.ScanLines; ++i)
                table[patternIndex, i] = (byte)((table[patternIndex, i] << 1) |
                    ((table[patternIndex, i] >> (patternWidth-1)) & 1));
        }

        internal void ShiftUp(int patternIndex)
        {
            byte carry = table[patternIndex, 0];
            for (int i = 0; i < table.ScanLines - 1; ++i)
                table[patternIndex, i] = table[patternIndex, i + 1];
            table[patternIndex, table.ScanLines - 1] = carry;
        }

        internal void ShiftDown(int patternIndex)
        {
            byte carry = table[patternIndex, table.ScanLines - 1];
            for (int i = table.ScanLines - 1; i > 0; --i)
                table[patternIndex, i] = table[patternIndex, i - 1];
            table[patternIndex, 0] = carry;
        }

        internal void HFlip(int patternIndex)
        {
            for (int y = 0; y < table.ScanLines; ++y)
            {
                Debugger.Log(1, "FntEdit-Debug", String.Format("{1:X}. Was: {0:X2} ({2,08}) ",
                    table[patternIndex, y], y, Convert.ToString(table[patternIndex, y], 2)));
                for (int x = 0; x < 4; ++x)
                {
                    int bit = table[patternIndex, y].GetBit((patternWidth-1) - x);
                    table[patternIndex, y] = table[patternIndex, y].SetBit(table[patternIndex, y].GetBit(x), (patternWidth-1) - x);
                    table[patternIndex, y] = table[patternIndex, y].SetBit(bit, x);
                }
                Debugger.Log(1, "FntEdit-Debug", String.Format("{1:X}. Now: {0:X2} ({2,08})\n",
                    table[patternIndex, y], y, Convert.ToString(table[patternIndex, y], 2)));
            }
            if (table.width < 8)
            {
//                MessageBox.Show("table.width < 8");
                for (int i = table.width; i < 8; i++)
                    ShiftRight(patternIndex);
            }
        }

        internal void VFlip(int pattern)
        {
            for (int y = 0; y < table.ScanLines / 2; ++y)
            {
                byte tmp = table[pattern, table.ScanLines - y - 1];
                table[pattern, table.ScanLines - y - 1] = table[pattern, y];
                table[pattern, y] = tmp;
            }
        }

        private void OnCopy(object sender, EventArgs e)
        {
            int idx = focus.X + focus.Y * 16;
            Copy(idx);
            if (sender == cutToolStripMenuItem)
                Clear(idx);
            if (editors.ContainsKey(idx))
                editors[idx].Refresh();
        }

        private void OnClear(object sender, EventArgs e)
        {
            int idx = focus.X + focus.Y * 16;
            Clear(idx);
            if (editors.ContainsKey(idx))
                editors[idx].Refresh();
        }

        private void OnPaste(object sender, EventArgs e)
        {
            int idx = focus.X + focus.Y * 16;
            Paste(idx);
            if (editors.ContainsKey(idx))
                editors[idx].Refresh();
        }

        private void OnInvertClick(object sender, EventArgs e)
        {
            int idx = focus.X + focus.Y * 16;
            Invert(idx);
            if (editors.ContainsKey(idx))
                editors[idx].Refresh();
        }

        private void OnHFlipClick(object sender, EventArgs e)
        {
            int idx = focus.X + focus.Y * 16;
            HFlip(idx);
            if (editors.ContainsKey(idx))
                editors[idx].Refresh();
        }

        private void OnVFlipClick(object sender, EventArgs e)
        {
            int idx = focus.X + focus.Y * 16;
            VFlip(idx);
            if (editors.ContainsKey(idx))
                editors[idx].Refresh();
        }

        private void onExportLCDClick(object sender, EventArgs e)
        {
            if (ModifiedWarnCancel()) return;

            DialogResult result;
            ExportForm exportDialog = new ExportForm(table, fileName);
            result = exportDialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
        }

        private void LanguageButton_Click(object sender, EventArgs e)
        {
            if (LanguageButton.Text == "EN")
            {
                LanguageButton.Text = "MK";
                Application.CurrentCulture = new System.Globalization.CultureInfo("mk-MK");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("mk-MK");
            }
            else
            {
                LanguageButton.Text = "EN";
                Application.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); 
            }
            //MessageBox.Show("Current culture set to:" + System.Globalization.CultureInfo.CurrentCulture);
            //Application.CurrentCulture.EnglishName;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (ModifiedWarnCancel()) return;

            DialogResult result;
            ExportForm exportDialog = new ExportForm(table, fileName);
            result = exportDialog.ShowDialog();
            if (result == DialogResult.Cancel)
                return;
        }
    }
}
