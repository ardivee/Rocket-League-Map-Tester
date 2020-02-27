using System;
using System.Collections.Generic;
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

namespace RLMapTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string mapPath = Properties.Settings.Default.MapFolder;
        public string modPath = Properties.Settings.Default.ModsFolder;

        public MainWindow()
        {
            InitializeComponent();

            UpdateMapList();
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

            var selectedMap = (ListViewItem)MapList.SelectedItem;

            if(selectedMap != null)
            {
                var map = (string)selectedMap.Tag;

                System.IO.File.Copy(map, System.IO.Path.Combine(modPath, "Labs_Underpass_P.upk"), true);

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
    }
}
