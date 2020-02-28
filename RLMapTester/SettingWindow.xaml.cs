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
using System.Windows.Shapes;

namespace RLMapTester
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        MainWindow mainWindow;

        public SettingWindow(MainWindow window)
        {
            InitializeComponent();

            MapsFolder.Text = Properties.Settings.Default.MapFolder;
            ModsFolder.Text = Properties.Settings.Default.ModsFolder;

            mainWindow = window;

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.MapFolder = MapsFolder.Text;
            Properties.Settings.Default.ModsFolder = ModsFolder.Text;
            Properties.Settings.Default.Save();

            mainWindow.mapPath = MapsFolder.Text;
            mainWindow.modPath = ModsFolder.Text;

            MessageBox.Show("Saved");
        }

        private void SelectMapFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderSelectDialog
            {
                Title = "Select your Maps Folder"
            };

            if (dialog.Show())
            {
                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    MapsFolder.Text = dialog.FileName;
                }
            }
        }

        private void SelectModFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderSelectDialog
            {
                Title = "Select your Mods Folder"
            };

            if (dialog.Show())
            {
                if (!string.IsNullOrWhiteSpace(dialog.FileName))
                {
                    ModsFolder.Text = dialog.FileName;
                }
            }
        }
    }
}
