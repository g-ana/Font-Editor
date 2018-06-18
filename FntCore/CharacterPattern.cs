/* CharacterPattern.cs
 * Ана Ѓорѓевиќ, 2018
 * inspired by Juhász, Ádám L.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections;
using System.Windows.Forms;

namespace FntCore
{
    [Serializable]
    public class CharacterPattern: ISerializable, INotifyPropertyChanged
    {
        public enum exportHformat { BIN, DEC, HEX };
        int height;
        int columns;
        List<List<byte>> table;
        public int width { get { return columns; } }

        [NonSerialized]
        private const string PATTERN_COUNT_ID = "pno";
        private const string SCAN_LINES_ID = "sl";
        private const string WIDTH_ID = "w";
        private const string TABLE_ID = "table";

        public CharacterPattern() : this(256, 16) { }

        public CharacterPattern(int scanLines) : this(256, scanLines) { }

        public CharacterPattern(int characterCount, int scanLines, int chracterPatternWidth = 8)
        {
            // Check for valid heights (and either warn, or error)
            // These are valid: 8, 9, 13, 14, 16
            this.height = scanLines;
            this.columns = chracterPatternWidth;
            table = new List<List<byte>>(characterCount);
            for (int i = 0; i < characterCount; ++i)
            {
                table.Add(new List<byte>(scanLines));
                for (int j = 0; j < scanLines; ++j)
#if DEBUG
                {
                    if (j % 2 == 0)
                        table[i].Add(85);
                    else
                        table[i].Add(170);
                }
#else
                    table[i].Add(0);
#endif
            }
        }

        public CharacterPattern(int characterCount, int scanLines, string fntFile): this(characterCount, scanLines)
        {
            if (new FileInfo(fntFile).Length < scanLines * characterCount)
                throw new EndOfStreamException(String.Format(Properties.Resources.TooShortEx, scanLines, characterCount));
            using (BinaryReader fnt = new BinaryReader(new FileStream(fntFile, FileMode.Open)))
            {
                for (int pattern = 0; pattern < table.Capacity; ++pattern)
                    for (int row = 0; row < scanLines; ++row)
                        table[pattern][row] = fnt.ReadByte();
                fnt.Close();
            }
        }

        public CharacterPattern(SerializationInfo info, StreamingContext context)
        {
            int n=0,h=1,c=1;
            try
            {
                n = (int)info.GetValue(PATTERN_COUNT_ID, typeof(int));
                h = (int)info.GetValue(SCAN_LINES_ID, typeof(int));
                c = (int)info.GetValue(WIDTH_ID, typeof(int));
            }
            catch (SystemException)
            {
                MessageBox.Show(String.Format("Wrong format!"),
                    "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (n == 0)
                {
                    n = 1;
                    h = 7;
                    c = 5;
                }
            }
            height = h;
            columns = c;
            byte[,] result = (byte[,])info.GetValue(TABLE_ID, typeof(byte[,]));

            table = new List<List<byte>>(result.GetLength(0));
            for (int pattern = 0; pattern < table.Capacity; ++pattern)
            {
                table.Add(new List<byte>(height));
                for (int row = 0; row < height; ++row)
                    table[pattern].Add(result[pattern, row]);
            }
        }

        public byte this[int pattern, int row]
        {
            get { return table[pattern][row]; }
            set {
                if (table[pattern][row] != value)
                {
                    table[pattern][row] = value;
                    FirePropertyChanged(new IndexedPropertyChangedEventArg(
                        System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4),
                        pattern, row));
                }
            }
        }

        public byte[] this[int pattern]
        {
            get { return table[pattern].ToArray(); }
            set
            {
                table[pattern] = new List<byte>(value);
                FirePropertyChanged(new IndexedPropertyChangedEventArg(
                    System.Reflection.MethodBase.GetCurrentMethod().Name.Substring(4),
                    pattern));
            }
        }

        public int PatternCount { get { return table.Capacity; } }

        public int ScanLines { get { return height; } }

        public void SaveToFile(string fileName)
        {
            using (BinaryWriter output = new BinaryWriter(new FileStream(fileName,
                FileMode.Create, FileAccess.Write)))
            {
                foreach (List<byte> pattern in table)
                    foreach (byte row in pattern)
                        output.Write(row);
                output.Close();
            }
        }

        public void SaveLCD2File(string fileName, exportHformat expFmt, bool defConst, bool defStatic, bool defPROGMEM, String defName, String defType = "byte")
        {
            // LCD user def. chars are 5x8 
            const int patHeight = 8;
            bool binary = (expFmt == exportHformat.BIN);

            using (StreamWriter outputFile = new StreamWriter(new FileStream(fileName,
                FileMode.Create, FileAccess.Write)))
            {
                outputFile.WriteLine("{0}{1}{2}{3} {4}[][{5}] = {{",
                    defConst ? "const " : "",
                    defStatic ? "static " : "",
                    defPROGMEM ? "PROGMEM " : "",
                    defType, defName, patHeight);

//                outputFile.WriteLine("const static PROGMEM byte customChar[][8] = {");
                foreach (List<byte> pattern in table)
                {
                    int r = 0;
                    if (binary)
                        outputFile.WriteLine("{");
                    else
                        outputFile.Write("{");
                    //                    foreach (byte row in pattern)
                    for (int i = 0; i < patHeight; i++)
                    {
                        byte mask = 0x10;
                        byte row;

                        if (i < height)
                            row = pattern[i];
                        else
                            row = 0;

                        if (binary)
                        {
                            outputFile.Write("0b");
                            for (int j = 0; j < 5; j++)
                            {
                                outputFile.Write(((row & mask) != 0) ? "1" : "0");
                                mask >>= 1;
                            }
                            if (r < (patHeight - 1))
                                outputFile.WriteLine(",");
                            else
                                outputFile.WriteLine("},");
                        }
                        else
                        {
                            if (expFmt == exportHformat.HEX)
                                outputFile.Write("0x{0:X}", row & 0x1f);
                            else
                                outputFile.Write("{0}", row & 0x1f);
                            if (r < (patHeight - 1))
                                outputFile.Write(",");
                            else
                                outputFile.WriteLine("},");
                        }
                        r++;
                    }
                }
                outputFile.WriteLine("};");
                outputFile.Close();
            }
        }

        public void SaveNokia5110File(string fileName, exportHformat expFmt, bool defConst, bool defStatic, bool defPROGMEM, String defName, String defType = "byte")
        {
            bool binary = (expFmt == exportHformat.BIN);
            // Nokia5110 chars are 5x7 ? 8?

            using (StreamWriter outputFile = new StreamWriter(new FileStream(fileName,
                FileMode.Create, FileAccess.Write)))
            {
                outputFile.WriteLine("{0}{1}{2}{3} {4}[][{5}] = {{",
                    defConst ? "const " : "",
                    defStatic ? "static " : "",
                    defPROGMEM ? "PROGMEM " : "",
                    defType, defName, columns);

//                outputFile.WriteLine("const static byte customChar[][5] = {");
                byte mask;
                foreach (List<byte> pattern in table)
                {
                    int c = 0;
                    if (binary)
                        outputFile.WriteLine("{");
                    else
                        outputFile.Write("{");

                    // column order - 5 columns left to right MSB at bottom
                    byte[] col = new byte[5] { 0, 0, 0, 0, 0 };
                    // foreach (byte row in pattern)
                    mask = 0x10;
                    for (int j = 0; j < 5; j++)
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            byte row = (i < 7) ? pattern[i] : (byte)0;
                            col[j] >>= 1;
                            if((row & mask) != 0)
                                col[j] |= 0x40;
                        }
                        mask >>= 1;
                    }

                    if (binary)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            outputFile.Write("0b");
                            mask = 0x80; // MSB set
                            while (mask!=0)
                            {
                                outputFile.Write(((col[j] & mask) != 0) ? "1" : "0");
                                mask >>= 1;
                            }
                            if (j < (5 - 1))
                                outputFile.WriteLine(",");
                            else
                                outputFile.WriteLine("},");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if(expFmt == exportHformat.HEX)
                                outputFile.Write("0x{0:X}", col[j]);
                            else
                                outputFile.Write("{0}", col[j]);
                            if (j < (5 - 1))
                                outputFile.Write(",");
                            else
                                outputFile.WriteLine("},");
                        }
                    }
                    //outputFile.WriteLine(",");
                    c++;
                }
                outputFile.WriteLine("};");
                outputFile.Close();
            }
        }

 
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            byte[,] output = new byte[table.Capacity, height];
            for (int pattern = 0; pattern < table.Capacity; ++pattern)
                for (int row = 0; row < height; ++row)
                    output[pattern, row] = table[pattern][row];
            info.AddValue(PATTERN_COUNT_ID, table.Capacity);
            info.AddValue(SCAN_LINES_ID, ScanLines);
            info.AddValue(WIDTH_ID, width);
            info.AddValue(TABLE_ID, output);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FirePropertyChanged(string propertyName)
        {
            FirePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        private void FirePropertyChanged(PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLineIf(System.Diagnostics.Debugger.IsAttached,
                String.Format("property \u201C{0}\u201D is changed", e.PropertyName), "Font Table");
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

    }
}

