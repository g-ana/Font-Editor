/* Program.cs
 * Ана Ѓорѓевиќ, 2018
 * inspired by  Juhász, Ádám L.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Globalization;

namespace FntEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            int? scanLines = null;
            int? number = null;
            string file = String.Empty;

            //---------------------
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("mk-MK"); ;
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("mk-MK"); ;

            //---------------------

            if (args.Any(s => s.StartsWith("/size:", true, CultureInfo.CurrentCulture)))
            {
                string[] size = args.Last(s => s.StartsWith("/size:", true, CultureInfo.CurrentCulture)).Split(':');
                if (size.Length >= 3)
                {
                    try
                    {
                        scanLines = Convert.ToInt32(size[1]);
                        number = Convert.ToInt32(size[2]);
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Invalid size: \u201C{0}:{1}:{2}\u201D", size);
                        scanLines = number = null;
                    }
                }
                else if (size.Length == 2)
                {
                    try
                    {
                        scanLines = Convert.ToInt32(size[1]);
                        number = 256;
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Invalid size: \u201C{0}:{1}\u201D", size);
                        scanLines = number = null;
                    }
                }
                else
                {
                    Console.Error.WriteLine("Invalid size: \u201C{0}:{1}\u201D", size);
                }
            }

            try
            {
                file = args.First(s => !s.StartsWith("/"));
            }
            // This can happen, when no match was found.
            catch (InvalidOperationException) { }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (scanLines.HasValue && number.HasValue && !String.IsNullOrWhiteSpace(file))
                Application.Run(new TableForm(file, scanLines.Value, number.Value));
            else if (scanLines.HasValue && number.HasValue)
                Application.Run(new TableForm(scanLines.Value, number.Value));
            else if (!String.IsNullOrWhiteSpace(file))
                Application.Run(new TableForm(file));
            else
                Application.Run(new TableForm());
        }
    }
}
