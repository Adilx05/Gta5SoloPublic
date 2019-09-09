using MahApps.Metro.Controls;
using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;
using System.Timers;

namespace Gta5SoloPublic
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private double value = 0;
        System.Timers.Timer t = new System.Timers.Timer();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("GTA5"))
                process.Suspend();

            value = 0;
            ProgressBarBaslat();
            ZamanlayiciBaslat();
        }

        public void ProgressBarBaslat()
        {
            t.Elapsed += new ElapsedEventHandler(UpdateProgress);
            t.Interval = 1000;
            t.Enabled = true;
        }

        private void UpdateProgress(object sender, ElapsedEventArgs e)
        {
            value += 100/14;
            if (value >= 100)
            {
                t.Stop();
            }

            if (CheckAccess())
                this.Pb_Durum.Value = value;
            else
            {
                Dispatcher.Invoke(new Action(() => { this.Pb_Durum.Value = value; }));
            }
        }

        private void ZamanlayiciBaslat()
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(ZamanlayiciIslemAsync);
            aTimer.Interval = 15000;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private void ZamanlayiciIslemAsync(object source, ElapsedEventArgs a)
        {
            

            foreach (var process in Process.GetProcessesByName("GTA5"))
                process.Resume();

            Dispatcher.Invoke(new Action(() =>
            {
                this.ShowMessageAsync("Bilgi", "Solo Public Serverınız Açıldı", MessageDialogStyle.Affirmative);
            }));
        }

        
        public void UpdateProgressBar(int value)
        {
            
        }
    }
}
