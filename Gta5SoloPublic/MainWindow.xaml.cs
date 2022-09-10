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
using System.Windows.Forms;

namespace Gta5SoloPublic
{
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool isKayitMode = false;
        private System.Windows.Forms.Keys saveKey;
        private double value = 0;
        private bool isSaveAcik;
        System.Timers.Timer t = new System.Timers.Timer();
        public MainWindow()
        {
            InitializeComponent();
            saveKey = Properties.Settings.Default.SaveTus;
            isSaveAcik = Properties.Settings.Default.SaveAcik;
            SzninItligi.IsChecked = isSaveAcik;
            SaveBt.IsEnabled = isSaveAcik;
            DinlenecekTuslariAyarla();
            SavedLabel.Content = "Şu anki secili tuş : " + Properties.Settings.Default.SaveTus.ToString();

        }

        kzhook klavyeDinleyicisi = new kzhook();

        private void DinlenecekTuslariAyarla()
        {
            klavyeDinleyicisi.HookedKeys.Add(saveKey);


            //basıldığında ilk burası çalışır
            klavyeDinleyicisi.KeyDown += new System.Windows.Forms.KeyEventHandler(islem1);
            //basıldıktan sonra ikinci olarak burası çalışır
            klavyeDinleyicisi.KeyUp += new System.Windows.Forms.KeyEventHandler(islem2);
        }

        void islem1(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == saveKey && isSaveAcik)
            {
                foreach (var process in Process.GetProcessesByName("GTA5"))
                    process.Suspend();

                value = 0;
                ProgressBarBaslat();
                ZamanlayiciBaslat();

                //this.ShowMessageAsync("Okunan Tuş",e.KeyCode.ToString());
            }
            e.Handled = false;
        }

        void islem2(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            //Yapılmasını istediğiniz kodlar burada yer alacak
            // Burası ilgili tuşlara basılıp çekildikten sonra çalışır



            //Eğer buraya gelecek olan tuşa basıldığında
            //o tuşun normal işlevi yine çalışsın istiyorsanız
            //e.Handled değeri false olmalı
            //eğer ilgili tuşa basıldığında burada yakalansın
            // ve devamında tuş başka bir işlev gerçekleştirmesin
            //istiyorsanız bu değeri true yapmalısınız
            e.Handled = true;
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
            value += 100 / 14;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            isKayitMode = true;
            SaveBt.Background = Brushes.DarkRed;
        }

        private void MetroWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (isKayitMode)
            {
                this.ShowMessageAsync("Kaydedilen Tuş", e.Key.ToString());
                if (Enum.TryParse(e.Key.ToString(), out System.Windows.Forms.Keys winFormsKey))
                {
                    saveKey = winFormsKey;
                }
                Properties.Settings.Default.SaveTus = saveKey;
                Properties.Settings.Default.Save();
                SavedLabel.Content = "Şu anki secili tuş : " + Properties.Settings.Default.SaveTus.ToString();
                SaveBt.Background = Brushes.Turquoise;
                HookDegistir();
                isKayitMode = false;
            }
        }

        private void HookDegistir()
        {
            klavyeDinleyicisi.HookedKeys.Clear();
            klavyeDinleyicisi.KeyDown -= new System.Windows.Forms.KeyEventHandler(islem1);
            klavyeDinleyicisi.KeyDown -= new System.Windows.Forms.KeyEventHandler(islem2);
            klavyeDinleyicisi.HookedKeys.Add(saveKey);
            //basıldığında ilk burası çalışır
            klavyeDinleyicisi.KeyDown += new System.Windows.Forms.KeyEventHandler(islem1);
            //basıldıktan sonra ikinci olarak burası çalışır
            klavyeDinleyicisi.KeyUp += new System.Windows.Forms.KeyEventHandler(islem2);
        }

        private void SzninItligi_Unchecked(object sender, RoutedEventArgs e)
        {
            SaveBt.IsEnabled = false;
            isSaveAcik = false;
            Properties.Settings.Default.SaveAcik = false;
            Properties.Settings.Default.Save();
        }

        private void SzninItligi_Checked(object sender, RoutedEventArgs e)
        {
            SaveBt.IsEnabled = true;
            isSaveAcik = true;
            Properties.Settings.Default.SaveAcik = true;
            Properties.Settings.Default.Save();
        }
    }
}
