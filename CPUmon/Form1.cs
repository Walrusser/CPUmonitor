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

        //The menu items to be placed in the contextmenu of the icon
        MenuItem quitItem;
        MenuItem appNameItem;

        //The thread that's going to get the cpu load info and display it
        Thread iconUpdateThread;

        //The rate of the thread's going to get new data in ms
        private static int updateRate = 100;

        public Form1()
        {

        
            InitializeComponent();
            //Import all of the icons from ico files
            greyCpuIcon = new Icon("Icons/cpu.ico", 16,16);
            blueCpuIcon = new Icon("Icons/cpu_blue.ico", 16, 16);
            greenCpuIcon = new Icon("Icons/cpu_green.ico", 16, 16);
            yellowCpuIcon = new Icon("Icons/cpu_yellow.ico", 16, 16);
            redCpuIcon = new Icon("Icons/cpu_red.ico", 16, 16);

            //Create the menu items that's going to be added to the icons menu and assign a method to the quit item
            quitItem = new MenuItem("Quit");
            appNameItem = new MenuItem("CPU Icon v.1");

            //Create the context menu, add the menuitems and add the contextmenu to the icon
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(appNameItem);
            contextMenu.MenuItems.Add(quitItem);

            quitItem.Click += QuitItem_Click;

            //Create the notifyicon and show it with the grey cpu icon
            cpuIcon = new NotifyIcon();
            cpuIcon.ContextMenu = contextMenu;
            cpuIcon.Icon = greyCpuIcon;
            cpuIcon.Visible = true;


            //Hide the main form and start the thread
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            iconUpdateThread = new Thread(IconUpdateThread);
            iconUpdateThread.Start();
        }

        private void QuitItem_Click(object sender, EventArgs e)
        {
            //Closes the form, exits the application and deletes the notifyicon
            Application.Exit();
            this.Close();
            iconUpdateThread.Abort();
            cpuIcon.Dispose();
        }

        public void IconUpdateThread() {

            //Creates the performancecounter that checks for the current cpuload from windows and creates and int to hold it
            PerformanceCounter perfCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            int currentCpuLoad;

            //Sleeps, gets the current load and converts it into a icon switch
            while (true) {
                Thread.Sleep(updateRate);

                currentCpuLoad = Convert.ToInt32(perfCounter.NextValue());

                if (currentCpuLoad < 25) { cpuIcon.Icon = blueCpuIcon; } //0 - 25
                if (currentCpuLoad > 25 && currentCpuLoad < 50) { cpuIcon.Icon = greenCpuIcon; } //25 - 50
                if (currentCpuLoad > 50 && currentCpuLoad < 75) { cpuIcon.Icon = yellowCpuIcon; } //50 - 75
                if (currentCpuLoad > 75) { cpuIcon.Icon = redCpuIcon; } //75 - 100
            }
        }
    }
}
