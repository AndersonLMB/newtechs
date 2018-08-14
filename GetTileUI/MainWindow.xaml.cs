﻿using System;
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
using GetTiles;

namespace GetTileUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DateTime lastClickTime { get; set; }

        public TileDownloader TD { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lastClickTime = DateTime.Now;
            var td = new TileDownloader();
            this.TD = td;
            td.DownloadToFolder(StringUtils.XyzTo012(UrlTextbox.Text.ToString()), FolderTextbox.Text.ToString(), int.Parse(LevelsTextbox.Text.ToString()));
            td.DownloadFileTasks.OnCompletedCountAdded += DownloadFileTasks_OnCompletedCountAdded;
        }

        private void DownloadFileTasks_OnCompletedCountAdded(int lastestCount, int total)
        {

            DownloadProgressTextbox.Text = String.Format("{0} / {1}", lastestCount, total);

            //string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            //double len = this.TD.DownloadFileTasks.DownloadedSize;
            //int order = 0;
            //while (len >= 1024 && order < sizes.Length - 1)
            //{
            //    order++;
            //    len = len / 1024;
            //}
            //string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            DownloadedSizeTextbox.Text = StringUtils.SizeToReadable(this.TD.DownloadFileTasks.DownloadedSize);

            double ratio = ((double)lastestCount / (double)total);


            EstimateSizeTextbox.Text = StringUtils.SizeToReadable((int)((double)TD.DownloadFileTasks.DownloadedSize / ratio)).ToString();
            AverageSizeTextblock.Text = StringUtils.SizeToReadable((long)(((double)(TD.DownloadFileTasks.DownloadedSize)) / ((double)lastestCount)));
            FailedCountTextblock.Text = TD.DownloadFileTasks.FailedCount.ToString();
            //DownloadedSizeTextbox.Text = String.Format("{0} KB", (this.TD.DownloadFileTasks.DownloadedSize / 1024).ToString());
            DownloadProgressbar.Maximum = total;
            DownloadProgressbar.Value = lastestCount;
            UsedTimeTextblock.Text = DateTime.Now.Subtract(lastClickTime).ToString();
            //throw new NotImplementedException();
        }
    }
}
