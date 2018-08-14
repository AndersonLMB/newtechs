using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GetTiles
{
    public class TileDownloader
    {
        public async Task DownloadToFolderAsync(string url, string folder, int maxLevel)
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

            for (var i = 1; i <= maxLevel; i++)
            {
                var levelDirectoryInfo = Directory.CreateDirectory(Path.Combine(di.FullName, String.Format("L{0}", i)));

                DownloadLevelToFolder(url, levelDirectoryInfo.FullName, i);


            }




        }

        public void DownloadLevelToFolder(string url, string levelDirectory, int level)
        {
            var size = Math.Pow(2, level);
            //each Y
            for (int i = 0; i < size; i++)
            {
                var y = i;
                var yDir = Directory.CreateDirectory(Path.Combine(levelDirectory, String.Format("Y{0}", i)));

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
                var pngUrl = String.Format(url, i, y, level);
                WebClient webClient = new WebClient();
                var pngFileName = String.Format("X{0}.png", i);
                var pngFileFullname = Path.Combine(yDirectory, pngFileName);
                webClient.DownloadFileAsync(new Uri(pngUrl), pngFileFullname);
                ;
            }






        }




    }



}

