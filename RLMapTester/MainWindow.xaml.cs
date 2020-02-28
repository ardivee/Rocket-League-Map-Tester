using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
using WinForm = System.Windows.Forms;

namespace RLMapTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string mapPath = Properties.Settings.Default.MapFolder;
        public string modPath = Properties.Settings.Default.ModsFolder;

        private bool isWatchingMap = false;
        private string selectedMap = null;

        WinForm.NotifyIcon notifyIcon = new WinForm.NotifyIcon();

        System.IO.FileSystemWatcher watcher = new System.IO.FileSystemWatcher();

        public MainWindow()
        {
            InitializeComponent();

            UpdateMapList();

            notifyIcon.Icon = new Icon(@"../../rltest_logo_fav.ico");
            notifyIcon.Visible = true;

            watcher.Renamed += Watcher_Renamed;
        }

        private void Watcher_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            if (!System.IO.Directory.Exists(modPath))
            {
                notifyIcon.ShowBalloonTip(500, "Rocket League Map Tester", "Mod folder not set!", WinForm.ToolTipIcon.Warning);
                return;
            }

            if (selectedMap != null)
            {
                System.IO.File.Copy(selectedMap, System.IO.Path.Combine(modPath, "Labs_Underpass_P.upk"), true);
                notifyIcon.ShowBalloonTip(500, "Rocket League Map Tester", "Map Updated", WinForm.ToolTipIcon.Info);
            }
        }

        private void UpdateMapList()
        {
            if (!System.IO.Directory.Exists(mapPath))
                return;

            MapList.Items.Clear();

            var maps = System.IO.Directory.EnumerateFiles(mapPath, "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".udk") || s.EndsWith(".upk"));

            foreach (string map in maps)
            {
                var mapname = System.IO.Path.GetFileNameWithoutExtension(map);
                var lvi = new ListViewItem();
                lvi.Content = mapname;
                lvi.Tag = map;
                MapList.Items.Add(lvi);
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateMapList();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.Directory.Exists(modPath))
            {
                MessageBox.Show("Mod folder not set!");
                return;
            }

            if(selectedMap != null)
            {
                System.IO.File.Copy(selectedMap, System.IO.Path.Combine(modPath, "Labs_Underpass_P.upk"), true);

                MessageBox.Show("Copied!");
            } else
            {
                MessageBox.Show("No map selected!");
            }
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new SettingWindow(this);
            window.Show();
        }

        private void WatchBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var selectedMap = (ListViewItem)MapList.SelectedItem;

            if(selectedMap != null)
            {
                if(!isWatchingMap)
                {
                    notifyIcon.ShowBalloonTip(500, "Rocket League Map Tester", "Watching Map", WinForm.ToolTipIcon.Info);

                    btn.Content = "Stop Watching";
                    isWatchingMap = true;

                    var map = (string)selectedMap.Tag;
                    watcher.Path = System.IO.Path.GetDirectoryName(map);
                    watcher.Filter = System.IO.Path.GetFileName(map);

                    watcher.EnableRaisingEvents = true;
                } else
                {
                    notifyIcon.ShowBalloonTip(500, "Rocket League Map Tester", "Stopped Watching", WinForm.ToolTipIcon.Info);

                    btn.Content = "Watch Map";
                    isWatchingMap = false;

                    watcher.EnableRaisingEvents = false;
                }
            } else
            {
                MessageBox.Show("No map selected!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Icon.Dispose();
        }

        private void MapList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ListViewItem)MapList.SelectedItem;

            if(item != null)
            {
                selectedMap = (string)item.Tag;
            }
        }
    }
}
