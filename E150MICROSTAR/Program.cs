using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace E150MICROSTAR
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string arg = args[0];
            string[] argList = arg.Split(',');
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmActionMicroStar(argList));

        }
    }
}
