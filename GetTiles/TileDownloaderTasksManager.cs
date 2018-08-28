using System.Collections.Generic;
using System.ServiceModel;
using System.Net;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GetTiles
{
    public class TileDownloaderTasksManager
    {
        private List<TileDownloaderTask> tileDownloaderTasks;
        public void Sth()
        {

        }

        public TileDownloaderTask AddTask(string url, string filename)
        {

            return null;
        }

        private void AddTask(TileDownloaderTask tileDownloaderTask)
        {

        }


    }
    public class TileDownloaderTask
    {

        #region Construcotrs
        public TileDownloaderTask()
        {
            MaxDownloadLimit = 2;
            DownloadedCount = 0;
        }
        #endregion


        #region Properties
        public TileDownloaderTasksManager TileDownloaderTasksManager { get; set; }
        public int DownloadedCount { get; set; }
        public int MaxDownloadLimit { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
        #endregion

        #region Methods
        public Task TryDownload()
        {
            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }
            WebClient webClient = new WebClient();
            //var task = webClient.DownloadFileTaskAsync(Url, Filename);
            //task.Wait()
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            var task = webClient.DownloadFileTaskAsync(new Uri(Url), Filename);
            //Trace.WriteLine(task.Id);
            return task;
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadedCount += 1;
            //报错
            if (e.Error != null)
            {
                //没有超次数
                if (DownloadedCount < MaxDownloadLimit)
                {
                    TryDownload();
                }
                //超过次数
                else
                {
                    if (OnDownloadTaskFinallyDownload != null)
                    {
                        OnDownloadTaskFinallyDownload(this, "FAILED");
                    }
                }
            }
            else
            {
                if (OnDownloadTaskFinallyDownload != null)
                {
                    OnDownloadTaskFinallyDownload(this, "SUCCESS");
                }
            }

            //throw new NotImplementedException();
        }
        #endregion

        public event DownloadTaskFinallyDownloadedDelegate OnDownloadTaskFinallyDownload;
    }

    public delegate void DownloadTaskFinallyDownloadedDelegate(TileDownloaderTask tileDownloaderTask, string message);

    public class DownloadTaskFinallyDownloadedObject
    {

    }
}

