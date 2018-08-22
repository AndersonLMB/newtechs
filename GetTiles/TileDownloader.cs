using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GetTiles
{

    public static class Consts
    {
        public static List<double> GoogleScales = new List<double>() {
            1128.497176,
            2256.994353,
            4513.988705,
            9027.977411,
            18055.954822,
            36111.909643,
            72223.819286,
            144447.638572,
            288895.277144,
            577790.554289,
            1155581.108577,
            2311162.217155,
            4622324.434309,
            9244648.868618,
            18489297.737236,
            36978595.474472,
            73957190.948944,
            147914381.897889,
            295828763.795777,
            591657527.591555,
       };


    }

    public enum Projection
    {
        LonLat, SphericalMercator
    }

    public class TileDownloader
    {

        #region Properties
        public DownloadFileTasks DownloadFileTasks { get; set; }
        public string Servers { get; set; }
        public Extent Extent { get; set; }
        public Projection Projection { get; set; }
        #endregion

        #region Constructors
        public TileDownloader()
        {
            this.DownloadFileTasks = new DownloadFileTasks()
            {
                TasksCount = 0,
                TasksCompletedCount = 0
            };
            Servers = "0";
            this.Extent = new Extent()
            {
                xMin = -20026376.39,
                yMin = -20048966.10,
                xMax = 20026376.39,
                yMax = 20048966.10
            };
            this.Projection = Projection.SphericalMercator;

            //this.DownloadFileTasks.OnCompletedCountAdded += DownloadFileTasks_OnCompletedCountAdded;
        }
        #endregion

        #region Methods



        private void DownloadFileTasks_OnCompletedCountAdded()
        {
            Console.WriteLine();
            Console.CursorTop -= 1;
            Console.CursorLeft = 0;
            Console.WriteLine("                        ");
            Console.CursorTop -= 1;
            Console.CursorLeft = 0;
            Console.WriteLine("{0} / {1}", DownloadFileTasks.TasksCompletedCount, DownloadFileTasks.TasksCount);
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 所有下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="folder"></param>
        /// <param name="maxLevel"></param>
        /// <param name="minLevel"></param>
        public void DownloadToFolder(string url, string folder, int maxLevel, int minLevel)
        {

            Console.WriteLine("URL: {0}\nFolder: {1}\nLevel: {2}", url, folder, maxLevel);

            //清空文件夹
            var di = new DirectoryInfo(folder);
            foreach (var file in di.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (var dir in di.EnumerateDirectories())
            {
                dir.Delete(true);
            }

            for (var i = minLevel; i <= maxLevel; i++)
            {
                var levelDirectoryInfo = Directory.CreateDirectory(Path.Combine(di.FullName, String.Format("{0}", i)));

                DownloadLevelToFolder(url, levelDirectoryInfo.FullName, i);


            }




        }

        /// <summary>
        /// 下载整个等级的
        /// </summary>
        /// <param name="url"></param>
        /// <param name="levelDirectory"></param>
        /// <param name="level"></param>
        public void DownloadLevelToFolder(string url, string levelDirectory, int level)
        {
            var rowCount = Math.Pow(2, level);
            //each Y
            for (int i = 0; i < rowCount; i++)
            {
                var y = i;
                var yDir = Directory.CreateDirectory(Path.Combine(levelDirectory, String.Format("{0}", i)));

                DownloadRowToFolder(url, yDir.FullName, level, i);
            }
        }

        /// <summary>
        /// 整行下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="yDirectory"></param>
        /// <param name="level"></param>
        /// <param name="y"></param>
        public void DownloadRowToFolder(string url, string yDirectory, int level, int y)
        {
            var size = Math.Pow(2, level);
            for (int i = 0; i < size; i++)
            {
                var serversSplit = Servers.Split(',');
                int randomIndex = new Random().Next(0, serversSplit.Length);
                var randomServer = serversSplit[randomIndex];


                var pngUrl = String.Format(url, i, y, level, randomServer);

                var pngFileName = String.Format("{0}.png", i);
                var pngFileFullname = Path.Combine(yDirectory, pngFileName);

                DownloadToFile(pngUrl, pngFileFullname);
                //WebClient webClient = new WebClient();
                //webClient.DownloadFileAsync(new Uri(pngUrl), pngFileFullname);
                ;
            }
        }

        /// <summary>
        /// 将URL下载成文件
        /// </summary>
        /// <param name="pngUrl"></param>
        /// <param name="pngFileFullname"></param>
        public void DownloadToFile(string pngUrl, string pngFileFullname)
        {
            DownloadFileTasks.AddTask(pngUrl, pngFileFullname);
        }

        #endregion


        //public List<DownloadFileTask> downloadFileTasks = new List<DownloadFileTask>();


    }

    public class Extent
    {
        public double xMin { get; set; }
        public double yMin { get; set; }
        public double xMax { get; set; }
        public double yMax { get; set; }
    }

    public class DownloadFileTasks
    {
        public DownloadFileTasks()
        {
            this.DownloadedSize = 0;
            FailedCount = 0;


        }


        /// <summary>
        ///  任务数量
        /// </summary>
        public int TasksCount { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        private int _tasksCompletedCount = 0;

        public int TasksCompletedCount
        {
            get { return _tasksCompletedCount; }
            set
            {
                OnCompletedCountAdded?.Invoke(value, TasksCount);
                _tasksCompletedCount = value;
            }
        }

        public long DownloadedSize { get; set; }
        public int FailedCount { get; set; }
        public void AddTask(string pngUrl, string pngFileFullname)
        {
            this.TasksCount += 1;
            DownloadFileTask task = new DownloadFileTask()
            {
                PngUrl = pngUrl,
                PngFileFullname = pngFileFullname,
                Status = 0,
                Tasks = this,
            };
            //WebClient webClient = new WebClient();
            task.Download();
        }

        public event CompletedCountAdd OnCompletedCountAdded;

    }

    public delegate void CompletedCountAdd(int lastestCount, int total);


    public class DownloadFileTask
    {
        public int Status { get; set; }
        public string PngUrl { get; set; }
        public string PngFileFullname { get; set; }
        public DownloadFileTasks Tasks { get; set; }
        public void Download()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileAsync(new Uri(PngUrl), PngFileFullname);
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //Console.WriteLine((sender as WebClient).BaseAddress);
            if (e.Error != null)
            {
                Console.WriteLine("下载失败");
                Tasks.FailedCount += 1;
                // 下载失败
                Download();
            }

            else
            {
                this.Status = 1;
                this.Tasks.TasksCompletedCount += 1;
                //Console.CursorTop += 1;
                //Console.CursorLeft = 0;
                //Console.WriteLine("                                                  ");
                //Console.CursorTop += 1;
                //Console.CursorLeft = 0;
                var fi = new FileInfo(PngFileFullname);
                long length = fi.Length;
                this.Tasks.DownloadedSize += length;
                Console.WriteLine("{0} / {1} ", this.Tasks.TasksCompletedCount, this.Tasks.TasksCount);

                //成功
            }
            //throw new NotImplementedException();
        }
    }


}

