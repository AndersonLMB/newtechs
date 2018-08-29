using GetTiles;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace GetTileUI
{
    /// <summary>
    /// Interaction logic for TileDownloadUI.xaml
    /// </summary>
    public partial class TileDownloadUI : Window
    {
        public TileDownloaderTasksManager Manager { get; set; }
        public int TaskTotalCount { get; set; }
        public long DownloadedSize { get; set; }

        public TileDownloadUI()
        {
            InitializeComponent();
            var directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "pzhtdt");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            FolderDirectoryInputTextBox.Text = directoryPath;
            DownloadedSize = 0;
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
            if (ProjectionSelect.SelectedItem == WebMctCbi)
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
                #region 亲测这三行代码顺序不能交换
                td2.OnTileDownloadTasksAllCreated += Td2_OnTileDownloadTasksAllCreated;
                td2.TilesDownloadExtent = new DoubleExtent(ExtentInputTextBox.Text);
                td2.DownloadByResolutionIndexRange(Convert.ToInt32(MinzoomInputTextBox.Text), Convert.ToInt32(MaxzoomInputTextBox.Text), BoolCreateDirectory.IsChecked.Value);
                #endregion

            }

            else
            {
                var td2 = new TileDownloader2();
                td2.LonlatInit();
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
                #region 亲测这三行代码顺序不能交换
                td2.OnTileDownloadTasksAllCreated += Td2_OnTileDownloadTasksAllCreated;
                td2.TilesDownloadExtent = new DoubleExtent(ExtentInputTextBox.Text);
                td2.DownloadByResolutionIndexRange(Convert.ToInt32(MinzoomInputTextBox.Text), Convert.ToInt32(MaxzoomInputTextBox.Text), BoolCreateDirectory.IsChecked.Value);
                #endregion
            }


        }

        /// <summary>
        /// 所有任务创建完毕，提交到TileDownloaderTasksManager托管后执行
        /// </summary>
        /// <param name="tileDownloader"></param>
        /// <param name="manager"></param>
        private void Td2_OnTileDownloadTasksAllCreated(TileDownloader2 tileDownloader, TileDownloaderTasksManager manager)
        {

            Manager = manager;
            //TileDownloadTaskManagerDataGrid.ItemsSource = manager.TileDownloaderTasks;
            TaskTotalCount = Manager.TileDownloaderTasks.Count;
            TileDownloadTaskManagerListView.ItemsSource = manager.TileDownloaderTasks;
            DownloadProgressbar.Maximum = TaskTotalCount;
            ProgressTextblock.Text = String.Format("{0} / {1}", 0, TaskTotalCount);
            manager.TileDownloaderTasks.ForEach((a) =>
            {
                a.OnDownloadTaskFinallyDownload += (b, c) =>
                {
                    Manager.TileDownloaderTasks.Remove(a);
                    TileDownloadTaskManagerListView.ItemsSource = Manager.TileDownloaderTasks;
                    TileDownloadTaskManagerListView.Items.Refresh();
                    var downloadedCount = TaskTotalCount - Manager.TileDownloaderTasks.Count;
                    ProgressTextblock.Text = String.Format("{0} / {1}", downloadedCount, TaskTotalCount);
                    DownloadProgressbar.Value = downloadedCount;
                    DownloadedSize += a.Size;
                    DownloadSizeTextblock.Text = StringUtils.SizeToReadable(DownloadedSize).ToString();
                    var estimateSize = ((double)DownloadedSize) / ((double)downloadedCount / (double)TaskTotalCount);
                    EstimatedSizeTextblock.Text = StringUtils.SizeToReadable(Convert.ToInt64(estimateSize));
                };
            });
        }

        private void EachTileDownloaderTaskCreatedExecute(TileDownloaderTask a)
        {
            a.OnDownloadTaskFinallyDownload += (b, c) =>
            {
                Manager.TileDownloaderTasks.Remove(a);
                TileDownloadTaskManagerListView.ItemsSource = Manager.TileDownloaderTasks;
                TileDownloadTaskManagerListView.Items.Refresh();
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var downloaderTask = button.DataContext as TileDownloaderTask;
            var task = downloaderTask.TryDownload();
            button.IsEnabled = false;
        }

        /// <summary>
        /// 单个任务下载按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadAllButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.TileDownloaderTasks.ForEach((a) =>
            {
                a.TryDownload();
            });
            ConfirmButton.IsEnabled = false;
            DownloadAllButton.IsEnabled = false;
        }

        /// <summary>
        /// Preview按钮点击之后，用系统浏览器打开URL预览图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var downloaderTask = button.DataContext as TileDownloaderTask;
            System.Diagnostics.Process.Start(downloaderTask.Url);
        }

        private void TrySearchButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            button.IsEnabled = false;
            //Thread.Sleep(100);
            var admName = TrySearchAdmAreaName.Text;
            var lonlatExtentString = GeoUtils.GetLonLatExtentStringFromAdministrativeByTdt(admName);
            if (ProjectionSelect.SelectedItem == WebMctCbi)
            {
                var webMctExtentString = GeoUtils.LonLatExtentStringToWebMctExtentString(lonlatExtentString);
                ExtentInputTextBox.Text = webMctExtentString;
            }
            else
            {
                ExtentInputTextBox.Text = lonlatExtentString;
            }

            button.IsEnabled = true;


        }
    }
}
