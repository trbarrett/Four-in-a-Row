using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FourRow
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Testing();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new UI.MainForm());
        }

        static void Testing() {
            var test = new Tests.Unit.AI.AIHelperTest();
            test.TestDoesTileHaveThreeDirectlyConnectingPlayerTokens_FilledOnOneEnd1();
        }
    }
}
