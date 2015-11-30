using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace CPUmon
{
    public partial class Form1 : Form
    { 
        //Create the notify icon used that's going to be displayed in the task tray
        NotifyIcon cpuIcon; 

        //Create all the icons
        Icon greyCpuIcon;
        Icon blueCpuIcon;
        Icon greenCpuIcon;
        Icon yellowCpuIcon;
        Icon redCpuIcon;

        ContextMenu contextMenu;

        //Create the thread used to pull data from the system mangement
        Thread getDataThread = new Thread(dataThread);

        //The int that's being used to hold the current cpu load
        private static int cpuLoad;

        public Form1()
        {
            InitializeComponent();
            //Import all of the icons from ico files
            greyCpuIcon = new Icon("cpu.ico", 16,16);
            blueCpuIcon = new Icon("cpu_blue.ico", 16, 16);
            greenCpuIcon = new Icon("cpu_green.ico", 16, 16);
            yellowCpuIcon = new Icon("cpu_yellow.ico", 16, 16);
            redCpuIcon = new Icon("cpu_red.ico", 16, 16);

            //Create the notifyicon and show it with the grey cpu icon
            cpuIcon = new NotifyIcon();
            cpuIcon.Icon = greyCpuIcon;
            cpuIcon.Visible = true;

            //Create the menu items that's going to be added to the icons menu and assign a method to the quit item
            MenuItem quitItem = new MenuItem("Quit");
            MenuItem appNameItem = new MenuItem("CPU Icon v.1");

            //Create the context menu, add the menuitems and add the contextmenu to the icon
            contextMenu = new ContextMenu();
            cpuIcon.ContextMenu = contextMenu;
            contextMenu.MenuItems.Add(quitItem);
            contextMenu.MenuItems.Add(appNameItem);

            quitItem.Click += QuitItem_Click;

            //Hide the main form and start the thread
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            getDataThread.Start();
            
            //Get the int with the current cpuload in and switch the current icon to the correct one
            while (true) {
                Thread.Sleep(100);
                //Use ifs instead of a switch because it's faster
                if (cpuLoad < 25) { cpuIcon.Icon = blueCpuIcon; } //0 - 25
                if (cpuLoad > 25 && cpuLoad < 50) { cpuIcon.Icon = greenCpuIcon; } //25 - 50
                if (cpuLoad > 50 && cpuLoad < 75) { cpuIcon.Icon = yellowCpuIcon; } //50 - 75
                if (cpuLoad > 75) { cpuIcon.Icon = redCpuIcon; } //75 - 100
            }
        }

        private static void dataThread(){
            PerformanceCounter cpuPerfCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            while (true) {
                Thread.Sleep(1000);
                cpuLoad = Convert.ToInt32(cpuPerfCounter.NextValue());
            }
        }

        private void QuitItem_Click(object sender, EventArgs e)
        {
            cpuIcon.Dispose();
            getDataThread.Abort();
            this.Close();
        }
    }
}
