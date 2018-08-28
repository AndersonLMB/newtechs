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
        #region Properties     
        public List<TileDownloaderTask> TileDownloaderTasks { get => tileDownloaderTasks; set => tileDownloaderTasks = value; }
        #endregion

        #region Constructors
        public TileDownloaderTasksManager()
        {
            TileDownloaderTasks = new List<TileDownloaderTask>();
        }
        #endregion


        private List<TileDownloaderTask> tileDownloaderTasks;



        public void Sth()
        {

        }

        public TileDownloaderTask AddTask(string url, string filename)
        {
            var downloadTask = new TileDownloaderTask()
            {
                Url = url,
                Filename = filename,
                TileDownloaderTasksManager = this
            };
            TileDownloaderTasks.Add(downloadTask);
            //downloadTask.Index = TileDownloaderTasks.IndexOf(downloadTask);
            return downloadTask;
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
            NotTryDownloadYet = true;
        }
        #endregion


        #region Properties
        public static int TotalCount { get; set; }
        public int Index { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
        public TileDownloaderTasksManager TileDownloaderTasksManager { get; set; }
        public int DownloadedCount { get; set; }
        public int MaxDownloadLimit { get; set; }
        public WebClient WebClient { get; set; }
        public bool NotTryDownloadYet { get; set; }
        public long Size { get; set; }

        #endregion

        #region Methods
        public Task TryDownload()
        {

            //TryDownloadActivated = true;
            if (OnDownloadTaskFirstStartDownload != null)
            {
                OnDownloadTaskFirstStartDownload(this);
            }
            NotTryDownloadYet = false;
            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }
            this.WebClient = new WebClient();
            //var task = webClient.DownloadFileTaskAsync(Url, Filename);
            //task.Wait()
            this.WebClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            var task = this.WebClient.DownloadFileTaskAsync(new Uri(Url), Filename);
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
                    var fi = new FileInfo(Filename);
                    var size = fi.Length;
                    this.Size = size;
                    OnDownloadTaskFinallyDownload(this, "SUCCESS");
                }
            }

            //throw new NotImplementedException();
        }
        #endregion

        public event DownloadTaskFinallyDownloadedDelegate OnDownloadTaskFinallyDownload;
        public event DownloadTaskFirstStartDownloadDelegate OnDownloadTaskFirstStartDownload;
    }

    public delegate void DownloadTaskFinallyDownloadedDelegate(TileDownloaderTask tileDownloaderTask, string message);
    public delegate void DownloadTaskFirstStartDownloadDelegate(TileDownloaderTask tileDownloaderTask);
    public class DownloadTaskFinallyDownloadedObject
    {

    }
}

