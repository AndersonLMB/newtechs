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
using System.Windows.Forms;
using System.IO;
using GetTiles;

namespace GetTileUI
{
    /// <summary>
    /// Interaction logic for TileDownloadUI.xaml
    /// </summary>
    public partial class TileDownloadUI : Window
    {
        public TileDownloadUI()
        {
            InitializeComponent();
        }

        private void FolderSelectButton_Click(object sender, RoutedEventArgs e)
        {
            using (var fbdd = new FolderBrowserDialog())
            {
                if (Directory.Exists(FolderDirectoryInputTextBox.Text))
                {

                    fbdd.SelectedPath = FolderDirectoryInputTextBox.Text;
                }
                var result = fbdd.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.OK)
                {
                    FolderDirectoryInputTextBox.Text = fbdd.SelectedPath;
                }
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var td2 = new TileDownloader2();
            td2.CommonInit();
            td2.DownloadFolderPath = FolderDirectoryInputTextBox.Text;
            td2.UrlTemplate = UrlInputTextBox.Text;
            td2.Servers = ServersInputTextBox.Text;
            var di = new DirectoryInfo(td2.DownloadFolderPath);
            foreach (var file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (var dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }

            //11208658.152227892
            //2912864.3309648014
            //11549272.899636596
            //3212118.7161881635

            //11288373.211211396, 3039098.0261044647, 11374899.927230217, 3104145.9371751496
            //11304730.735264424, 3049264.1508663935, 11362364.254591448, 3093062.5680737994
            //td2.TilesDownloadExtent = new DoubleExtent()
            //{
            //    xMin = 11288373,
            //    yMin = 3039098,
            //    xMax = 11374899,
            //    yMax = 3104145
            //};
            td2.TilesDownloadExtent = new DoubleExtent(ExtentInputTextBox.Text);
            td2.DownloadByResolutionIndexRange(Convert.ToInt32(MinzoomInputTextBox.Text), Convert.ToInt32(MaxzoomInputTextBox.Text), false);
        }
    }
}
