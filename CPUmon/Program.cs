using System;
using System.Windows.Forms;

namespace CPUmon
{
    static class Program
    {
        /*
        A simple program that creates an icon in the taskbar tray and displays the current cpu load 
        by showing a blue icon when the load is under 25%, a green when 25% - 50%, a yellow when 50 - 75% and a 
        red icon when 75% - 100%
        */

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


    }


}
