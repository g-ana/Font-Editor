/* PatternEditor.cs
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
using FntCore;

namespace FntEditor
{
    public partial class PatternEditor : Form
    {
        CharacterPattern table;
        int patternIndex;
        //byte[] pattern;
        Point cursor;
        int mouseColor = -1;
        bool cVisible = false;
        int extraHeight;
        int patternWidth = 5;

        internal TableForm EditorOwner { get; set; }

        public PatternEditor()
        {
            InitializeComponent();

            patternPicture.BackColor = BackColor = Color.FromArgb(250, 250, 250);
            extraHeight = Height - patternPicture.Height;
        }

        public PatternEditor(CharacterPattern table, int pattern): this()
        {
            this.table = table;
            this.patternIndex = pattern;
            patternWidth = table.width;
            //this.pattern = table[pattern];
            Text += String.Format(" \u2013 0x{0:X}", pattern);

            Height = table.ScanLines * 32 + extraHeight;
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (cursor.Y > 0)
                            --cursor.Y;
                        else
                            cursor.Y = table.ScanLines - 1;
                        e.Handled = true;
                        break;
                    case Keys.Left:
                        if (cursor.X > 0)
                            --cursor.X;
                        else
                            cursor.X = (patternWidth-1);
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        if (cursor.Y < table.ScanLines - 1)
                            ++cursor.Y;
                        else
                            cursor.Y = 0;
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        if (cursor.X < (patternWidth-1))
                            ++cursor.X;
                        else
                            cursor.X = 0;
                        e.Handled = true;
                        break;
                    case Keys.Space:
                        table[patternIndex, cursor.Y] = table[patternIndex, cursor.Y].SetBit(
                            table[patternIndex, cursor.Y].GetBit((patternWidth-1) - cursor.X) ^ 1, (patternWidth-1) - cursor.X);
                        table[patternIndex, cursor.Y] = table[patternIndex, cursor.Y];
                        e.Handled = true;
                        break;
                    case Keys.Escape:
                        Close();
                        break;
                    default:
                        break;
                }

                if (e.Handled)
                    cVisible = true;
            }
            else if (e.Modifiers == Keys.Shift)
            {
                //byte carry;
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        EditorOwner.ShiftUp(patternIndex);
                        Refresh();
                        e.Handled = true;
                        break;
                    case Keys.Down:
                        EditorOwner.ShiftDown(patternIndex);
                        Refresh();
                        e.Handled = true;
                        break;
                    case Keys.Left:
                        EditorOwner.ShitLeft(patternIndex);
                        Refresh();
                        e.Handled = true;
                        break;
                    case Keys.Right:
                        EditorOwner.ShiftRight(patternIndex);
                        Refresh();
                        e.Handled = true;
                        break;
                    default:
                        break;
                }
            }

            if (e.Handled)
                Refresh();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            int px = e.X * patternWidth / (sender as PictureBox).Width;
            int py = e.Y * table.ScanLines / (sender as PictureBox).Height;
            System.Diagnostics.Debug.WriteLineIf(System.Diagnostics.Debugger.IsAttached,
                String.Format("X: {0}, Y: {1} ({2}, {3})", px, py, e.X, e.Y), "Editor Mouse Event");
            mouseColor = 1 - table[patternIndex, py].GetBit((patternWidth-1) - px);
            table[patternIndex, py] = table[patternIndex, py].SetBit(mouseColor, (patternWidth-1) - px);
            cursor.X = px;
            cursor.Y = py;
            Invalidate(true);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (mouseColor >= 0)
            {
                mouseColor = -1;
                //table[patternIndex] = pattern;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            cVisible = false;
            if (mouseColor >= 0)
            {
                int px = e.X * patternWidth / (sender as PictureBox).Width;
                int py = e.Y * table.ScanLines / (sender as PictureBox).Height;
                if (px < 0 || px >= patternWidth || py < 0 || py >= table.ScanLines) return;
                table[patternIndex, py] = table[patternIndex, py].SetBit(mouseColor, (patternWidth-1) - px);
                cursor.X = px;
                cursor.Y = py;
                table[patternIndex, py] = table[patternIndex, py];
                Invalidate(true);
            }
        }

        private void PaintPattern(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Brush brush;
            for (int row = 0; row < table.ScanLines; ++row)
                for (int col = 0; col < patternWidth; ++col)
                {
                    if (cVisible && cursor.X == col && cursor.Y == row)
                        brush = table[patternIndex, row].GetBit((patternWidth-1) - col) == 0 ?
                            new SolidBrush(Color.FromArgb(
                                255 - SystemColors.Highlight.R,
                                255 - SystemColors.Highlight.G,
                                255 - SystemColors.Highlight.B)) :
                            SystemBrushes.Highlight;
                    else
                        brush = table[patternIndex, row].GetBit((patternWidth-1) - col) == 0 ?
                            new SolidBrush(BackColor) : new SolidBrush(ForeColor);
                    canvas.FillRectangle(brush,
                        col * (sender as PictureBox).Width / (float)patternWidth - 0.5f,
                        row * (sender as PictureBox).Height / (float)table.ScanLines - 0.5f,
                        (sender as PictureBox).Width / (float)patternWidth, (sender as PictureBox).Height / (float)table.ScanLines);
                }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            Invalidate(true);
        }

        private void RequestHelp(object sender, HelpEventArgs hlpevent)
        {
            //            Help.ShowHelp(this, Program.HelpFile, HelpNavigator.Topic,
            //                Program.TopicId("EditPattern"));
            MessageBox.Show("ToDo ... Help...");
        }

        private void OnCloseMenu(object sender, EventArgs e)
        {
            Close();
        }

        private void OnExit(object sender, EventArgs e)
        {
            EditorOwner.Close();
        }

        private void OnNew(object sender, EventArgs e)
        {
            EditorOwner.NewPattern(sender, e);
        }

        private void OnOpen(object sender, EventArgs e)
        {
            EditorOwner.Open();
        }

        private void OnSave(object sender, EventArgs e)
        {
            EditorOwner.Save(false);
        }

        private void OnSaveAs(object sender, EventArgs e)
        {
            EditorOwner.Save(true);
        }

        private void OnImport(object sender, EventArgs e)
        {
            EditorOwner.ImportFnt(sender, e);
        }

        private void OnExport(object sender, EventArgs e)
        {
            EditorOwner.ExportFnt(sender, e);
        }

        private void OnCut(Object sender, EventArgs e)
        {
            EditorOwner.Copy(patternIndex);
            EditorOwner.Clear(patternIndex);
            Refresh();
        }

        private void OnCopy(object sender, EventArgs e)
        {
            EditorOwner.Copy(patternIndex);
        }

        private void OnPaste(object sender, EventArgs e)
        {
            EditorOwner.Paste(patternIndex);
            Refresh();
        }

        private void OnClear(object sender, EventArgs e)
        {
            EditorOwner.Clear(patternIndex);
            Refresh();
        }

        private void OnVFlip(object sender, EventArgs e)
        {
            EditorOwner.VFlip(patternIndex);
            Refresh();
        }

        private void OnHFlip(object sender, EventArgs e)
        {
            EditorOwner.HFlip(patternIndex);
            Refresh();
        }

        private void OnHelp(object sender, EventArgs e)
        {
            //            Help.ShowHelp(this, Program.HelpFile, HelpNavigator.Topic,Program.TopicId("EditPattern"));
            MessageBox.Show("To do ...");
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        private void OnInvertClick(object sender, EventArgs e)
        {
            EditorOwner.Invert(patternIndex);
            Refresh();
        }

        private void OnShiftLeftClick(object sender, EventArgs e)
        {
            EditorOwner.ShitLeft(patternIndex);
            Refresh();
        }

        private void OnShiftRightClick(object sender, EventArgs e)
        {
            EditorOwner.ShiftRight(patternIndex);
            Refresh();
        }

        private void onShiftUpClick(object sender, EventArgs e)
        {
            EditorOwner.ShiftUp(patternIndex);
            Refresh();
        }

        private void onShiftDownClick(object sender, EventArgs e)
        {
            EditorOwner.ShiftDown(patternIndex);
            Refresh();
        }
    }
}
